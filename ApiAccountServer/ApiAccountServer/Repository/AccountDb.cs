using ApiAccountServer.Models.DAO;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace ApiAccountServer.Repository;

public class AccountDb : IAccountDb
{
    private ILogger<AccountDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConnection;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public AccountDb(ILogger<AccountDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;
        _dbConfig = dbConfig;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new QueryFactory(_dbConnection, _compiler);
    }

    public async Task<ErrorCode> InsertAccountAsync(string id, string pw)
    {
        try
        {
            int result = await _queryFactory.Query("Account").InsertAsync(new AccountDbData
            {
                //uid는 auto increment로 자동 생성
                id= id,
                pw = pw,
                create_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                recent_login_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return ErrorCode.AccountDbFailException;
        }

        return ErrorCode.None;
    }

    public async Task<string> FindUserById(string id)
    {
        try
        {
            var result = await _queryFactory.Query("Account").Where("id", id).FirstOrDefaultAsync<string>();

            return result;

        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    //로그인 성공 시 인증토큰 반환
    public async Task<ErrorCode> VerifyUserLogin(string id, string pw)
    {
        try
        {
            var result = await _queryFactory.Query("Account").Where("id", id).FirstOrDefaultAsync<AccountDbData>();

            if(result == null)
            {
                _logger.LogError("Login Fail - verification");
                return ErrorCode.LoginFailVerification;
            }

            var verifyPw = Security.Security.VerifyPassword(pw, result.pw);
            //var verifyPw = Security.Security.VerifyPassword(result.pw, pw);

            if (!verifyPw)
            {
                _logger.LogError("Login Fail - password");
                return ErrorCode.LoginFailVerification;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return ErrorCode.AccountDbFailException;
        }

        return ErrorCode.None;
    }

    public void Dispose()
    {
        Close();
    }

    private void Open()
    {
        _dbConnection = new MySqlConnection(_dbConfig.Value.MySqlAccountDb);
        _dbConnection.Open();
    }

    private void Close()
    {
        _dbConnection.Close();
    }

}
