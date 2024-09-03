namespace PMFWikipedia.Models.ViewModels
{
    public class PostViewModel
    {
        public string AuthorName { get; set; } = string.Empty;
        public long PostId { get; set; }
        public long AuthorId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}