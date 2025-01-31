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

    static string ServerName = "server";  

    public IrcServiceServer(ILogger<IrcServiceServer> logger)
    {
        _logger = logger;
    }

    private void ParseMessage(string token, string message){
        Regex r;
        Match match;
        if(!usersByToken.ContainsKey(token)){
            // return "500";
            User new_user = new User();
            new_user.Token = token;
            usersByToken[token] = new_user;
        }

        User user = usersByToken[token];
        r = new Regex(@"PASS\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(match.Groups[1].Value == "letmein"){   
                if(user.passed){
                    return;
                }
                user.passed = true;
                return;
            } else {
                return;
            }
        }

        if(!user.passed){
            user.AddMessage(":" + ServerName + " 464 :Password incorrect\r\n");
            return;
        }

        r = new Regex(@"NICK\s+(\w+)\s*\r\n");
        match = r.Match(message);
        if (match.Success){
            if(usersByToken.ContainsKey(token)){
                user.Nick = match.Groups[1].Value;
                user.AddMessage(":" + ServerName + " 400 " + user.Nick + " :Welcome!\r\n");
                return;
            } else {
                return;
            }         
        }

        r = new Regex(@"JOIN\s+(#?\w+)\s*\r\n");
        match = r.Match(message);
        if(match.Success){
            string channel_name = match.Groups[1].Value;
            string nickname = user.Nick;
            Channel channel = channelService.getChannel(channel_name);
            if(!channel.Contains(user)){
                channel.AddUser(user);
                channel.AddMessage(":" + user.Nick + " " + match.Groups[0].Value);
                user.AddMessage(":" + ServerName + " 400 :" + channel.topic + "\r\n");
            }
            return;
        }
        
        r = new Regex(@"PART\s+(#?\w+)\s*(:\w*)?\r\n");
        match = r.Match(message);
        if(match.Success){
            string channel_name = match.Groups[1].Value;
            string nickname = user.Nick;
            Channel channel = channelService.getChannel(channel_name);            
            channel.AddMessage(":" + nickname + " " + match.Groups[0].Value);
            channel.RemoveUser(user);
            user.AddMessage(":" + ServerName + " 400");
            return;    
        }
        
        r = new Regex(@"PRIVMSG\s+(#?\w+)\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){ 
            string nickname = user.Nick;
            string channel_name = match.Groups[1].Value;
            if(!channelService.Contains(channel_name)){
                user.AddMessage(":" + ServerName + " 403 :No such channel\r\n");
            }
            Channel channel = channelService.getChannel(channel_name);  
            if(channel.Contains(user)){
                channel.AddMessage(":" + nickname + " " + match.Groups[0].Value);
                user.AddMessage(":" + ServerName + " 301 :" + match.Groups[2].Value + "\r\n");
            }
            return;
        }

        r = new Regex(@"LIST\s*\r\n");
        match = r.Match(message);
        if (match.Success){ 
            string nickname = user.Nick;
            string channel_name = match.Groups[1].Value;
            var channels = channelService.getChannelList();  
            user.AddMessage(":" + ServerName + " 321 " + user.Nick + " :Channel: Users Name\r\n");
            foreach(var ch in channels){
                user.AddMessage(":" + ServerName + " 322 " + user.Nick + " :" + ch.name + " " +  ":" + ch.topic + "\r\n");
            }
            user.AddMessage(":" + ServerName + " 323 "+ user.Nick + " :End of /List\r\n");
            return;
        }

        r = new Regex(@"QUIT\s+:(.*)\r\n");
        match = r.Match(message);
        if (match.Success){             
            user.passed = false;            
            return;  
        }

        user.AddMessage(":" + ServerName + " 421 :Unknown command\r\n");
        // r = new Regex(@"LIST")

        // return "421";  
    }

    public override Task<IrcToken> GetToken(IrcVoid request, ServerCallContext context){
        var token = new IrcToken{
            // Token = counter.ToString(),
            Token = ""
        };
        // counter++;
        
        return Task.FromResult(token);
    }
    public override Task<IrcVoid> SendMessage(IrcMessage request, ServerCallContext context)
    {       
        ParseMessage(request.Token ,request.Message);
        return Task.FromResult(new IrcVoid());
    }

    public override async Task GetMessages(IrcToken request, IServerStreamWriter<IrcMessage> responseStream, ServerCallContext context)
    {           
        foreach(var mes in usersByToken[request.Token].GetMessages()){            
            await responseStream.WriteAsync(new IrcMessage{Message = mes});            
        }        
    }

}
