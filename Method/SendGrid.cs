using SendGrid;
using SendGrid.Helpers.Mail;
using Doctrack.Method;

namespace Doctrack.SendGrid
{
  public class EmailService
  {
    public static async Task SendVerificationEmailAsync(string receipient)
    {
      var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey", EnvironmentVariableTarget.User);
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress("chakrit.artamoz@gmail.com");
      var to = new EmailAddress(receipient);
      var token = VerificationTokenGenerator.GenerateEmailVerificationToken();
      var callbackUrl = $"http://localhost:5192/accounts/verifyEmail?token={token}";
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
  }
}