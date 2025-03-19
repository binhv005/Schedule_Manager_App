using System;

public class User
{
    private static int userCount = 0; // Biến đếm số lượng user
    private int userId;
    private string userName;
    private string password; // Lưu mật khẩu trực tiếp, không hash

    public delegate void UserEventHandler(User user);
    public event UserEventHandler OnUserRegistered;
    public event UserEventHandler OnUserLoggedIn;
    public event UserEventHandler OnPasswordChanged;

    public User(string userName, string password)
    {
        this.userId = ++userCount; // Tăng biến đếm mỗi khi tạo user mới
        this.userName = userName;
        this.password = password; // Lưu trực tiếp

        OnUserRegistered?.Invoke(this);
    }

    public int GetUserId()
    {
        return userId;
    }

    public string GetUserName()
    {
        return userName;
    }

    public static int GetUserCount()
    {
        return userCount;
    }

    public bool Authenticate(string password)
    {
        return this.password == password;
    }

    public bool ChangePassword(string oldPassword, string newPassword)
    {
        if (!Authenticate(oldPassword))
        {
            Console.WriteLine("❌ Mật khẩu cũ không đúng!");
            return false;
        }

        this.password = newPassword;
        Console.WriteLine("🔐 Mật khẩu đã thay đổi thành công!");

        OnPasswordChanged?.Invoke(this);
        return true;
    }

    public void Login(string password)
    {
        if (Authenticate(password))
        {
            Console.WriteLine("✅ " + userName + " đã đăng nhập thành công!");
            OnUserLoggedIn?.Invoke(this);
        }
        else
        {
            Console.WriteLine("❌ Sai mật khẩu!");
        }
    }
}
