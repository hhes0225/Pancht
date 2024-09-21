using ApiGameServer.Repository;

namespace ApiGameServer.Middleware;

public class CheckAuthMiddleware
{
    RequestDelegate _next;
    IMemoryDb _memoryDb;
    ILogger<CheckAuthMiddleware> _logger;

    public CheckAuthMiddleware(ILogger<CheckAuthMiddleware> logger, RequestDelegate next, IMemoryDb memoryDb)
    {
        _logger = logger;
        _next = next;
        _memoryDb = memoryDb;
    }

    public async Task Invoke(HttpContext context)
    {
        var url = context.Request.Path.Value;

        //로그인 요청, 유저 생성 요청은 인증 체크를 하지 않음
        if (url=="/Login" || url=="CreateUser")
        {
            await _next(context);
        }
        else
        {
            // 모든 헤더 출력
            foreach (var header in context.Request.Headers)
            {
                _logger.LogInformation($"{header.Key}: {header.Value}");
            }


            var id = context.Request.Headers["Id"].ToString();
            var authToken = context.Request.Headers["AuthToken"].ToString();

            var verifyResult = await _memoryDb.VerifyAccessTokenAsync(id, authToken);

            if (verifyResult != ErrorCode.None)
            {
                context.Response.StatusCode = 400;
                return;
            }

            _logger.LogInformation("Success Auth Check");
            await _next(context);
        }

        return;
    }
}
