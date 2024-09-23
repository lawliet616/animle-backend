using Animle.interfaces;
using MailKit.Net.Smtp;
using MimeKit;

public static class EmailService
{
    public static Boolean SendEmail(EmailDto emailDto)
    {
        var configuration = new ConfigurationBuilder()
         .AddJsonFile("appsettings.json")
          .Build();

        string email = configuration.GetSection("AppSettings:Email").Value;

        string emailPass = configuration.GetSection("AppSettings:EmailPassword").Value;

        string gSercret = configuration.GetSection("AppSettings:GmailSecret").Value;


        var emailMessage = new MimeMessage();

    

        if(emailDto.From != null)
        {
            emailMessage.From.Add(new MailboxAddress(emailDto.From, emailDto.Email));
        }
        emailMessage.To.Add(new MailboxAddress("", emailDto.To));
        emailMessage.Subject = emailDto.Subject;
        emailMessage.Body = new TextPart("plain") { Text = emailDto.Body };
        try
        {

        using (var client = new SmtpClient())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate(email, gSercret);

            client.Send(emailMessage);
            client.Disconnect(true);
            return true;
        }

        } catch (Exception ex)
        {
            return false;
        }
    }
 
    
}