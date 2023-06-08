using SendGrid;
using SendGrid.Helpers.Mail;

namespace Doctrack.SendGrid
{
  public class EmailService
  {
    public static async Task SendVerificationEmailAsync(string receipient, string token)
    {
      // var apiKey = "";
      var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey", EnvironmentVariableTarget.User);
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress("chakrit.artamoz@gmail.com");
      var to = new EmailAddress(receipient);
      var callbackUrl = $"https://sangha-doctrack.azurewebsites.net/accounts/verifyEmail?token={token}";
      var subject = "Verify your email address";
      var plainTextContent = $"Please click on the following link to verify your email address: {callbackUrl}";
      var htmlContent = $"Please click on the following link to verify your email address: <a href='{callbackUrl}'>{callbackUrl}</a>";
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);

      if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        Console.WriteLine("Email sent successfully.");
      }
      else
      {
        Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
      }
    }

    public static async Task SendConfirmResetAsync(string username, string receipient, string token)
    {
      // var apiKey = "";
      var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey", EnvironmentVariableTarget.User);
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress("chakrit.artamoz@gmail.com");
      var to = new EmailAddress(receipient);
      var callbackUrl = $"https://sangha-doctrack.azurewebsites.net/accounts/ResetPassword?username={username}&token={token}";
      // var callbackUrl = $"http://localhost:5192/accounts/ResetPassword?username={username}&token={token}";
      var subject = "Reset Doctrack Password";
      var plainTextContent = $"Please click on the following link to reset your password: {callbackUrl}";
      var htmlContent = $"Please click on the following link to reset your password: <a href='{callbackUrl}'>{callbackUrl}</a>";
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);

      if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        Console.WriteLine("Email sent successfully.");
      }
      else
      {
        Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
      }
    }
  }
}