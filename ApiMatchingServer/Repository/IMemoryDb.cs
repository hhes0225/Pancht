namespace ApiMatchingServer.Repository;

public interface IMemoryDb
{
    //매칭 큐에 넣기 - id : tierscore sorted set에 넣기
    //매칭 큐에서 빼기 - id : tierscore sorted set에서 빼기
    //유저 정보 해시에 넣기 - id, 승패정보, 유저상태 hash에 넣기
    //유저 정보 해시에서 빼기 - id, 승패정보, 유저상태 hash에서 빼기
    //매칭 정보 리스트에 넣기

}
