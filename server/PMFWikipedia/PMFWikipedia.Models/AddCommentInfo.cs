namespace PMFWikipedia.Models
{
    public class AddCommentInfo
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}