﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

using CloudStructures;
using CloudStructures.Structures;
using System.Linq;

namespace APIServer;

public interface IMatchWoker : IDisposable
{
    public void AddUser(string userID);
    //public void RemoveUser(string userID);

    public (bool, CompleteMatchingData) GetCompleteMatching(string userID);
}

public class MatchWoker : IMatchWoker
{
    List<string> _pvpServerAddressList = new();

    System.Threading.Thread _reqWorker = null;
    System.Threading.Thread _completeWorker = null;


    ConcurrentQueue<string> _reqQueue = new();
    ConcurrentDictionary<string, string> _completeDic = new();// key는 유저ID

    //TODO: 2개의 Pub/Sub을 사용하므로 Redis 객체가 2개 있어야 한다.
    // 매칭서버에서 -> 게임서버, 게임서버 -> 매칭서버로

    string _redisAddress = "";
    string _requestMatchingKey;
    string _checkMatchingKey;

    public RedisConnection RedisConn;

    public MatchWoker(IOptions<MatchingConfig> matchingConfig)
    {
        Console.WriteLine("MatchWoker 생성자 호출");

        _redisAddress = matchingConfig.Value.RedisAddress;

        //TODO: Redis 연결 및 초기화 한다
        RedisConfig redisConfig = new RedisConfig("default", matchingConfig.Value.RedisAddress);
        RedisConn = new RedisConnection(redisConfig);

        _reqWorker = new System.Threading.Thread(this.RunMatching);
        _reqWorker.Start();

        _completeWorker = new System.Threading.Thread(this.RunMatchingComplete);
        _completeWorker.Start();
    }
    
    public void AddUser(string userID)
    {
        _reqQueue.Enqueue(userID);
        
        foreach(var tmp in _reqQueue)
        {
            Console.WriteLine($"큐 안의 값 확인 : {tmp}");
        }
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

                if (_reqQueue.Count < 2)
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


public class MatchingConfig
{
    public string RedisAddress { get; set; }
    public string PubKey { get; set; }
    public string SubKey { get; set; }
}