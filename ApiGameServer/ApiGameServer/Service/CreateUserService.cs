using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using ApiGameServer.Service.Interfaces;
namespace ApiGameServer.Service;

public class CreateUserService:ICreateUserService
{
    readonly ILogger<CreateUserService> _logger;
    readonly IPanchtDb _panchtDb;
    readonly IMemoryDb _memoryDb;
    readonly IAccountServerAuthHandler _accountServerAuthHandler;

    public CreateUserService(ILogger<CreateUserService> logger, IPanchtDb userDataDb, IMemoryDb memoryDb, IAccountServerAuthHandler accountServerAuthHandler)
    {
        _logger = logger;
        _panchtDb = userDataDb;
        _memoryDb = memoryDb;
        _accountServerAuthHandler = accountServerAuthHandler;
    }

    //유저 데이터 생성 및 DB에 추가
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        var createUserResponse = new CreateUserResponse();

        //계정 서버에 인증 요청
        var verifyResponse = await _accountServerAuthHandler.RequestVerifyToken(request.Id, request.AuthToken);

        if (verifyResponse != ErrorCode.None)
        {
            createUserResponse.Result = verifyResponse;
            return createUserResponse;
        }

        //닉네임 중복 체크한다 
        //db에 해당 닉네임 조회
        if (await _panchtDb.CheckNicknameExistAsync(request.Nickname))
        {
            _logger.LogError("Nickname already exists");
            createUserResponse.Result = ErrorCode.GameCreateFailNicknameExist;
            return createUserResponse;
        }

        //유저 데이터 DB에 추가
        var result = await _panchtDb.CreateUserDataAsync(request.Id, request.Nickname);

        if (result.Item1 != ErrorCode.None)
        {
            _logger.LogError($"Create Userdata Error: {result.Item1}");
            createUserResponse.Result = result.Item1;
            return createUserResponse;
        }

        //redis - id, 토큰 저장
        var authSetResult = await _memoryDb.SetAccessTokenAsync(request.Id, request.AuthToken);

        if (authSetResult != ErrorCode.None)
        {
            _logger.LogError($"SetAccessTokenAsync Error: {authSetResult}");
            createUserResponse.Result = authSetResult;
            return createUserResponse;
        }

        createUserResponse.Result = ErrorCode.None;
        createUserResponse.UserGameData = result.Item2;

        return createUserResponse;
    }


    
}
