namespace ApiAccountServer;

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

    //Login Error 처리: 3000~
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
    AccountDbConnectionFail = 5002

}
