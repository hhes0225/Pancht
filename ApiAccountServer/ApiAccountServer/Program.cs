using ApiAccountServer;
using ApiAccountServer.Repository;
using ApiAccountServer.Service;
using ApiAccountServer.Security;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Service DI ���
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

//Repository DI ���
builder.Services.AddScoped<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();


//Controller ���
builder.Services.AddControllers();


//Configuration ���� - db ���� �ε�
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));



var app = builder.Build();

//Routing ����
//�Ӽ� ��� ����� ���, REST API ���� �� ���
app.MapControllers();


app.Run(configuration["ServerUrl"]);
