﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZLogger;
using static ApiMatchingServer.Controllers.CheckMatching;

namespace ApiMatchingServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckMatching : Controller
{
    IMatchWoker _matchWorker;


    public CheckMatching(IMatchWoker matchWorker)
    {
        _matchWorker = matchWorker;
    }

    [HttpPost]
    public CheckMatchingRes Post(CheckMatchingReq request)
    {
        CheckMatchingRes response = new();

        (var result, var completeMatchingData) = _matchWorker.GetCompleteMatching(request.UserID);
        
        //TODO: 결과를 담아서 보낸다

        return response;
    }


}

