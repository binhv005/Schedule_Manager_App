using System;
using System.Collections.Generic;

class UserManager
{
    private static UserManager instance;
    private Dictionary<int, User> users;

    private UserManager()
    {
        this.users = new Dictionary<int, User>();
    }

    public static UserManager GetInstance()
    {
        if (instance == null)
        {
            instance = new UserManager();
        }
        return instance;
    }

    // ✅ Đăng ký người dùng
    public bool Register(string userName, string password)
    {
        foreach (KeyValuePair<int, User> entry in users)
        {
            if (entry.Value.GetUserName() == userName)
            {
                Console.WriteLine("❌ Tên người dùng đã tồn tại!");
                return false;
            }
        }

        User newUser = new User(userName, password);
        users.Add(newUser.GetUserId(), newUser);
        Console.WriteLine("✅ Đăng ký thành công!");
        return true;
    }

    // ✅ Xác thực đăng nhập
    public User? Authenticate(string userName, string password)
    {
        foreach (KeyValuePair<int, User> entry in users)
        {
            if (entry.Value.GetUserName() == userName && entry.Value.Authenticate(password))
            {
                Console.WriteLine("✅ Đăng nhập thành công!");
                return entry.Value;
            }
        }
        Console.WriteLine("❌ Sai tên người dùng hoặc mật khẩu!");
        return null;
    }

    // ✅ Hiển thị danh sách người dùng
    public void DisplayUsers()
    {
        Console.WriteLine("\n📌 Danh sách người dùng:");
        foreach (KeyValuePair<int, User> entry in users)
        {
            Console.WriteLine($"🆔 ID: {entry.Key} | 👤 {entry.Value.GetUserName()}");
        }
    }
}
