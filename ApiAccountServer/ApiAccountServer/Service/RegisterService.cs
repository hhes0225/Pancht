using ApiAccountServer.Repository;
using ApiAccountServer.Models.DTO;
using Microsoft.AspNetCore.Identity;
using ApiAccountServer.Security;

namespace ApiAccountServer.Service;

public class RegisterService:IRegisterService
{
    
    private ILogger<RegisterService> _logger;
    IAccountDb _accountRepo;

    public RegisterService(ILogger<RegisterService> logger, IAccountDb accountRepo)
    {
        _logger = logger;
        _accountRepo = accountRepo;
    }

    public async Task<ErrorCode> RegisterAsync(RegisterRequest request)
    {
        try
        {
            //이메일 중복 체크
            var accountExist = await IsAccountExist(request.Id);
            if (accountExist != ErrorCode.None)
            {
                return ErrorCode.RegisterFailEmailExist;
            }

            //비밀번호 확인 동일 체크
            var confirmPw = ConfirmPassword(request.Password, request.ConfirmPassword);
            if (confirmPw == false)
            {
                return ErrorCode.RegisterFailPasswordNotMatch;
            }

            //비밀번호 해싱
            var encryptedPw = Security.Security.EncryptPassword(request.Password);
            _logger.LogInformation($"encryptedPw: {encryptedPw}");

            _logger.LogInformation($"{request.Id} Register Success");

            //계정 등록
            var result = await _accountRepo.InsertAccountAsync(request.Id, encryptedPw);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            return ErrorCode.RegisterFailException;
        }

        return ErrorCode.None;
    }

    public async Task<ErrorCode> IsAccountExist(string id)
    {
        try
        {
            var result = await _accountRepo.FindUserById(id);

            if (!string.IsNullOrEmpty(result))
            {
                _logger.LogError("Account already exist");
                return ErrorCode.RegisterFailEmailExist;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return ErrorCode.AccountDbFailException;
        }

        return ErrorCode.None;
    }

    public bool ConfirmPassword(string pw, string confirmPw)
    {
        if(pw != confirmPw)
        {
            return false;
        }

        return true;
    } 
}
