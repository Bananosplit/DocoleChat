public class Channel{
    public List<User> users;
    public List<string> messages;

    public string name;
    public string topic;

    public Channel(string name){
        this.name = name;
        topic = "Welcome!";
        users = new List<User>();
        messages = new List<string>();
    }

    public void AddUser(User user){
        users.Add(user);
    }

    public void RemoveUser(User user){
        users.Remove(user);
    }

    public void AddMessage(string mes){
        messages.Add(mes);
        foreach(var user in users){
            user.AddMessage(mes);
        }
    }

    public bool Contains(User user){
        return users.Contains(user);
    }
}
