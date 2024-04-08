namespace PMFWikipedia.Models
{
    public class ResetPasswordInfo
    {
        public string token { get; set; }
        public string password { get; set; }
        public string repeatPassword { get; set; }
    }
}