using Google.Protobuf.WellKnownTypes;
using Grpc.Core;


namespace ChatServer.Services;

// :todo
// - test grpc handlers
// - performance test
public class ChatServerService : ChatServer.ChatServerBase
{
    private readonly ILogger<GreeterService> _logger;
    public ChatServerService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateResponce> Create(CreateRequest request, ServerCallContext context)
    {
        return Task.FromResult(new CreateResponce(){ Id = 41241234});
    }

    public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
    {
        return Task.FromResult(new Empty());
    }

    public override async Task<Empty> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        return await Task.FromResult(new Empty());
    }
}
