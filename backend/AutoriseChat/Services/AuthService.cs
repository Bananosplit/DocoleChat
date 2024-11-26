using AutoriseChat.ModuleServices;
using Grpc.Core;
using System.Runtime.Intrinsics.X86;

namespace AutoriseChat.Services;

public class AuthService : ChatAuth.ChatAuthBase
{
    private readonly ILogger<AuthService> _logger;
    private readonly TokenValidator tokenValidator;

    public AuthService(ILogger<AuthService> logger, TokenValidator tokenValidator)
    {
        _logger = logger;
        this.tokenValidator = tokenValidator;
    }

    public override Task<CreateTokenResponce> CreateToken(CreateTokenRequest request, ServerCallContext context)
    {
        var vm = new LoginModel() { 
            Username = request.Name,
            Password = request.Password
        };

        var token = tokenValidator.GenerateToken(vm);
        var result = new CreateTokenResponce() { Token = token };

        return Task.FromResult(result);
    }
}
