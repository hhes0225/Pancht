using ApiGameServer.Models.DAO;
using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using ApiGameServer.Service.Interfaces;
using System.Text.Json;

namespace ApiGameServer.Service;

public class LoginService:ILoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly string _apiServerAddress;
    private readonly IPanchtDb _userDataDb;
    private readonly IMemoryDb _memoryDb;
    readonly IAccountServerAuthHandler _accountServerHandler;

    public LoginService(ILogger<LoginService> logger, IConfiguration configuration, IPanchtDb userDataDb, IMemoryDb memoryDb, IAccountServerAuthHandler accountServerHandler)
    {
        _logger = logger;
        _apiServerAddress = configuration["AccountServerUrl"];
        _userDataDb = userDataDb;
        _memoryDb = memoryDb;
        _accountServerHandler = accountServerHandler;
    }


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var loginResponse = new LoginResponse();

        //계정 서버에 인증 요청
        var verifyResponse = await _accountServerHandler.RequestVerifyToken(request.Id, request.AuthToken);

        if (verifyResponse != ErrorCode.None)
        {
            loginResponse.Result = verifyResponse;
            return loginResponse;
        }

        //유저 데이터 로드 영역
        var userData = await _userDataDb.GetUserDataAsync(request.Id);

        if (userData.Item2 == null)
        {
            loginResponse.Result = ErrorCode.GameDataNotExist;
            return loginResponse;
        }

        _logger.LogInformation($"Successfully authenticated user {userData.Item2.id}");


        //redis - id, 토큰 저장
        var authSetResult = await _memoryDb.SetAccessTokenAsync(request.Id, request.AuthToken);

        if (authSetResult != ErrorCode.None)
        {
            loginResponse.Result = authSetResult;
            return loginResponse;
        }

        loginResponse.Result = ErrorCode.None;
        loginResponse.UserGameData = userData.Item2;

        return loginResponse;
    }
    

    
}
