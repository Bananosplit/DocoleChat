class Channel{
    public List<string> users;
    public List<string> messages;

    public string name;

    public Channel(string name){
        this.name = name;
        users = new List<string>();
        messages = new List<string>();
    }
}
