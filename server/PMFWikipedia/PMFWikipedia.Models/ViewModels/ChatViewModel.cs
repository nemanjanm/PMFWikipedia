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
        public long User1Id { get; set; }
        public long User2Id { get; set; }
        public UserViewModel? User { get; set; }
    }
}