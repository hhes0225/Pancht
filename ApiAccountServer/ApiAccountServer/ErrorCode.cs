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

    //Account Server 측 Login Error 처리: 2100~
    LoginFailException = 2101,
    LoginFailVerification = 2102,

    //인증 토큰 Error 처리: 2200~
    RedisFailException = 2201,
    AuthTokenFailException = 2202,
    AuthTokenRegisterFail = 2203,
    AuthTokenInfoNotExist = 2204,
    AuthTokenIdNotMatch = 2205,
    AuthTokenTokenNotMatch = 2206,



    //Account DB Error 처리: 3000~
    AccountDbFailException = 3001,
    AccountDbConnectionFail = 3002,

}
