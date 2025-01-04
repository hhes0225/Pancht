using ApiGameServer.Models.DTO;
using ApiGameServer.Service.Interfaces;
using System.Text.Json;

namespace ApiGameServer.Service;

public class AccountServerAuthHandler:IAccountServerAuthHandler
{
    readonly ILogger<AccountServerAuthHandler> _logger;
    readonly string _accountServerAddress;

    public AccountServerAuthHandler(ILogger<AccountServerAuthHandler> logger, IConfiguration configuration)
    {
        _logger = logger;
        _accountServerAddress = configuration["AccountServerUrl"];
    }

    public async Task<ErrorCode> RequestVerifyToken(string id, string authToken)
    {
        try
        {
            HttpClient client = new HttpClient();
            var accountServerResponse = await client.PostAsJsonAsync($"{_accountServerAddress}/VerifyToken", new
            {
                Id = id,
                AuthToken = authToken
            });

            if (!accountServerResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"Account Server Connection Fail: {accountServerResponse.StatusCode}");
                return ErrorCode.LoginFailAccountConnectionException;
            }

            var responseContent = await accountServerResponse.Content.ReadAsStringAsync();
            responseContent = responseContent.Replace("result", "Result");
            //json attribute명 소문자로 된 것을 클래스 프로퍼티 형식에 맞게 대문자로 변환해야 역직렬화 정상동작

            var response = JsonSerializer.Deserialize<AccountVerificationResponse>(responseContent);

            _logger.LogInformation($"Account Server Response: {responseContent}");

            if (response.Result != ErrorCode.None)
            {
                _logger.LogError($"Account Server Response Error: {response.Result}");
                return response.Result;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.LogError(e, "RequestVerifyToken Error");
            return ErrorCode.LoginFailAccountConnectionException;
        }
    }
}
