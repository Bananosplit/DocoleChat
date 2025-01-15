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

    static ChannelService channelService = new ChannelService();    
    
    public static Dictionary<string, User> usersByToken = new Dictionary<string, User>();        

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
    }

    private string ParseMessage(string token, string message){
        Regex r;
        Match match;
        if(!usersByToken.ContainsKey(token)){
            return "500";
        }

        User user = usersByToken[token];
        r = new Regex(@"PASS\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(match.Groups[1].Value == "letmein"){   
                if(user.passed){
                    return "462";
                }
                user.passed = true;
                return "400";            
            } else {
                return "watch";
            }
        }

        if(!user.passed){
            return "watch";
        }

        r = new Regex(@"NICK\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(usersByToken.ContainsKey(token)){
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
            Channel channel = channelService.getChannel(channel_name);            
            channel.AddMessage(":" + nickname + " JOIN" + "\r\n");
            channel.AddUser(user);            
            return "400";    
        }
        r = new Regex(@"PART\s+(#?\w+)\s*(:\w*)?\r\n");
        match = r.Match(message);
        if(match.Success){
            string channel_name = match.Groups[1].Value;
            string nickname = user.Nick;
            Channel channel = channelService.getChannel(channel_name);
            if(match.Groups[2].Value == ""){
                channel.AddMessage(":" + nickname + " PART " + channel_name + "\r\n");
            } else {
                channel.AddMessage(":" + nickname + " PART " + channel_name + " :" + match.Groups[2].Value + "\r\n");
            }
            
            channel.AddUser(user);            
            return "400";    
        }
        
        r = new Regex(@"PRIVMSG\s+(#?\w+)\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){ 
            string nickname = user.Nick;
            string channel_name = match.Groups[1].Value;
            Channel channel = channelService.getChannel(channel_name);            
            if(channel.Contains(user)){
                channel.AddMessage(":" + nickname + " " + match.Groups[0].Value + "\r\n");
                return "400";  
            } else {
                return "watch";
            }
        }

        r = new Regex(@"QUIT\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){             
            user.passed = false;            
            return "400";  
        }

        return "421";  
    }

    public override Task<IrcToken> GetToken(IrcVoid request, ServerCallContext context){
        var token = new IrcToken{
            // Token = counter.ToString(),
            Token = ""
        };
        // counter++;
        
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
        foreach(var mes in usersByToken[request.Token].GetMessages()){            
            await responseStream.WriteAsync(new IrcMessage{Message = mes});            
        }        
    }

}
