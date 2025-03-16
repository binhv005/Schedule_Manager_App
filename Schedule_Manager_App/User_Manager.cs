using System;
using System.Collections.Generic;

public class UserManager
{
    private static UserManager instance;
    private Dictionary<string, User> users;

    private UserManager()
    {
        users = new Dictionary<string, User>();
    }

    public static UserManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UserManager();
            }
            return instance;
        }
    }

    public bool Register(string username, string password)
    {
        if (users.ContainsKey(username))
        {
            Console.WriteLine("Tên đăng nhập đã tồn tại.");
            return false;
        }

        User newUser = new User(users.Count + 1, username, password);
        users[username] = newUser;
        Console.WriteLine("Đăng ký thành công!");
        return true;
    }

    public User Login(string username, string password)
    {
        if (users.ContainsKey(username) && users[username].Authenticate(password))
        {
            return users[username];
        }

        Console.WriteLine("Sai tên đăng nhập hoặc mật khẩu.");
        return null;
    }
}
