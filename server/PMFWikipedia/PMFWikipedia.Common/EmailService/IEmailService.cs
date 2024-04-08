namespace PMFWikipedia.Common.EmailService
{
    public interface IEmailService
    {
        public Task SendEmail(string reciever, string subject, string body, string title);
        public string GetTemplate (string body, string title);
        public string GetInitTemplate(string title, string token);
    }
}
