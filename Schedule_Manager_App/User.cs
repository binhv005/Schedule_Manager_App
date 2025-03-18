using System;
using System.Security.Cryptography;
using System.Text;

public class User
{
    private static int userCount = 0; // Biến đếm số lượng user
    private int userId;
    private string userName;
    private string passwordHash = ""; // Gán giá trị mặc định để tránh lỗi null

    public delegate void UserEventHandler(User user);
    public event UserEventHandler OnUserRegistered;
    public event UserEventHandler OnUserLoggedIn;
    public event UserEventHandler OnPasswordChanged;

    public User(string userName, string password)
    {
        this.userId = ++userCount; // Tăng biến đếm mỗi khi tạo user mới
        this.userName = userName;
        SetPassword(password);

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

    private void SetPassword(string password)
    {
        passwordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        SHA256 sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    public bool Authenticate(string password)
    {
        return passwordHash == HashPassword(password);
    }

    public bool ChangePassword(string oldPassword, string newPassword)
    {
        if (!Authenticate(oldPassword))
        {
            Console.WriteLine("❌ Mật khẩu cũ không đúng!");
            return false;
        }

        SetPassword(newPassword);
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