using ApiAccountServer.Models.DTO;
using ApiAccountServer.Repository;
using CloudStructures;

namespace ApiAccountServer.Service;

public class LoginService:ILoginService
{
    ILogger<LoginService> _logger;
    IAccountDb _accountRepository;
    IMemoryDb _memoryRepository;


    public LoginService(ILogger<LoginService> logger, IAccountDb accountRepository, IMemoryDb memoryRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _memoryRepository = memoryRepository;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        //RDBMS에서 조회
        var result = await _accountRepository.VerifyUserLogin(request.Id, request.Password);
        
        if (result != ErrorCode.None)
        {
            return new LoginResponse { Result=result};    
        }

        _logger.Log(LogLevel.Information, $"{request.Id} DB Verify success");

        //인증코드 생성
        var token =  Security.Security.GenerateAuthToken(request.Id);
        _logger.LogInformation($"token: {token}");

        //Redis에 저장
        result = await _memoryRepository.SetAccessToken(request.Id, token);
        if (result != ErrorCode.None)
        {
            return new LoginResponse { Result = result };
        }



        return new LoginResponse { Result = result, Id = request.Id, AuthToken=token};
    }

}
