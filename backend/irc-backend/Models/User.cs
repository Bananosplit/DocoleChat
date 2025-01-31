// namespace irc_backend.Services;

public class User{
    public string Token {get; set; } = string.Empty;
    public string Nick {get; set; } = string.Empty;

    public List<string> messages;
    // public List<string> channels;

    public bool online;    
    public bool passed;

    public User(){
        messages = new List<string>();
        // channels = new List<string>();
        online = false;
        passed = false;
    }

    public void AddMessage(string mes){
        messages.Add(mes);
    }

    public List<string> GetMessages(){
        List<string> result = new List<string>(messages);
        messages.Clear();
        return result;
    }
}