namespace PMFWikipedia.Models.ViewModels
{
    public class KolokvijumViewModel
    {
        public string Title { get; set; }
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string FilePath { get; set; }
        public bool Allowed { get; set; }
        public List<KolokvijumResenjeViewModel> Resenja { get; set; } = new List<KolokvijumResenjeViewModel>();
    }
}