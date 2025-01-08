class User{
    public string Token {get; set; } = string.Empty;
    public string Nick {get; set; } = string.Empty;

    public List<string> messages;
    public List<string> channels;

    User(){
        messages = new List<string>();
        channels = new List<string>();
    }
}