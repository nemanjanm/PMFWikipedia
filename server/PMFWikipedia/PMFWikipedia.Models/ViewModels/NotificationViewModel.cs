namespace PMFWikipedia.Models.ViewModels
{
    public class NotificationViewModel
    {
        public long Id { get; set; }
        public string AuthorName { get; set; }
        public long PostId { get; set; }
        public string SubjectName { get; set; }
        public long SubjectId { get; set; }
        public bool IsRead { get; set; }
        public int NotificationId { get; set; }
        public DateTime TimeStamp { get; set; } 
    }
}