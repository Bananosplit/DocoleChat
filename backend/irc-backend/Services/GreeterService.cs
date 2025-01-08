using System.Text.RegularExpressions;
using Grpc.Core;
using Irc;
using irc_backend;
using Grpc.Net.Client;
using Microsoft.VisualBasic;
// using GrpcIrcServiceClient;
// using Irc.IrcServiceClient;

namespace irc_backend.Services;

public class IrcServiceServer : IrcService.IrcServiceBase
{
    private readonly ILogger<IrcServiceServer> _logger;

    // TODO message in chat to detach class

    // TODO do nickname token convert to other class
    static Dictionary<String, String> NicknameTokenDict = new Dictionary<string, string>();
    static Dictionary<String, String> TokenNicknameDict = new Dictionary<string, string>();

    // TODO do channel is detach class
    static Dictionary<String, List<String>> channelMessages = new Dictionary<string, List<string>>();
    static Dictionary<String, List<String>> nicknameDiscribes = new Dictionary<string, List<string>>();
    static int counter = 0;

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
    }

    private String ParseMessage(String token, String message){
        Regex r;
        Match match;
        r = new Regex(@":\s+NICK\s+(\w+).*");
        match = r.Match(message);
        if (match.Success){
            NicknameTokenDict.Add(match.Groups[1].Value, token);   
            TokenNicknameDict.Add(token, match.Groups[1].Value);
            return "400";            
        }

        r = new Regex(@":(\w+)\s+JOIN\s+(\w+)\r\n");
        match = r.Match(message);
        if(match.Success){
            String channel_name = match.Groups[2].Value;
            String nickname = match.Groups[1].Value;
            if(!channelMessages.ContainsKey(channel_name)){
                channelMessages.Add(channel_name, new List<string>());
            }
            if(!nicknameDiscribes.ContainsKey(nickname)){
                nicknameDiscribes.Add(nickname, new List<string>());
            }
            nicknameDiscribes[nickname].Add(channel_name);
            return "400";    
        }
        
        r = new Regex(@":(\w+)\s+PRIVMSG\s+(\w+)\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){ 
            String nick = match.Groups[1].Value;
            String channel_name = match.Groups[2].Value;
            String text = match.Groups[3].Value;
            if(!channelMessages.ContainsKey(channel_name)){
                return "401";
            }
            channelMessages[channel_name].Add(nick + ": " + text);
            return "400";  
        }

        return "400";  
    }

    public override Task<IrcToken> GetToken(IrcVoid request, ServerCallContext context){
        var token = new IrcToken{
            Token = counter.ToString(),
        };
        counter++;
        
        return Task.FromResult(token);
    }
    public override Task<IrcReply> SendMessage(IrcMessage request, ServerCallContext context)
    {       
        return Task.FromResult(new IrcReply{
            Message = ParseMessage(request.Token ,request.Message)
        });
    }

    public override async Task GetMessages(IrcToken request, IServerStreamWriter<IrcMessage> responseStream, ServerCallContext context)
    {   
        String nickname = TokenNicknameDict[request.Token];
        foreach(var ch in nicknameDiscribes[nickname]){
            foreach(var mes in channelMessages[ch]){
                await responseStream.WriteAsync(new IrcMessage{Message = mes});
            }        
        }
        
    }

}
