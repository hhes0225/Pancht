using ApiGameServer.Models.DAO;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;

namespace ApiGameServer.Repository;

public class MemoryDb:IMemoryDb
{
    readonly ILogger<MemoryDb> _logger;
    RedisConnection _redisConn;

    public MemoryDb(ILogger<MemoryDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;

        RedisConfig redisConfig = new RedisConfig("default", dbConfig.Value.RedisPanchtDb);
        _redisConn = new RedisConnection(redisConfig);
    }

    public void Dispose()
    {
    }

    //id, 인증토큰 Redis에 등록
    public async Task<ErrorCode> SetAccessTokenAsync(string id, string authToken)
    {
        var result = ErrorCode.None;

        try
        {
            RedisDbData authData = new()
            {
                Id = id,
                AuthToken = authToken
            };

            string key = authData.Id;

            RedisString<RedisDbData> redis = new(_redisConn, key, GetExpireTime());

            if(await redis.SetAsync(authData, GetExpireTime()) == false)
            {
                return ErrorCode.GameServerAuthTokenRegisterFail;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SetAccessTokenAsync Error");
            return ErrorCode.GameServerRedisException;
        }

        return result;
    }

    //id, 인증토큰으로 Redis에 저장된 값과 verify
    public async Task<ErrorCode> VerifyAccessTokenAsync(string id, string authToken)
    {
        try
        {
            RedisString<RedisDbData> redis = new RedisString<RedisDbData>(_redisConn, id, null);
            RedisResult<RedisDbData> userAuthData = await redis.GetAsync();

            if (userAuthData.HasValue)
            {
                return ErrorCode.GameServerAuthTokenInfoNotExist;
            }

            if (userAuthData.Value.Id != id)
            {
                return ErrorCode.GameServeAuthTokenIdNotMatch;
            }
            if (userAuthData.Value.AuthToken != authToken)
            {
                return ErrorCode.GameServeAuthTokenNotMatch;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "VerifyAccessTokenAsync Error");
            return ErrorCode.GameServerRedisException;
        }

        return ErrorCode.None;
    }

    public TimeSpan GetExpireTime()
    {
        return TimeSpan.FromHours(RedisKeyExpireTime.LoginKeyExpireHour);
    }
}
