using CloudStructures;
using CloudStructures.Structures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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



}
