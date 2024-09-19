namespace ApiGameServer.Models.DAO;

public class RedisDbData
{   
    public string Id { get; set; } =string.Empty;
    public string AuthToken { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Id: {Id}, AuthToken: {AuthToken}";
    }
}

public class RedisKeyExpireTime
{
    public const ushort LoginKeyExpireHour = 6;
}