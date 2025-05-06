using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

using CloudStructures;
using CloudStructures.Structures;
using System.Linq;
using ApiMatchingServer.Repository;
using Microsoft.Extensions.Logging;
using ApiMatchingServer.Model.DAO;
using System.Threading.Tasks;
using UserStateLibrary;

namespace ApiMatchingServer;

public interface IMatchWoker : IDisposable
{
    public Task<ErrorCode> AddUserToWaitingQueue(UserMatchInfo userMatchInfo);
    public Task<ErrorCode> RemoveUserFromWaitingQueue(string userID);

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID);
}

public class MatchWoker : IMatchWoker
{
    readonly ILogger<MatchWoker> _logger;
    readonly IMemoryDb _memoryDb;
    readonly UserStateManager _userStateManager;

    List<string> _pvpServerAddressList = new();

    System.Threading.Thread _reqWorker = null;
    System.Threading.Thread _completeWorker = null;

    ConcurrentQueue<UserMatchInfo> _waitingQueue = new();
    ConcurrentDictionary<string, CompleteMatchingData> _completeDic = new();// key는 유저ID

    public MatchWoker(ILogger<MatchWoker> logger, IMemoryDb memoryDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;

        _reqWorker = new System.Threading.Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new System.Threading.Thread(this.RunMatchingComplete);
        _completeWorker.Start();
    }
    
    public async Task<ErrorCode> AddUserToWaitingQueue(UserMatchInfo userMatchInfo)
    {
        var result = ErrorCode.None;
        _waitingQueue.Enqueue(userMatchInfo);

        //var result = await _memoryDb.SetUserState(userMatchInfo.Id, UserState.Matching);
        //var result = await _userStateManager.ChangeStateIfMatchAsync(usetMatchInfo.Id, )


        foreach (var tmp in _waitingQueue)
        {
            Console.WriteLine($"큐 안의 값 확인 : {tmp.Id}, 티어: {tmp.TierScore}, 연승: {tmp.WinStreak}");
        }

        return result;
    }

    public async Task<ErrorCode> RemoveUserFromWaitingQueue(string userID)
    {
        //return await _memoryDb.SetUserState(userID, UserState.None);
        var result = ErrorCode.None;

        try
        {
            var changeState = await _userStateManager.ChangeStateIfMatchAsync(userID, UserState.Matching, UserState.None);

            if (changeState == false)
            {
                _logger.LogError($"매칭 큐에서 유저 제거 실패 : {userID}");
                result = ErrorCode.MatchingServerUserStateNotExist;
                return result;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "RemoveUserFromWaitingQueue Error");
        }

        return result;
    }

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID)
    {
        //TODO: _completeDic에서 검색해서 있으면 반환한다.
        if( _completeDic.TryGetValue(userID, out var result))
        {
            return (true, result);
        }


        return (false, null);
    }

    void RunMatching()
    {
        while (true)
        {
            try
            {

                if (_waitingQueue.Count < 2)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                //TODO: 큐에서 2명을 가져온다. 두명을 매칭 로직에 따라 매칭시킨다
                //var matchingResult = MatchingLogic();

                //TODO: Redis의 Pub/Sub을 이용해서 매칭된 유저들을 게임서버에 전달한다.


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    void MatchingLogic()
    {
        if (_waitingQueue.Count < 2)
        {
            return;
        }

        //큐에서 유저 2명 성공적으로 꺼내왔을 경우
        if (_waitingQueue.TryDequeue(out var user1) && _waitingQueue.TryDequeue(out var user2))
        {
            _logger.LogInformation($"매칭 성공 : {user1.Id} vs {user2.Id}");
        }
        else
        {
            _logger.LogError("매칭 실패");
            //_waitingQueue.Enqueue(user1);
            //_waitingQueue.Enqueue(user2);
        }
    }

        void RunMatchingComplete()
    {
        while (true)
        {
            try
            {
                //TODO: Redis의 Pub/Sub을 이용해서 매칭된 결과를 게임서버로 받는다

                //TODO: 매칭 결과를 _completeDic에 넣는다
                // 2명이 하므로 각각 유저를 대상으로 총 2개를 _completeDic에 넣어야 한다
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }        
    }



    public void Dispose()
    {
        Console.WriteLine("MatchWoker 소멸자 호출");
    }
}


public class CompleteMatchingData
{    
    public string ServerAddress { get; set; }
    public int RoomNumber { get; set; }
}
