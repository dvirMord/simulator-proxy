using System.Net.Http.Json;
using proxy_simulator.Constants;
using proxy_simulator.Interfaces;
using static proxy_simulator.DTOs.MultimediaApiDTO;

namespace proxy_simulator.Services
{
    public class MultimediaServiceAPI : IMultimediaServiceAPI
    {
        private static class ServiceApi
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
            _httpClient = httpClient;
            _logger = logger;
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
                    _logger.LogWarning(ServicesLogs.Multimedia.UPLOAD_FILE_FAILED, fileName, response.StatusCode);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Multimedia.UPLOAD_FILE_SUCCESS, fileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Multimedia.UPLOAD_FILE_ERROR, fileName);
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
                    _logger.LogWarning(ServicesLogs.Multimedia.DELETE_FILE_FAILED, dto.FileName, response.StatusCode);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Multimedia.DELETE_FILE_SUCCESS, dto.FileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Multimedia.DELETE_FILE_ERROR, dto.FileName);
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
                    string serverError = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Multimedia.START_STREAM_FAILED, dto.FileName, response.StatusCode, serverError);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Multimedia.START_STREAM_SUCCESS, dto.FileName, dto.SourceFileId);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Multimedia.START_STREAM_ERROR, dto.FileName);
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
                    string serverError = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Multimedia.STOP_STREAM_FAILED, dto.StreamName, response.StatusCode, serverError);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Multimedia.STOP_STREAM_SUCCESS, dto.StreamName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Multimedia.STOP_STREAM_ERROR, dto.StreamName);
                return false;
            }
        }
    }
}