class User{
    public string Token {get; set; } = string.Empty;
    public string Nick {get; set; } = string.Empty;

    public List<string> messages;
    public List<string> channels;

    public User(){
        messages = new List<string>();
        channels = new List<string>();
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