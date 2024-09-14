namespace PMFWikipedia.Models.ViewModels
{
    public class PostViewModel
    {
        public string AuthorName { get; set; } = string.Empty;
        public long PostId { get; set; }
        public long AuthorId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public long SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public bool Allowed { get; set; }
        public string EditorName { get; set; }
        public long EditorId { get; set; }
        public List<EditPostViewModel> editHistory {get; set;} = new List<EditPostViewModel>();
        public DateTime TimeStamp { get; set; }
        public DateTime TimeEdited { get; set; }
    }
}