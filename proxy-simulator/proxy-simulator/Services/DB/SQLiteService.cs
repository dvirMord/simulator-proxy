using Dapper;
using Microsoft.Data.Sqlite;
using proxy_simulator.Interfaces;
using proxy_simulator.Config;
using proxy_simulator.Constants;

namespace proxy_simulator.Services
{
    public class SQLiteService : IDBService, IAsyncDisposable
    {
        
        private readonly ILogger<SQLiteService> _logger;

        //============properties=======================
        private readonly string _connectionPath;
        private SqliteConnection _sqliteConnection = null!;
        private readonly SemaphoreSlim _semaphoreLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        // --------------------constructors----------------
        public SQLiteService(string connectionPath, ILogger<SQLiteService> logger)
        {
            _connectionPath = connectionPath;
            _logger = logger;
        }

        public SQLiteService(ILogger<SQLiteService> logger)
        {
            this._connectionPath = AppConfig.Configuration.GetConnectionString(DBConstants.Settings.APP_SETTING_KEY) ?? 
                throw new KeyNotFoundException(DBConstants.ConfigExceptions.PATH_NOT_IN_CONF);
            this._logger = logger;
        }

        //--------------------interface functions-------------------
        public async Task CreateConnectionAndInitialize(CancellationToken cancellationToken = default)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = _connectionPath,
                ForeignKeys = true
            };

            _sqliteConnection = new SqliteConnection(builder.ConnectionString);
            await _sqliteConnection.OpenAsync(cancellationToken);
            _logger.LogInformation(ServicesLogs.SQLite.CONNECTION_OPENED, builder.DataSource);

            await InitializeDatabaseAsync();
        }

        public async Task InitializeDatabaseAsync()
        {
            string initScript = this.GetTablesQuery();
            await this.ExecuteAsync(initScript);
            this._logger.LogInformation(DBConstants.Logs.SUCCESSFULLY_READY_LOG);
        }

        public async Task CloseConnection()
        {
            await DisposeAsync();
        }

        //--------------------Dapper wrapper functions(sqlite commands)-------------------
        public async Task<int> ExecuteAsync(string query, object? parameters = null)
        {
            await _semaphoreLock.WaitAsync();
            try
            {
                return await _sqliteConnection.ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ServicesLogs.SQLite.DB_OPERATION_FAILED, query);
                throw;
            }
            finally
            {
                _semaphoreLock.Release();
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null)
        {
            await _semaphoreLock.WaitAsync();
            try
            {
                return await _sqliteConnection.QueryAsync<T>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ServicesLogs.SQLite.DB_OPERATION_FAILED, query);
                throw;
            }
            finally
            {
                _semaphoreLock.Release();
            }
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string query, object? parameters = null)
        {
            await _semaphoreLock.WaitAsync();
            try
            {
                return await _sqliteConnection.QuerySingleOrDefaultAsync<T>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ServicesLogs.SQLite.DB_OPERATION_FAILED, query);
                throw;
            }
            finally
            {
                _semaphoreLock.Release();
            }
        }

        //==================================Exit=============================================
        public async ValueTask DisposeAsync()
        {
            if (_sqliteConnection is not null)
            {
                await _sqliteConnection.CloseAsync();
                await _sqliteConnection.DisposeAsync();
                _sqliteConnection = null!;
            }
            this._semaphoreLock.Dispose();
            this._logger.LogInformation(DBConstants.Logs.SUCCESSFULLY_CLEAR_N_DISPOSE_LOG_);
        }

        // --------------------private/helper functions-------------------
        private string GetTablesQuery()
        {
            const string initScript = @"
                PRAGMA journal_mode = WAL;
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS Devices (
                    Name TEXT PRIMARY KEY
                );

                CREATE TABLE IF NOT EXISTS Channels (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Type TEXT NOT NULL,
                    SimId INTEGER NOT NULL,
                    DeviceName TEXT NOT NULL,

                    CONSTRAINT FK_Channels_Devices FOREIGN KEY (DeviceName) 
                        REFERENCES Devices(Name) ON DELETE CASCADE
                );

                CREATE INDEX IF NOT EXISTS IX_Channels_DeviceName ON Channels (DeviceName);
            ";

            return initScript;
        }
    }
}