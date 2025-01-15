public class ChannelService{
    Dictionary<string, Channel> channels;

    public ChannelService(){
        channels = new Dictionary<string, Channel>();
    }

    public Channel getChannel(string name){
        if(!channels.ContainsKey(name)){
            channels.Add(name, new Channel(name));
        }
        return channels[name];
    }
}