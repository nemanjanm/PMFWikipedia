namespace PMFWikipedia.Models.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Program {  get; set; }
        public string PhotoPath { get; set; }
        public string? FullName { get; set; }
        public string ConnectionId { get; set; }
    }
}
