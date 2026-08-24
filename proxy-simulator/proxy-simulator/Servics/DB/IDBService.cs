namespace multimedia_simulator.Interfaces
{
    public interface IDBService
    {
        public Task CreateConnectionAndInitialize(CancellationToken cancellationToken = default);
        Task InitializeDatabaseAsync();
        public Task CloseConnection();
        //Used for write operations where you do not expect table rows back,
        //only a confirmation of how many rows changed.
        public Task<int> ExecuteAsync(string query, object? parameters = null);

        //Used for read operations that can return zero, one, or many rows
        public Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null);

        //Used for lookups where at most one record is expected
        public Task<T?> QuerySingleOrDefaultAsync<T>(string query, object? parameters = null);
    }
}
