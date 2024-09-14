using Microsoft.AspNetCore.Http;

namespace PMFWikipedia.Models
{
    public class KolokvijumModel
    {
        public string Year { get; set; }
        public string Title { get; set; }
        public IFormFile File {  get; set; }
        public long AuthorId { get; set; }
        public long SubjectId { get; set; }
    }
}