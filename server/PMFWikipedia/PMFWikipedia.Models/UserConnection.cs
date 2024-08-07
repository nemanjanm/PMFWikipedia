namespace PMFWikipedia.Models
{
    public class UserConnection
    {
        public string Username { get; set; } = string.Empty;
        public string ChatRoom { get; set; } = string.Empty;
        public long MyId { get; set; }
        public long SecondId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}