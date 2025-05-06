﻿// 클라이언트가 플랫폼 서버에서 인증을 받았는지 확인해 준다
// 클라이언트는 플랫폼 서버에서 받은 인증토큰과 자신의 인증ID(계정 ID 혹은 플랫폼에서 만들어준 ID)로 보낸다.
// 사용할 수 있는 인증ID와 인증토큰은 이미 정해져 있다.
// 게임 서버는 인증이 성공하면
// - 이 유저의 default 게임데이터가 없다면 생성해줘야 한다.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using ApiMatchingServer.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZLogger;


namespace ApiMatchingServer.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestMatchingController : ControllerBase
{
    IMatchWoker _matchWorker;
    ILogger<RequestMatchingController> _logger;

    public RequestMatchingController(IMatchWoker matchWorker, ILogger<RequestMatchingController> logger)
    {
        _matchWorker = matchWorker;
        _logger = logger;
    }

    [HttpPost]
    public MatchingResponse Post(MatchingRequest request)
    {
        //MatchingRequest 출력
        _logger.LogInformation("MatchingRequest received: {UserID}, {TierScore}, {LastGameResult}", request.UserID, request.TierScore, request.LastGameResult);

        MatchingResponse response = new();

        //_matchWorker.AddUserToWaitingQueue(request.UserID);

        return response;
    }

}


