using ApiAccountServer.Models.DTO;

namespace ApiAccountServer.Service;

public interface IRegisterService
{
    public Task<ErrorCode> RegisterAsync(RegisterRequest request);
    public Task<ErrorCode> IsAccountExist(string id);
    public bool ConfirmPassword(string pw, string confirmPw);
    public string EncryptPassword(string pw);   
}
