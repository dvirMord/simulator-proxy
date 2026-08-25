using proxy_simulator.Interfaces;
using static proxy_simulator.DTOs.MultimediaApiDTO;

namespace proxy_simulator.Services
{
    public class MultimediaServiceAPI: IMultimediaServiceAPI
    {
        private class ServiceApi
        {
            public const string FILES_API = "/api/v1/ms/files";
            public const string START_STREAM_API = "/api/v1/ms/stream/start";
            public const string STOP_STREAM_API = "/api/v1/ms/stream/stop";
        }

        private readonly HttpClient _httpClient;
        private readonly ILogger<MultimediaServiceAPI> _logger;


        //================Constructor==========================
        public MultimediaServiceAPI(HttpClient httpClient, ILogger<MultimediaServiceAPI> logger)
        {
            this._httpClient = httpClient;
            this._logger = logger;
        }
        //===================END================================

        public async Task<bool> UploadFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "file", fileName);

                var response = await _httpClient.PostAsync(ServiceApi.FILES_API, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[MultimediaService] Failed to upload file '{FileName}'. StatusCode: {StatusCode}", fileName, response.StatusCode);
                    return false;
                }

                _logger.LogInformation("[MultimediaService] File '{FileName}' uploaded successfully.", fileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, "[MultimediaService] Error uploading file '{FileName}'.", fileName);
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(DeleteFileDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, ServiceApi.FILES_API)
                {
                    Content = JsonContent.Create(dto)
                };

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("[MultimediaService] Failed to delete file '{FileName}'. StatusCode: {StatusCode}", dto.FileName, response.StatusCode);
                    return false;
                }

                _logger.LogInformation("[MultimediaService] File '{FileName}' deleted successfully.", dto.FileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, "[MultimediaService] Error deleting file '{FileName}'.", dto.FileName);
                return false;
            }
        }

        public async Task<bool> StartStreamAsync(StartStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.START_STREAM_API, dto, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string serverError = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"[MultimediaService] Failed to start stream for file '{dto.FileName}'. StatusCode: {response.StatusCode} error from server: {serverError}");
                    return false;
                }

                _logger.LogInformation("[MultimediaService] Stream started successfully for file '{FileName}' (SourceFileId: {SourceFileId}).", dto.FileName, dto.SourceFileId);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, "[MultimediaService] Error starting stream for file '{FileName}'.", dto.FileName);
                return false;
            }
        }

        public async Task<bool> StopStreamAsync(StopStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.STOP_STREAM_API, dto, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string serverError = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"[MultimediaService] Failed to stop stream '{dto.StreamName}'. StatusCode: {response.StatusCode} ServerError: {serverError}");
                    return false;
                }

                _logger.LogInformation("[MultimediaService] Stream '{StreamName}' stopped successfully.", dto.StreamName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, "[MultimediaService] Error stopping stream '{StreamName}'.", dto.StreamName);
                return false;
            }
        }
    }   
}