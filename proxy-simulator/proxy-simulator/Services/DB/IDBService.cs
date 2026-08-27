public interface IDBService
{
    Task<int> ExecuteAsync(string query, object? parameters = null);
    Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null);
    Task<T?> QuerySingleOrDefaultAsync<T>(string query, object? parameters = null);
    Task CreateConnectionAndInitialize(CancellationToken cancellationToken = default);
    Task InitializeDatabaseAsync();
    Task CloseConnection();
    Task<int> InsertDeviceAsync(string deviceName);
    Task<int> InsertChannelAsync(string type, int simId, string deviceName);

    Task<IEnumerable<string>> GetAllDevicesAsync();
}