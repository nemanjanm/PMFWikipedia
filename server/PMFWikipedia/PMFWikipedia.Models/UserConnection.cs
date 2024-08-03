namespace PMFWikipedia.Models
{
    public class UserConnection
    {
        public string Username { get; set; } = string.Empty;
        public string ChatRoom { get; set; } = string.Empty;
        public int MyId { get; set; }
        public string SecondId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}