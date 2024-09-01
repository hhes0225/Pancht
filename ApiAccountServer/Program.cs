using ApiAccountServer;
using ApiAccountServer.Repository;
using ApiAccountServer.Service;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Service DI 등록
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

//Repository DI 등록
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


//Controller 등록
builder.Services.AddControllers();

////Cors 설정
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.AllowAnyOrigin()    // 모든 도메인 허용
//               .AllowAnyMethod()    // 모든 HTTP 메서드 허용 (GET, POST, PUT, DELETE 등)
//               .AllowAnyHeader();   // 모든 헤더 허용
//    });
//});



//Configuration 설정 - db 설정 로드
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));



var app = builder.Build();

//Routing 설정
//속성 기반 라우팅 사용, REST API 구축 시 사용
app.MapControllers();


app.Run(configuration["ServerUrl"]);
