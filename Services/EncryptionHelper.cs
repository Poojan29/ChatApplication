using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private static readonly string Key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
    private static readonly string IV = Environment.GetEnvironmentVariable("ENCRYPTION_IV");

    public static string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(IV);

        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using (StreamWriter writer = new(cs))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string encryptedText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(IV);

        byte[] buffer = Convert.FromBase64String(encryptedText);

        using MemoryStream ms = new(buffer);
        using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader reader = new(cs);
        return reader.ReadToEnd();
    }
}