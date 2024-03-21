using Microsoft.AspNetCore.Http;

namespace PMFWikipedia.Models
{
    public class RegisterInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }   
        public string Password { get; set; }
        public string Program { get; set; }
    }
}