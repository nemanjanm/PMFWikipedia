namespace PMFWikipedia.Models.ViewModels
{
    public class ChatViewModel
    {
        public long ChatId {  get; set; }
        public long Id {  get; set; }
        public string Content { get; set; } 
        public bool IsRead { get; set; }
        public DateTime TimeStamp { get; set; }
        public long SenderId { get; set; }
    }
}