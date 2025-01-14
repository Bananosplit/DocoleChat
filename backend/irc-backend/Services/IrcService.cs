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

        
    static Dictionary<string, Channel> channels = new Dictionary<string, Channel>();
    static Dictionary<string, User> users = new Dictionary<string, User>();    

    // static Dictionary<User, >
    static int counter = 0;

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
    }

    private string ParseMessage(string token, string message){
        Regex r;
        Match match;
        User user = users[token];
        r = new Regex(@"PASS\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(match.Groups[1].Value == "letmein"){   
                if(user.connected){
                    return "462";
                }
                user.connected = true;
                return "400";            
            } else {
                return "watch";
            }
        }

        if(!user.connected){
            return "watch";
        }

        r = new Regex(@"NICK\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(users.ContainsKey(token)){
                user.Nick = match.Groups[1].Value;
                return "400";
            } else {
                return "watch";
            }         
        }

        r = new Regex(@"JOIN\s+(#?\w+)\s*\r\n");
        match = r.Match(message);
        if(match.Success){
            string channel_name = match.Groups[1].Value;
            string nickname = user.Nick;
            if(!channels.ContainsKey(channel_name)){
                channels.Add(channel_name, new Channel(channel_name));
            }
            channels[channel_name].users.Add(nickname);                        
            foreach(var mes in channels[channel_name].messages){
                user.messages.Add(mes);
            }
            return "400";    
        }
        
        r = new Regex(@"PRIVMSG\s+(#?\w+)\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){ 
            string nickname = user.Nick;
            string channel_name = match.Groups[1].Value;
            if(!channels.ContainsKey(channel_name)){
                return "401";
            }
            channels[channel_name].messages.Add(":" + nickname + " " + match.Groups[0].Value);
            foreach(var nk in channels[channel_name].users){
                users[token].AddMessage(":" + nickname + " " + match.Groups[0].Value);
            }
            return "400";  
        }

        r = new Regex(@"QUIT\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){             
            user.online = false;            
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
        foreach(var mes in users[request.Token].GetMessages()){            
            await responseStream.WriteAsync(new IrcMessage{Message = mes});            
        }        
    }

}
