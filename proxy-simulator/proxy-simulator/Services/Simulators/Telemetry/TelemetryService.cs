using System.Net.Http.Headers;
using proxy_simulator.Constants;
using proxy_simulator.Interfaces;
using static proxy_simulator.DTOs.TelemetryApiDTO;

namespace proxy_simulator.Services
{
    public class TelemetryServiceAPI : ITelemetryServiceAPI
    {
        private static class ServiceApi
        {
            public const string FILES_API = "/api/v1.0/ts/files";
            public const string START_STREAM_API = "/api/v1.0/ts/start";
            public const string STOP_STREAM_API = "/api/v1.0/ts/stop";
        }

        private readonly HttpClient _httpClient;
        private readonly ILogger<TelemetryServiceAPI> _logger;

        public TelemetryServiceAPI(HttpClient httpClient, ILogger<TelemetryServiceAPI> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> UploadKlvFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(ServicesConstants.Telemetry.FILE_FORM);

                content.Add(streamContent, "file", fileName);

                var response = await _httpClient.PostAsync(ServiceApi.FILES_API, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Telemetry.UPLOAD_KLV_FAILED, fileName, response.StatusCode, error);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Telemetry.UPLOAD_KLV_SUCCESS, fileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.UPLOAD_KLV_ERROR, fileName);
                return false;
            }
        }

        public async Task<bool> DeleteKlvFileAsync(DeleteFileDTO dto, CancellationToken cancellationToken = default)
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
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Telemetry.DELETE_KLV_FAILED, dto.FileName, response.StatusCode, error);
                    return false;
                }

                _logger.LogInformation(ServicesLogs.Telemetry.DELETE_KLV_SUCCESS, dto.FileName);
                return true;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.DELETE_KLV_ERROR, dto.FileName);
                return false;
            }
        }

        public async Task<StreamResponseDTO?> StartStreamAsync(StartStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.START_STREAM_API, dto, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Telemetry.START_STREAM_FAILED, dto.FileName, response.StatusCode, error);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<StreamResponseDTO>(cancellationToken: cancellationToken);
                _logger.LogInformation(ServicesLogs.Telemetry.START_STREAM_SUCCESS, dto.FileName, result?.Message ?? string.Empty);
                return result;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.START_STREAM_ERROR, dto.FileName);
                return null;
            }
        }

        public async Task<StreamResponseDTO?> StopStreamAsync(StopStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.STOP_STREAM_API, dto, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning(ServicesLogs.Telemetry.STOP_STREAM_FAILED, dto.FileName, response.StatusCode, error);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<StreamResponseDTO>(cancellationToken: cancellationToken);
                _logger.LogInformation(ServicesLogs.Telemetry.STOP_STREAM_SUCCESS, dto.FileName, result?.Message ?? string.Empty);
                return result;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.STOP_STREAM_ERROR, dto.FileName);
                return null;
            }
        }
    }
}