using ApiGameServer;
using ApiGameServer.Repository;
using ApiGameServer.Service;

var builder = WebApplication.CreateBuilder(args);

//Service DI(등록)
//builder.Services.AddHttpClient();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IAccountServerAuthHandler, AccountServerAuthHandler>();

//Repository DI(등록)
builder.Services.AddScoped<IPanchtDb, PanchtDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();

//Controller 등록
builder.Services.AddControllers();

//Configuration 지정
IConfiguration configuration = builder.Configuration; 
builder.Services.Configure<DbConfig>(configuration.GetSection("DbConfig"));


var app = builder.Build();

//미들웨어 등록

//라우팅 방식 지정 - 컨트롤러에 따른다
app.MapControllers();

app.Run(configuration["ServerUrl"]);
