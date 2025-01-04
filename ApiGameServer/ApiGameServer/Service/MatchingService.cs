using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;

namespace ApiGameServer.Service;

public class MatchingService:IMatchingService
{
    readonly ILogger<MatchingService> _logger;
    readonly string _matchingServerAddress;

    public MatchingService(ILogger<MatchingService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _matchingServerAddress = configuration["MatchingServerUrl"];
    }

    //매칭 요청 매칭 서버에 전달
    public async Task<MatchingResponse> RequestMatchingAsync(MatchingRequest request)
    {
        var response = new MatchingResponse();
        try
        {
            HttpClient client = new HttpClient();
            var responseFromMatchingServer = await client.PostAsJsonAsync($"{_matchingServerAddress}/Matching", request);

            if(!responseFromMatchingServer.IsSuccessStatusCode)
            {
                _logger.LogError($"Matching Server Connection Fail: {responseFromMatchingServer.StatusCode}");
                response.Result = ErrorCode.GameMatchingFailException;
                return response;
            }

            response = await responseFromMatchingServer.Content.ReadFromJsonAsync<MatchingResponse>();

            if(response.Result != ErrorCode.None)
            {
                _logger.LogError($"Matching Server Response Error: {response.Result}");
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "매칭 요청 실패");
            response.Result = ErrorCode.GameMatchingFailException;
            return response;
        }

        return response;
    }

    //매칭 상태 확인 요청 매칭 서버에 전달
    public async Task<CheckMatchingResponse> CheckMatchingAsync(CheckMatchingRequest request)
    {
        var response = new CheckMatchingResponse();
        try
        {
            HttpClient client = new HttpClient();
            var responseFromMatchingServer = await client.PostAsJsonAsync($"{_matchingServerAddress}/Matching", request);

            if (!responseFromMatchingServer.IsSuccessStatusCode)
            {
                _logger.LogError($"Matching Server Connection Fail: {responseFromMatchingServer.StatusCode}");
                response.Result = ErrorCode.GameMatchingFailException;
                return response;
            }

            response = await responseFromMatchingServer.Content.ReadFromJsonAsync<CheckMatchingResponse>();

            if (response.Result != ErrorCode.None)
            {
                _logger.LogError($"Matching Server Response Error: {response.Result}");
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "매칭 요청 실패");
            response.Result = ErrorCode.GameMatchingFailException;
            return response;
        }

        return response;
    }

    //매칭 취소 요청 매칭 서버에 전달
    public async Task<CancelMatchingResponse> CancelMatchingAsync(CancelMatchingRequest request)
    {
        var response = new CancelMatchingResponse();
        try
        {
            HttpClient client = new HttpClient();
            var responseFromMatchingServer = await client.PostAsJsonAsync($"{_matchingServerAddress}/CancelMatching", request);

            if (!responseFromMatchingServer.IsSuccessStatusCode)
            {
                _logger.LogError($"Matching Server Connection Fail: {responseFromMatchingServer.StatusCode}");
                response.Result = ErrorCode.GameMatchingFailException;
                return response;
            }

            response = await responseFromMatchingServer.Content.ReadFromJsonAsync<CancelMatchingResponse>();

            if (response.Result != ErrorCode.None)
            {
                _logger.LogError($"Matching Server - Cancel Response Error: {response.Result}");
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "매칭 취소 요청 실패");
            response.Result = ErrorCode.GameMatchingFailException;
            return response;
        }

        return response;
    }
}
