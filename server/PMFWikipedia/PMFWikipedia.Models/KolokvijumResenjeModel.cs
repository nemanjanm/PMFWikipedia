using Microsoft.AspNetCore.Http;

namespace PMFWikipedia.Models
{
    public class KolokvijumResenjeModel
    {
        public long KolokvijumId { get; set; }
        public IFormFile File { get; set; }
        public long AuthorId { get; set; }
        public long SubjectId { get; set; }
    }
}