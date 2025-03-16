using System;
using System.Security.Cryptography;
using System.Text;

public class User
{
    public int ID { get; private set; }
    public string Username { get; private set; }
    private string PasswordHash;

    public User(int id, string username, string password)
    {
        ID = id;
        Username = username;
        PasswordHash = HashPassword(password);
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public bool Authenticate(string password)
    {
        return PasswordHash == HashPassword(password);
    }
}
