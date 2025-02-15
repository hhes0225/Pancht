﻿using System;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    AuthCheckFail = 21,
    ReceiptCheckFail = 22,

    //Matching Server 측 Resid Error 처리: 3300~
    MatchingServerRedisException = 3300,
    MatchingServerUserStateNotExist = 3301,
    MatchingServerUserStateSetFail = 3302,

    //Matching 관련 Error 6000~
    MatchingFailError = 6002,
    MatchingNotYet = 6003,
}