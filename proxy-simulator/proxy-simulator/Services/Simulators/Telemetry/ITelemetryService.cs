using static proxy_simulator.DTOs.TelemetryApiDTO;

namespace proxy_simulator.Interfaces
{
    public interface ITelemetryServiceAPI
    {
        Task<bool> UploadKlvFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
        Task<bool> DeleteKlvFileAsync(DeleteFileDTO dto, CancellationToken cancellationToken = default);
        Task<StreamResponseDTO> StartStreamAsync(StartStreamDTO dto, CancellationToken cancellationToken = default);
        Task<StreamResponseDTO> StopStreamAsync(StopStreamDTO dto, CancellationToken cancellationToken = default);
    }
}