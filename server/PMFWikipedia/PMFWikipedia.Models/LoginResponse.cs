using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserViewModel User { get; set; }
        public List<SubjectViewModel> Subjects { get; set; }
    }
}