using ApiGameServer;
using ApiGameServer.Middleware;
using ApiGameServer.Repository;
using ApiGameServer.Service;
using ApiGameServer.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//Service DI(���)
//builder.Services.AddHttpClient();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IAccountServerAuthHandler, AccountServerAuthHandler>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICharacterListService, CharacterListService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IMatchingService, MatchingService>();


//Repository DI(���)
builder.Services.AddScoped<IPanchtDb, PanchtDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();
builder.Services.AddScoped<IUserStateDb, UserStateDb>();

//Controller ���
builder.Services.AddControllers();

//Configuration ����
IConfiguration configuration = builder.Configuration; 
builder.Services.Configure<DbConfig>(configuration.GetSection("DbConfig"));


var app = builder.Build();

//�̵���� ���
app.UseMiddleware<CheckAuthMiddleware>();

//����� ��� ���� - ��Ʈ�ѷ��� ������
app.MapControllers();

app.Run(configuration["ServerUrl"]);
