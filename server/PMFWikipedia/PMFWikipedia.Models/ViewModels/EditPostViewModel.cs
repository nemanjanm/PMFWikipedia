namespace PMFWikipedia.Models.ViewModels
{
    public class EditPostViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string EditorName { get; set; }
        public long EditorId { get; set; }
        public string Time{ get; set; }
        public DateTime DateCreated { get; set; }
    }
}