using System.Net;
using System.Net.Mail;

namespace Demo.Presentation.Utilities;

public static class MailSettings
{
    public static void SendEmail(Email email)
    {
        var client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("mohammedgamal.route@gmail.com"
            , "dggzhucafzvyvutj"); // Sender Email 

        client.Send("mohammedgamal.route@gmail.com",
           email.Recipient, email.Subject, email.Body);
    }
}
public class Email
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}