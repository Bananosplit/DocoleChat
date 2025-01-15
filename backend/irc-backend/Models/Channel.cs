public class Channel{
    public List<User> users;
    public List<string> messages;

    public string name;

    public Channel(string name){
        this.name = name;
        users = new List<User>();
        messages = new List<string>();
    }

    public void AddUser(User user){
        users.Add(user);
        foreach(var mes in messages){
            user.AddMessage(mes);
        }
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
