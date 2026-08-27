using System.Net.Http.Json;
using System.Text.Json;
using proxy_simulator.Constants;
using proxy_simulator.DTOs;
using proxy_simulator.Interfaces;
using proxy_simulator.ROs;

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

        public MultimediaServiceAPI(HttpClient httpClient, ILogger<MultimediaServiceAPI> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<int> UploadFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", fileName);

            var response = await _httpClient.PostAsync(ServiceApi.FILES_API, content, ct);
            var serverResponse = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(ServicesLogs.Multimedia.UPLOAD_FILE_FAILED, fileName, response.StatusCode, serverResponse);
                throw new HttpRequestException(string.Format(ServicesLogs.Multimedia.EXC_UPLOAD_FILE_FAILED, fileName, response.StatusCode, serverResponse), null, response.StatusCode);
            }

            var serverRo = JsonSerializer.Deserialize<SimulatorsRos.Multimedia.UploadFileResponse>(serverResponse);
            if (serverRo is null)
                throw new InvalidOperationException(string.Format(ServicesLogs.Multimedia.EXC_UPLOAD_FILE_INVALID_RESPONSE, fileName));

            if (!serverRo.Success)
                throw new InvalidOperationException(string.Format(ServicesLogs.Multimedia.EXC_UPLOAD_FILE_SERVER_REJECTED, fileName, serverRo.Message));

            _logger.LogInformation(ServicesLogs.Multimedia.UPLOAD_FILE_SUCCESS, fileName);
            return serverRo.IdInDb;
        }

        public async Task<bool> DeleteFileAsync(MultimediaApiDTO.DeleteFileDTO dto, CancellationToken ct = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, ServiceApi.FILES_API) { Content = JsonContent.Create(dto) };
            var response = await _httpClient.SendAsync(request, ct);
            var serverResponse = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(ServicesLogs.Multimedia.DELETE_FILE_FAILED, dto.FileName, response.StatusCode);
                throw new HttpRequestException(string.Format(ServicesLogs.Multimedia.EXC_DELETE_FILE_FAILED, dto.FileName, response.StatusCode, serverResponse), null, response.StatusCode);
            }

            _logger.LogInformation(ServicesLogs.Multimedia.DELETE_FILE_SUCCESS, dto.FileName);
            return true;
        }

        public async Task<bool> StartStreamAsync(MultimediaApiDTO.StartStreamDTO dto, CancellationToken ct = default)
        {
            var response = await _httpClient.PostAsJsonAsync(ServiceApi.START_STREAM_API, dto, ct);
            var serverResponse = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(ServicesLogs.Multimedia.START_STREAM_FAILED, dto.FileName, response.StatusCode, serverResponse);
                throw new HttpRequestException(string.Format(ServicesLogs.Multimedia.EXC_START_STREAM_FAILED, dto.FileName, response.StatusCode, serverResponse), null, response.StatusCode);
            }

            _logger.LogInformation(ServicesLogs.Multimedia.START_STREAM_SUCCESS, dto.FileName, dto.SourceFileId);
            return true;
        }

        public async Task<bool> StopStreamAsync(MultimediaApiDTO.StopStreamDTO dto, CancellationToken ct = default)
        {
            var response = await _httpClient.PostAsJsonAsync(ServiceApi.STOP_STREAM_API, dto, ct);
            var serverResponse = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(ServicesLogs.Multimedia.STOP_STREAM_FAILED, dto.StreamName, response.StatusCode, serverResponse);
                throw new HttpRequestException(string.Format(ServicesLogs.Multimedia.EXC_STOP_STREAM_FAILED, dto.StreamName, response.StatusCode, serverResponse), null, response.StatusCode);
            }

            _logger.LogInformation(ServicesLogs.Multimedia.STOP_STREAM_SUCCESS, dto.StreamName);
            return true;
        }
    }
}