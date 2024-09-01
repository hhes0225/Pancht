using ApiAccountServer;
using ApiAccountServer.Repository;
using ApiAccountServer.Service;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//Service DI ���
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();

//Repository DI ���
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


//Controller ���
builder.Services.AddControllers();

////Cors ����
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.AllowAnyOrigin()    // ��� ������ ���
//               .AllowAnyMethod()    // ��� HTTP �޼��� ��� (GET, POST, PUT, DELETE ��)
//               .AllowAnyHeader();   // ��� ��� ���
//    });
//});



//Configuration ���� - db ���� �ε�
IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));



var app = builder.Build();

//Routing ����
//�Ӽ� ��� ����� ���, REST API ���� �� ���
app.MapControllers();


app.Run(configuration["ServerUrl"]);
