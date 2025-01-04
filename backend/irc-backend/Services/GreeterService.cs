using System.Text.RegularExpressions;
using Grpc.Core;
using Irc;
using irc_backend;
using Grpc.Net.Client;
// using GrpcIrcServiceClient;
// using Irc.IrcServiceClient;

namespace irc_backend.Services;

public class IrcServiceServer : IrcService.IrcServiceBase
{
    private readonly ILogger<IrcServiceServer> _logger;
    Dictionary<String, IrcService.IrcServiceClient> clients;
    

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
        clients = new Dictionary<String, IrcService.IrcServiceClient>();
    }

    public override Task<IrcReply> SendMessage(IrcMessage request, ServerCallContext context)
    {
        Console.Write(request.Message);
        Console.Write(context.Peer);
        var r = new Regex(@":\s+NICK\s+(\w+).*");
        var match = r.Match(request.Message);
        if (match.Success){
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = 
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            using var channel = GrpcChannel.ForAddress("http://localhost:50051",
                 new GrpcChannelOptions { HttpHandler = handler });
            var client = new IrcService.IrcServiceClient(channel);            
            var reply = client.SendMessage(new IrcMessage { Message = ":banana PRIVMSG :ahahahahaha\r\n" });
            // var reply = await client.SendMessage(
            //     new IrcMessage { Message = ":banana PRIVMSG ahahahahaha\r\n" });
            Console.WriteLine("Reply: " + reply.Message);  
            clients.Add(match.Groups[1].Value, client);
        }


        return Task.FromResult(new IrcReply
        {            
            Message = "400"
        });
    }

}
