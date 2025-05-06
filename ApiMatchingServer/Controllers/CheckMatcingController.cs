using ApiMatchingServer.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZLogger;
using static ApiMatchingServer.Controllers.CheckMatchingController;

namespace ApiMatchingServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckMatchingController : ControllerBase
{
    IMatchWoker _matchWorker;
    ILogger<CheckMatchingController> _logger;


    public CheckMatchingController(IMatchWoker matchWorker, ILogger<CheckMatchingController> logger)
    {
        _matchWorker = matchWorker;
        _logger = logger;
    }

    [HttpPost]
    public CheckMatchingRes Post(CheckMatchingReq request)
    {
        CheckMatchingRes response = new();

        _logger.LogInformation("CheckMatchingRequest received: {UserID}", request.Id);

        if(request.Id==null)
        {
            response.Result = ErrorCode.AuthCheckFail;
            return response;
        }
        
        response.Result = ErrorCode.MatchingNotYet;

        //(var result, var completeMatchingData) = _matchWorker.GetCompleteMatching(request.UserID);

        //TODO: 결과를 담아서 보낸다

        return response;
    }


}

