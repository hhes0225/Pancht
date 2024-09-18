﻿using ApiGameServer.Models.DTO;
using ApiGameServer.Repository;
using System.Text.Json;

namespace ApiGameServer.Service;

public class LoginService:ILoginService
{
    private readonly ILogger<LoginService> _logger;
    private readonly string _apiServerAddress;
    private readonly IUserDataDb _userDataDb;
    private readonly IMemoryDb _memoryDb;

    public LoginService(ILogger<LoginService> logger, IConfiguration configuration, IUserDataDb userDataDb, IMemoryDb memoryDb)
    {
        _logger = logger;
        _apiServerAddress = configuration["AccountServerUrl"];
        _userDataDb = userDataDb;
        _memoryDb = memoryDb;
    }


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        //AccountServer에 HTTP Request로 사용자 정보 조회 요청
        var accountServerResponse = await SendVerifyRequestAsync(request.Id, request.AuthToken);
        var loginResponse = new LoginResponse();

        if (accountServerResponse.IsSuccessStatusCode)
        {
            //json 형식으로 응답 내용 읽어오기
            var responseContent = await accountServerResponse.Content.ReadAsStringAsync();
            responseContent = responseContent.Replace("result", "Result");
            //json attribute 명 소문자로 된 것을 클래스 형식에 맞게 대문자로 변환

            //응답 내용을 json에서 deserialize하여 LoginResponse 객체로 변환
            var response = JsonSerializer.Deserialize<LoginResponse>(responseContent);


            if (response.Result != ErrorCode.None)
            {
                return response;
            }
        }
        else
        {
            loginResponse.Result=ErrorCode.LoginFailAccountConnectionException;
            return loginResponse;
        }

        return loginResponse;
    }
    

    //Client request를 생성하여 ApiAccountServer에 전달
    public async Task<HttpResponseMessage> SendVerifyRequestAsync(string id, string authToken)
    {
        HttpClient client = new HttpClient();

        var accountServerResponse = await client.PostAsJsonAsync(_apiServerAddress, new
        {
            Id = id,
            AuthToken = authToken
        });

        return accountServerResponse;
    }

    //유저 데이터 로드
}