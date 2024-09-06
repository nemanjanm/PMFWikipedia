namespace PMFWikipedia.Models
{
    public class PostModel
    {
        public long? Id { get; set; }    
        public string Title { get; set; }
        public string Content { get; set; } 
        public long Author { get; set; }
        public long Subject { get; set; }
    }
}