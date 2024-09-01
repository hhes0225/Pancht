using ApiAccountServer.Models.DAO;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;

namespace ApiAccountServer.Repository;

public class AccountRepository : IAccountRepository
{
    private ILogger<AccountRepository> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConnection;
    private readonly SqlKata.Compilers.MySqlCompiler _compiler;
    private readonly QueryFactory _queryFactory;
    
    public AccountRepository(ILogger<AccountRepository> logger, IOptions<DbConfig> dbConfig)
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
            int result = await _queryFactory.Query("Account").InsertAsync(new AccountDb
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
    public async Task<ErrorCode> VerifyUser(string id, string pw)
    {
        return ErrorCode.None;
    }

    public void Dispose()
    {
        Close();
    }

    private void Open()
    {
        _dbConnection = new MySqlConnection(_dbConfig.Value.AccountDb);
        _dbConnection.Open();
    }

    private void Close()
    {
        _dbConnection.Close();
    }

}
