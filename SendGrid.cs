using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Doctrack.SendGrid
{
  internal class EmailService
  {
    public async Task Execute(string receipient)
    {
      var apiKey = Environment.GetEnvironmentVariable("SendGridAPIKey", EnvironmentVariableTarget.User);
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress("chakrit.artamoz@gmail.com");
      var subject = "Sending with SendGrid is Fun";
      var to = new EmailAddress(receipient);
      var plainTextContent = "and easy to do anywhere, even with C#";
      var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
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