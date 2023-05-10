
namespace Doctrack.Method
{
  public class VerificationTokenGenerator
  {
    public static string GenerateEmailVerificationToken(out byte[] keyToken)
    {
      byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
      keyToken = Guid.NewGuid().ToByteArray();
      string token = Convert.ToBase64String(time.Concat(keyToken).ToArray());
      return token;
    }

    public static bool ValidateToken(string token, byte[] userKey)
    {
      byte[] data = Convert.FromBase64String(token);
      DateTime time = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
      Guid key = new Guid(data.Skip(8).ToArray());
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