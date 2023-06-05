using Shortening.API.Constants;
using Shortening.API.Contexts;
using Shortening.API.UnitOfWorks.Abstracts;
using Shortening.API.UnitOfWorks.Concretes;
using Shortening.API.Repositories.Abstracts;
using Shortening.API.Repositories.Concretes;
using Shortening.API.Entities;
using Shortening.API.Services.Concretes;
using Shortening.API.Services.Abstracts;
using Shortening.API.Mappings;
using FluentValidation;
using Shortening.API.Dtos.RequestDtos;
using Shortening.API.Validators;
using Shortening.API.Middlewares;
using Serilog;
using Shortening.API.Caching.Abstracts;
using Shortening.API.Caching.Concretes;
using Shortening.API.Adapters;

var builder = WebApplication.CreateBuilder(args);

DatabaseConstants.MSSQL_CONNECTION_STRING = builder.Configuration.GetConnectionString("MSSqlString");
ShorteningConstants.SHORTENING_DOMAIN_URL = builder.Configuration.GetValue<string>("ShorteningDomainUrl");
CachingConstants.REDIS_URL = builder.Configuration.GetValue<string>("RedisURL");

builder.Services.AddDbContext<ShorteningDbContext>();

builder.Services.AddAutoMapper(typeof(UrlShorteningProfile));

builder.Services.AddScoped<IValidator<CreateUrlShorteningRequestDto>, CreateUrlShorteningValidator>();

builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork<ShorteningDbContext>>();

builder.Services
    .AddScoped<IEFRepository<UrlShorteningEntity, ShorteningDbContext>, EFRepository<UrlShorteningEntity, ShorteningDbContext>>();

builder.Services.AddScoped<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddScoped<CahceAdapter>();

builder.Services.AddTransient<ErrorHandlingMiddleware>();

builder.Services.AddLogging(config => { 
    config.AddConsole();
    config.AddDebug();
});

// Use Serilog
builder.Host.UseSerilog((hostContext, services, configuration) => {
    configuration
        .WriteTo.File($"{Directory.GetCurrentDirectory()}/logs/serilog-file.txt");
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
