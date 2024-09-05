namespace PMFWikipedia.Models.ViewModels
{
    public class NotificationViewModel
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public long PostId { get; set; }
        public string SubjectName { get; set; }
        public bool IsRead { get; set; }
    }
}