using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public int Program { get; set; }
        public string PhotoPath { get; set; }
        public string FullName { get; set; }
        public string ConnectionId { get; set; }
        public int Ispits { get; set; }
        public int Kolokvijums { get; set; }
        public List<FavoriteSubjectViewModel> FavoriteSubjects { get; set; } = new List<FavoriteSubjectViewModel>();
    }
}