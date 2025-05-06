using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ApiMatchingServer;
using ApiMatchingServer.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.Configure<MatchingConfig>(configuration.GetSection(nameof(MatchingConfig)));

//Repository DI
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();
builder.Services.AddSingleton<IMatchWoker, MatchWoker>();

builder.Services.AddControllers();
builder.WebHost.UseUrls(configuration["ServerUrl"]);


WebApplication app = builder.Build();

app.MapControllers();
app.Run(configuration["ServerUrl"]);


