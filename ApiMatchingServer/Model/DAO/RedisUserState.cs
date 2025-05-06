namespace ApiMatchingServer.Model.DAO;
using UserStateLibrary;

public class RedisUserState
{
    public UserState state { get; set; } = UserState.None;
}
