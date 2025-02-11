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
using ApiMatchingServer.Model;

namespace ApiMatchingServer;

public interface IMatchWoker : IDisposable
{
    public void AddUserToWaitingQueue(UserMatchInfo userMatchInfo);
    public void RemoveUserFromWaitingQueue(string userID);

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID);
}

public class MatchWoker : IMatchWoker
{
    readonly ILogger<MatchWoker> _logger;
    readonly IMemoryDb _memoryDb;

    List<string> _pvpServerAddressList = new();

    System.Threading.Thread _reqWorker = null;
    System.Threading.Thread _completeWorker = null;

    ConcurrentQueue<UserMatchInfo> _waitingQueue = new();
    ConcurrentDictionary<string, string> _completeDic = new();// key는 유저ID

    

    public MatchWoker(ILogger<MatchWoker> logger, IMemoryDb memoryDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;

        _reqWorker = new System.Threading.Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new System.Threading.Thread(this.RunMatchingComplete);
        _completeWorker.Start();
    }
    
    public void AddUserToWaitingQueue(UserMatchInfo userMatchInfo)
    {
        _waitingQueue.Enqueue(userMatchInfo);

        foreach (var tmp in _waitingQueue)
        {
            Console.WriteLine($"큐 안의 값 확인 : {tmp.Id}, 티어: {tmp.TierScore}, 연승: {tmp.WinStreak}");
        }
    }

    public void RemoveUserFromWaitingQueue(string userID)
    { 
    
    }

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID)
    {
        //TODO: _completeDic에서 검색해서 있으면 반환한다.

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

                //TODO: 큐에서 2명을 가져온다. 두명을 매칭시킨다

                //TODO: Redis의 Pub/Sub을 이용해서 매칭된 유저들을 게임서버에 전달한다.


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
