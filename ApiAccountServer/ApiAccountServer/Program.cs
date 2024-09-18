using ApiAccountServer;
using ApiAccountServer.Repository;
using ApiAccountServer.Service;
using ApiAccountServer.Security;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Service DI 등록
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

//Repository DI 등록
builder.Services.AddScoped<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();


//Controller 등록
builder.Services.AddControllers();


//Configuration 설정 - db 설정 로드
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));



var app = builder.Build();

//Routing 설정
//속성 기반 라우팅 사용, REST API 구축 시 사용
app.MapControllers();


app.Run(configuration["ServerUrl"]);
