namespace PMFWikipedia.Models.ViewModels
{
    public class KolokvijumResenjeViewModel
    {
        public long Id { get; set; }
        public long KolokvijumId { get; set; }
        public string FilePath { get; set; }
        public long AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}