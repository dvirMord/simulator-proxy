namespace proxy_simulator.Interfaces;

public interface IDBService
{
    Task<int> ExecuteAsync(string query, object? parameters = null);
    Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null);
    Task<T?> QuerySingleOrDefaultAsync<T>(string query, object? parameters = null);
    Task CreateConnectionAndInitialize(CancellationToken cancellationToken = default);
    Task InitializeDatabaseAsync();
    Task CloseConnection();

    Task<IEnumerable<string>> GetAllDevicesAsync();
}
