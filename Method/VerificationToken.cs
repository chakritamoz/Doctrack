
namespace Doctrack.Method
{
  public class VerificationTokenGenerator
  {
    public static string GenerateEmailVerificationToken(out byte[] key)
    {
      byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
      key = Guid.NewGuid().ToByteArray();
      string token = Convert.ToBase64String(time.Concat(key).ToArray());
      Console.WriteLine($"gen key: {key}");
      return token;
    }

    public static bool ValidateToken(string token, string userKey)
    {
      byte[] data = Convert.FromBase64String(token);
      DateTime time = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
      Guid key = new Guid(data.Skip(8).ToArray());
      Console.WriteLine($"key: {key}");
      if (DateTime.UtcNow > time.AddHours(24))
      {
        return false;
      }

      if (key != new Guid(userKey))
      {
        return false;
      }

      return true;
    }
  }
}