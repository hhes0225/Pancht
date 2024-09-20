using ApiGameServer;
using ApiGameServer.Repository;
using ApiGameServer.Service;

var builder = WebApplication.CreateBuilder(args);

//Service DI(���)
//builder.Services.AddHttpClient();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IAccountServerAuthHandler, AccountServerAuthHandler>();

//Repository DI(���)
builder.Services.AddScoped<IPanchtDb, PanchtDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();

//Controller ���
builder.Services.AddControllers();

//Configuration ����
IConfiguration configuration = builder.Configuration; 
builder.Services.Configure<DbConfig>(configuration.GetSection("DbConfig"));


var app = builder.Build();

//�̵���� ���

//����� ��� ���� - ��Ʈ�ѷ��� ������
app.MapControllers();

app.Run(configuration["ServerUrl"]);
