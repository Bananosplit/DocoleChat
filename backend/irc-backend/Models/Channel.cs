class Channel{
    public List<User> users;
    public List<string> messages;

    public string name;

    public Channel(string name){
        this.name = name;
        users = new List<User>();
        messages = new List<string>();
    }
}
