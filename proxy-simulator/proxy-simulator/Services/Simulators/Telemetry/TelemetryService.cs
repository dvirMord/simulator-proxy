using proxy_simulator.Constants;
using proxy_simulator.Interfaces;
using proxy_simulator.ROs;
using System.Net.Http.Headers;
using System.Text.Json;
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

        public async Task<int> UploadKlvFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(fileStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(ServicesConstants.Telemetry.FILE_FORM);
                content.Add(streamContent, ServicesConstants.Multemedia.HTTP_FILE_HEADER_NAME, fileName);

                var response = await _httpClient.PostAsync(ServiceApi.FILES_API, content, cancellationToken);
                var serverResponse = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(ServicesLogs.Telemetry.UPLOAD_KLV_FAILED, fileName, response.StatusCode, serverResponse);
                    throw new HttpRequestException(string.Format(ServicesLogs.Telemetry.EXC_UPLOAD_KLV_FAILED, fileName, response.StatusCode, serverResponse), null, response.StatusCode);
                }
                var serverRo = JsonSerializer.Deserialize<SimulatorsRos.Telemetry.UploadFileResponse>(serverResponse);
                if (serverRo is null || !serverRo.Success)
                    throw new InvalidOperationException(string.Format(ServicesLogs.Telemetry.EXC_UPLOAD_KLV_FAILED, fileName, response.StatusCode, serverResponse));

                _logger.LogInformation(ServicesLogs.Telemetry.UPLOAD_KLV_SUCCESS, fileName);
                return serverRo.DecodedId;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.UPLOAD_KLV_ERROR, fileName);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.UPLOAD_KLV_ERROR, fileName);
                throw;
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
                var error = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(ServicesLogs.Telemetry.DELETE_KLV_FAILED, dto.SimId, response.StatusCode, error);
                    throw new HttpRequestException(string.Format(ServicesLogs.Telemetry.EXC_DELETE_KLV_FAILED, dto.SimId, response.StatusCode, error), null, response.StatusCode);
                }

                _logger.LogInformation(ServicesLogs.Telemetry.DELETE_KLV_SUCCESS, dto.SimId);
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.DELETE_KLV_ERROR, dto.SimId);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.DELETE_KLV_ERROR, dto.SimId);
                throw;
            }
        }

        public async Task<StreamResponseDTO> StartStreamAsync(StartStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.START_STREAM_API, dto, cancellationToken);
                var error = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(ServicesLogs.Telemetry.START_STREAM_FAILED, dto.SimId, response.StatusCode, error);
                    throw new HttpRequestException(string.Format(ServicesLogs.Telemetry.EXC_START_STREAM_FAILED, dto.SimId, response.StatusCode, error), null, response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<StreamResponseDTO>(cancellationToken: cancellationToken)
                    ?? throw new InvalidOperationException(string.Format(ServicesLogs.Telemetry.EXC_START_STREAM_INVALID_RESPONSE, dto.SimId));

                _logger.LogInformation(ServicesLogs.Telemetry.START_STREAM_SUCCESS, dto.SimId, result.Message);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.START_STREAM_ERROR, dto.SimId);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.START_STREAM_ERROR, dto.SimId);
                throw;
            }
        }

        public async Task<StreamResponseDTO> StopStreamAsync(StopStreamDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ServiceApi.STOP_STREAM_API, dto, cancellationToken);
                var error = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(ServicesLogs.Telemetry.STOP_STREAM_FAILED, dto.SimId, response.StatusCode, error);
                    throw new HttpRequestException(string.Format(ServicesLogs.Telemetry.EXC_STOP_STREAM_FAILED, dto.SimId, response.StatusCode, error), null, response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<StreamResponseDTO>(cancellationToken: cancellationToken)
                    ?? throw new InvalidOperationException(string.Format(ServicesLogs.Telemetry.EXC_STOP_STREAM_INVALID_RESPONSE, dto.SimId));

                _logger.LogInformation(ServicesLogs.Telemetry.STOP_STREAM_SUCCESS, dto.SimId, result.Message);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.STOP_STREAM_ERROR, dto.SimId);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, ServicesLogs.Telemetry.STOP_STREAM_ERROR, dto.SimId);
                throw;
            }
        }
    }
}