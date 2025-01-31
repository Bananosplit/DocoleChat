public class ChannelService{
    Dictionary<string, Channel> channels;
    List<Channel> channelList;

    public ChannelService(){
        channels = new Dictionary<string, Channel>();
        channelList = new List<Channel>();
    }

    public Channel getChannel(string name){
        if(!channels.ContainsKey(name)){
            channels.Add(name, new Channel(name));
            channelList.Add(channels[name]);
        }
        return channels[name];
    }

    public bool Contains(string name){
        return channels.ContainsKey(name);
    }

    public List<Channel> getChannelList(){
        return channelList;        
    }
}