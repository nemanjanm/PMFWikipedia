namespace PMFWikipedia.Models.ViewModels
{
    public class NotificationViewModel
    {
        public string AuthorName { get; set; }
        public long PostId { get; set; }
        public string SubjectName { get; set; }
        public bool IsRead { get; set; }
    }
}