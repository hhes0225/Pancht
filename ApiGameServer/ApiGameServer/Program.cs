using ApiGameServer;
using ApiGameServer.Service;

var builder = WebApplication.CreateBuilder(args);

//Service DI(���)
builder.Services.AddScoped<ILoginService, LoginService>();

//Repository DI(���)

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
