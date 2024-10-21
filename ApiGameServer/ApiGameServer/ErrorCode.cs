namespace ApiGameServer;

public enum ErrorCode:UInt16
{
    None=0,

    //Common Error 처리: 1000~
    UnhandledException = 1001,
    InvalidRequest =1003,

    //Register Error 처리: 2000~
    RegisterFailException = 2001,
    RegisterFailEmailExist = 2002,
    RegisterFailPasswordNotMatch = 2003,

    //Account Server 측 Login Error 처리: 3000~
    LoginFailException = 3001,
    LoginFailVerification = 3002,

    //인증 토큰 Error 처리: 4000~
    RedisFailException = 4001,
    AuthTokenFailException = 4002,
    AuthTokenRegisterFail = 4003,
    AuthTokenInfoNotExist = 4004,
    AuthTokenIdNotMatch = 4005,
    AuthTokenTokenNotMatch = 4006,

    
    //Account DB Error 처리: 5000~
    AccountDbFailException = 5001,
    AccountDbConnectionFail = 5002,


    //Game Server 측 Login Error 처리: 6000~
    LoginFailAccountConnectionException = 6001,

    //Game SQL Error 처리: 7000~
    GameDataCreateFailException = 7001,
    GameDataLoadException = 7002,
    GameCreateFailNicknameExist = 7003,
    GameDataNotExist = 7004,
    GameCharacterDataNotExist = 7005,
    GameCharacterDataLoadFail = 7005,

    //Game Server Redis Error 처리: 8000~
    GameServerRedisException = 8001,
    GameServerAuthTokenRegisterFail = 8002,
    GameServerAuthTokenInfoNotExist = 8003,
    GameServeAuthTokenIdNotMatch = 8004,
    GameServeAuthTokenNotMatch = 8004,

    //Attendance Error 처리: 9000~
    AttendanceDataLoadFail = 9001,
    AttendanceDataCreateFail = 9002,
    AttendanceDataUpdateFail = 9003,
    AttendanceDataNotExist = 9004,
    AttendanceAlreadyDone = 9005,
    AttendanceDataCreateFailException = 9006,
    AttendanceDataUpdateFailException = 9007,
}
