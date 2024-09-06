namespace PMFWikipedia.Models.ViewModels
{
    public class CommentViewModel
    {
        public long Id { get; set; } 
        public string Content { get; set; } = string.Empty;
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
    }
}