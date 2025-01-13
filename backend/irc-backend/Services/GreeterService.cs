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
    static Dictionary<String, User> nicknameMessages = new Dictionary<string, User>();
    static Dictionary<String, List<String>> channelMembers = new Dictionary<string, List<string>>();

    // static Dictionary<User, >
    static int counter = 0;

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
    }

    private String ParseMessage(String token, String message){
        Regex r;
        Match match;
        r = new Regex(@"NICK\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            // foreach(var v in match.Groups.Values){
            //     Console.WriteLine(v.Value);
            // }
            User user = new User();
            user.Nick = match.Groups[1].Value;
            string nickname = user.Nick;
            NicknameTokenDict.Add(nickname, token);   
            TokenNicknameDict.Add(token, nickname);
            nicknameMessages.Add(nickname, user);
            return "400";            
        }

        r = new Regex(@"JOIN\s+(\w+)\r\n");
        match = r.Match(message);
        if(match.Success){
            String channel_name = match.Groups[1].Value;
            String nickname = TokenNicknameDict[token];
            if(!channelMessages.ContainsKey(channel_name)){
                channelMessages.Add(channel_name, new List<string>());
            }
            if(!channelMembers.ContainsKey(channel_name)){
                channelMembers.Add(channel_name, new List<string>());
            }
            channelMembers[channel_name].Add(nickname);
            foreach(var mes in channelMessages[channel_name]){
                nicknameMessages[nickname].messages.Add(mes);
            }
            return "400";    
        }
        
        r = new Regex(@"PRIVMSG\s+(\w+)\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){ 
            String nickname = TokenNicknameDict[token];
            String channel_name = match.Groups[1].Value;
            String text = match.Groups[2].Value;
            if(!channelMessages.ContainsKey(channel_name)){
                return "401";
            }
            channelMessages[channel_name].Add(":" + nickname + " " + match.Groups[0].Value);
            foreach(var nk in channelMembers[channel_name]){
                nicknameMessages[nk].AddMessage(":" + nickname + " " + match.Groups[0].Value);
            }
            return "400";  
        }

        return "421";  
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
        foreach(var mes in nicknameMessages[nickname].GetMessages()){            
            await responseStream.WriteAsync(new IrcMessage{Message = mes});            
        }        
    }

}
