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
            this._connectionPath = AppConfig.Configuration.GetConnectionString(ServicesConstants.SQlite.Settings.APP_SETTING_KEY) ?? 
                throw new KeyNotFoundException(ServicesLogs.SQLite.ConfigExceptions.PATH_NOT_IN_CONF);
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

            this._sqliteConnection = new SqliteConnection(builder.ConnectionString);
            await _sqliteConnection.OpenAsync(cancellationToken);
            this._logger.LogInformation(ServicesLogs.SQLite.CONNECTION_OPENED, builder.DataSource);

            await InitializeDatabaseAsync();
        }

        public async Task InitializeDatabaseAsync()
        {
            string initScript = this.GetTablesQuery();
            await this.ExecuteAsync(initScript);
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
            this._logger.LogInformation(ServicesLogs.SQLite.DB_DISPOSED);
        }

        // --------------------private/helper functions------------------
        // ==================== Device & Channel Operations ====================

        public async Task<int> InsertDeviceAsync(string deviceName)
        {
            return await ExecuteAsync(
                ServicesConstants.SQlite.Queries.INSERT_DEVICE,
                new { DeviceName = deviceName });
        }

        public async Task<IEnumerable<string>> GetAllDevicesAsync()
        {
            return await QueryAsync<string>(ServicesConstants.SQlite.Queries.GET_ALL_DEVICES);
        }
        public async Task<int> InsertChannelAsync(string type, int simId, string deviceName)
        {
            return await ExecuteAsync(
                ServicesConstants.SQlite.Queries.INSERT_CHANNEL,
                new { Type = type, SimId = simId, DeviceName = deviceName });
        }
        // --------------------private/helper functions-------------------
        private string GetTablesQuery() => ServicesConstants.SQlite.Queries.INIT_DB;
    }
}