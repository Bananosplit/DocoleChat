using System.Text;
using AutoriseChat.ModuleServices;
using AutoriseChat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// TODO:
// - сделать сервис авторизации:
//      - сделать jwt авторизацию
//      - сделать api авторизации для grpc сервиса
//      - сделать таблицу для бд постгрес, или прикрутить автоматическую 
//      - протестировать (возможно использовать unit тесты) 
//      - настроить трейсинг и логирование сервиса (продумать хранение логов и трейсов)
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException())
                    )
            };
        });
        builder.Services.AddSingleton<TokenValidator>();
        
        builder.Services.AddAuthorization();
        builder.Services.AddGrpc();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<AuthService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}