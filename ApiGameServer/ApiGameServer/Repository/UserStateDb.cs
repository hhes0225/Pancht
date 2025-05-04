namespace ApiGameServer.Repository;

using CloudStructures;
using Microsoft.Extensions.Options;
using UserStateLibrary;

public class UserStateDb: IUserStateDb
{
    readonly ILogger<UserStateDb> _logger;
    RedisConnection _redisConn;
    private readonly UserStateManager _manager;

    public UserStateDb(ILogger<UserStateDb> logger, IOptions<DbConfig> dbConfig)
    {
        _logger = logger;
        RedisConfig redisConfig = new RedisConfig("default", dbConfig.Value.RedisPanchtDb);
        _redisConn = new RedisConnection(redisConfig);
        _manager = new UserStateManager(_redisConn);
    }

    public void Dispose()
    {
    }

    // 유저 상태를 Redis에 등록하는 메서드
    public async Task<ErrorCode> CreateUserStateAsync(string id)
    {
        var result = ErrorCode.None;
        try
        {
            if(await _manager.SetStateAsync(id, UserState.None) == false)
            {
                return ErrorCode.UserStateCreateFail;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CreateUserState Error");
            return ErrorCode.UserStateFailException;
        }

        return result;
    }

    // 유저 상태를 매칭 대기 상태로 변경하는 메서드
    public async Task<ErrorCode> SetUserStateToMatchingAsync(string id)
    {
        var result = ErrorCode.None;

        try
        {
            if(await _manager.ChangeStateIfMatchAsync(id, UserState.None, UserState.Matching) == false)
            {
                return ErrorCode.UserStateChangeFail;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SetUserStateToMatching Error");
            return ErrorCode.UserStateFailException;
        }

        return result;
    }
}
