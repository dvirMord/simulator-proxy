using proxy_simulator.DTOs;

namespace proxy_simulator.Interfaces
{
    public interface IMultimediaServiceAPI
    {
        Task<int> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
        Task<bool> DeleteFileAsync(MultimediaApiDTO.DeleteFileDTO dto, CancellationToken cancellationToken = default);
        Task<string> StartStreamAsync(MultimediaApiDTO.StartStreamDTO dto, CancellationToken cancellationToken = default);
        Task<bool> StopStreamAsync(MultimediaApiDTO.StopStreamDTO dto, CancellationToken cancellationToken = default);
        Task<IEnumerable<MultimediaApiDTO.ChannelDTO>> GetActiveStreamsAsync(CancellationToken ct = default);
    }
}
