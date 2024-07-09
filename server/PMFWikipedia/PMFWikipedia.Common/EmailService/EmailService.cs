using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace PMFWikipedia.Common.EmailService
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string reciever, string subject, string body, string title)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(ConfigProvider.Email));
            mail.To.Add(MailboxAddress.Parse(reciever));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = GetTemplate(body, title) };
            SmtpClient smtp = new SmtpClient();

            await smtp.ConnectAsync(ConfigProvider.EmailHost, int.Parse(ConfigProvider.Port));
            await smtp.AuthenticateAsync(ConfigProvider.Email, ConfigProvider.Password);
            
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }

        public string GetTemplate(string body, string title)
        {
            return @"<!DOCTYPE html>" +
                "<html>" +
                "<head>  " +
                "<meta charset=\"utf-8\">" +
                "<title>" + title + "</title>" +
                "</head>" +
                "<body style=\"margin: 100px; padding: 0; font-family: Arial, sans-serif; text-align: center;\">  " +
                "<div style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff;  padding: 40px;  border-radius: 10px;  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\">" +
                body +
                "<footer> &copy; <em id=\"date\"></em>PMFWikipedia</footer>" +
                "</div>" +
                "</body>" +
                "</html>";
        }

        public string GetInitTemplate(string title, string token, string url)
        {
            string body = "" +
                "<h1 style=\"color: #333333;  margin-bottom: 20px;\">"+title+"</h1>" +
                " <p style=\"color: #333333;  line-height: 1.5; margin-bottom: 20px;\">Click the button below to take action:</p> " +
                "<a href=\"" + url + token + "\" style=\"background-color: #4CAF50; display: inline-block; color: #ffffff; " +
                "padding: 10px 20px;text-decoration: none;border-radius: 5px;\">Click</a>";

            return body;
        }
    }
}
