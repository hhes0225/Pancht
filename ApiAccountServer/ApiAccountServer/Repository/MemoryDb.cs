using ApiAccountServer.Models.DAO;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices.ObjectiveC;
namespace ApiAccountServer.Repository;

public class MemoryDb: IMemoryDb
{
    public RedisConnection _redisConn;
    public ILogger<MemoryDb> _logger;

    public MemoryDb(ILogger<MemoryDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;

        RedisConfig redisConfig = new RedisConfig("default", dbConfig.Value.RedisAccountDb);
        _redisConn = new RedisConnection(redisConfig);
    }

    public void Dispose()
    {
    }

    //id, 인증토큰 Redis에 등록
    public async Task<ErrorCode> SetAccessToken(string id, string authToken)
    {
        ErrorCode result = ErrorCode.None;

        RedisDbData authData = new()
        {
            Id = id,
            AuthToken = authToken
        };

        string key = authData.Id;

        try
        {
            RedisString<RedisDbData> redis = new(_redisConn, key, GetExpireTime());

            if(await redis.SetAsync(authData, GetExpireTime()) == false)
            {
                return ErrorCode.AuthTokenRegisterFail;
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            result = ErrorCode.RedisFailException;
        }

        return ErrorCode.None;
    }

    //id로 인증토큰 조회
    public async Task<ErrorCode> GetAccessToken(string id, string authToken)
    {
        try
        {
            RedisString<RedisDbData> redis = new RedisString<RedisDbData>(_redisConn, id, null);
            RedisResult<RedisDbData> userAuthData = await redis.GetAsync();

            if (!userAuthData.HasValue)
            {
                _logger.LogError("userAuthData is null");
                return ErrorCode.AuthTokenInfoNotExist;
            }

            if(userAuthData.Value.Id != id|| userAuthData.Value.AuthToken != authToken)
            {
                _logger.LogError("AuthTokenIdNotMatch");
                return ErrorCode.AuthTokenIdNotMatch;
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return ErrorCode.RedisFailException;
        }

        return ErrorCode.None;
    }

    public TimeSpan GetExpireTime()
    {
        return TimeSpan.FromHours(RedisKeyExpireTime.LoginKeyExpireHour);
    }
}
