using ApiMatchingServer.Model.DAO;
using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserStateLibrary;

namespace ApiMatchingServer.Repository;

public class MemoryDb:IMemoryDb
{
    readonly RedisConnection _redisConnection;
    readonly ILogger<MemoryDb> _logger;

    //TODO: 2개의 Pub/Sub을 사용하므로 Redis 객체가 2개 있어야 한다.
    // 매칭서버에서 -> 게임서버, 게임서버 -> 매칭서버로
    string _redisAddress = "";
    string _requestMatchingKey;//Redis 객체 키
    string _checkMatchingKey;//Redis 객체 키

    public MemoryDb(ILogger<MemoryDb> logger, IOptions<MatchingConfig> matchingConfig)
    {
        _logger = logger;

        RedisConfig redisConfig= new RedisConfig("default", matchingConfig.Value.RedisAddress);
        _redisConnection = new RedisConnection(redisConfig);
    }

    public void Dispose()
    {
    }

    //redis에 저장된 유저 상태 정보 가져오기
    public async Task<(ErrorCode, UserState)> GetUserState(string id)
    {
        try
        {
            string key = id + "_state";
            RedisString<RedisUserState> query = new RedisString<RedisUserState>(_redisConnection, key, TimeSpan.MaxValue);
            RedisResult<RedisUserState> queryResult = await query.GetAsync();

            if (!queryResult.HasValue)
            {
                return (ErrorCode.MatchingServerUserStateNotExist, UserState.None);
            }

            return (ErrorCode.None, queryResult.Value.state);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetUserState Error");
        }

        return (ErrorCode.None, UserState.None);
    }

    //redis에 유저 상태 정보 저장하기
    //기존 유효 시간이 남아있으면 그 시간을 유지한다
    public async Task<ErrorCode> SetUserState(string id, UserState state)
    {
        try
        {
            string key = id + "_state";
            var updatedState = new RedisUserState { state = state };

            string script = @"
            local ttl = redis.call('TTL', KEYS[1])
            redis.call('SET', KEYS[1], ARGV[1])
            if ttl > 0 then
                redis.call('EXPIRE', KEYS[1], ttl)
            end
            return ttl
            ";

            var query = await _redisConnection.GetConnection().GetDatabase().ScriptEvaluateAsync(
                script,
                new StackExchange.Redis.RedisKey[] { key },//LUA 스크립트에서 사용할 KEYS 배열
                new StackExchange.Redis.RedisValue[] { JsonConvert.SerializeObject(updatedState)});//LUA 스크립트에서 사용할 ARGV 배열

            long ttl = (long)query;

            if (ttl<0)//유효 시간이 없으면
            {
                return ErrorCode.MatchingServerUserStateSetFail;
            }

        }
        catch (Exception e)
        {
            _logger.LogError(e, "SetUserState Error");
            return ErrorCode.MatchingServerUserStateSetFail;
        }

        return ErrorCode.None;
    }

}
