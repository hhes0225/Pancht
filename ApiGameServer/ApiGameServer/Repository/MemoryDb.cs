using ApiGameServer.Models.DAO;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Options;
using UserStateLibrary;

namespace ApiGameServer.Repository;

public class MemoryDb : IMemoryDb
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

    //RedisString<RedisDbData>: key-value 쌍으로 Redis에 저장하는 구조체
    //이때, value는 RedisDbData 구조체로 정의되어 있음.
    //라이브러리 내부적으로 RedisDbData 구조체를 json으로 변환하여 Redis에 저장함.
    private RedisString<RedisDbData> BuildRedisKey(string id)
    {
        return new RedisString<RedisDbData>(_redisConn, id, GetExpireTime());
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

            var redisKey = BuildRedisKey(id);

            if (await redisKey.SetAsync(authData, GetExpireTime()) == false)
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
            var redisKey = BuildRedisKey(id);
            RedisResult<RedisDbData> userAuthData = await redisKey.GetAsync();

            if (!userAuthData.HasValue)
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
        //로그인 키 만료 시간
        //6시간으로 설정되어 있음.
        return TimeSpan.FromHours(RedisKeyExpireTime.LoginKeyExpireHour);
    }
}