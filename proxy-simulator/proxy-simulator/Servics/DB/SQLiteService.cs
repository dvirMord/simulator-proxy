using Dapper;
using Microsoft.Data.Sqlite;
using multimedia_simulator.Interfaces;
using proxy_simulator.Config;
using proxy_simulator.Constants;

namespace proxy_simulator.Services
{
    public class SQLiteService : IDBService, IAsyncDisposable
    {
        private const string SETTING_DB = "SQLiteDbPath";
        private const string DEFUALT_PATH = "simulator-proxy.db";
        private readonly ILogger<SQLiteService> _logger;
        private string DEFUALT_DB_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFUALT_PATH);

        //============properites=======================
        private string _connectionPath;
        private SqliteConnection _sqliteConnection = null!;
        private readonly SemaphoreSlim _semaphoreLock = new SemaphoreSlim(initialCount: 1, maxCount: 1); // To ensure thread safety for database operations

        // --------------------constructors----------------
        public SQLiteService(string connectionPath, ILogger<SQLiteService> logger)
        {
           this._connectionPath = connectionPath;
            this._logger = logger;
        }
        public SQLiteService(ILogger<SQLiteService> logger)
        {
            this._connectionPath = AppConfig.Configuration.GetConnectionString(SETTING_DB) ?? 
                throw new KeyNotFoundException(DBConstants.ConfigExceptions.PATH_NOT_IN_CONF);
            this._logger = logger;
        }

        //--------------------interface functions-------------------
        public async Task CreateConnectionAndInitialize(CancellationToken cancellationToken = default)
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder
            {
                DataSource = this._connectionPath,
                ForeignKeys = true
            };
            this._sqliteConnection = new SqliteConnection(builder.ConnectionString);
            await this._sqliteConnection.OpenAsync(cancellationToken);
            await this.InitializeDatabaseAsync();
        }

        public async Task InitializeDatabaseAsync()
        {
            string initScript = this.AllTables();
            await this.ExecuteAsync(initScript);
            this._logger.LogInformation("[SQLiteService] DB is ready!");
        }

        public async Task CloseConnection()
        {
            await this.DisposeAsync();
        }

        //--------------------Dapper wrapper functions(sqlite commands)-------------------
        public async Task<int> ExecuteAsync(string query, object? parameters = null)
        {
            await this._semaphoreLock.WaitAsync();
            try
            {
                return await this._sqliteConnection.ExecuteAsync(query, parameters);
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
                return await this._sqliteConnection.QueryAsync<T>(query, parameters);
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
                return await this._sqliteConnection.QuerySingleOrDefaultAsync<T>(query, parameters);
            }
            finally
            {
                _semaphoreLock.Release();
            }
        }
        //-----------------------------------------------------------------------------------

        //==================================Exit=============================================
        public async ValueTask DisposeAsync()
        { 
            if(this._sqliteConnection is not null)
            {
                await this._sqliteConnection.CloseAsync();
                await this._sqliteConnection.DisposeAsync();
                this._sqliteConnection = null!;
            }
            this._semaphoreLock.Dispose();
            this._logger.LogInformation("[SQLiteService] DB is closed and disposed.");
        }
        
        // --------------------private/helper functions-------------------
        private string AllTables()
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
                    DeviceName TEXT NOT NULL,s

                    CONSTRAINT FK_Channels_Devices FOREIGN KEY (DeviceName) 
                        REFERENCES Devices(Name) ON DELETE CASCADE
                );

                CREATE INDEX IF NOT EXISTS IX_Channels_DeviceName ON Channels (DeviceName);
            ";

            return initScript;
        }
    }
}
