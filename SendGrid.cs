using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Doctrack
{
  internal class EmailService
  {
    private static void Main()
    {
      Execute().Wait();
    }

    static async Task Execute()
    {
      var apiKey = Environment.GetEnvironmentVariable("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");
      var client = new SendGridClient(apiKey);
      var from = new EmailAddress("chakrit.artamoz@gmail.com", "CHAKRIT");
      var subject = "Sending with SendGrid is Fun";
      var to = new EmailAddress("prototype.vf@gmail.com", "PROTOTYPE");
      var plainTextContent = "and easy to do anywhere, even with C#";
      var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);
    }
  }
}