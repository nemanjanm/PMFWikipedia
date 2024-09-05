namespace PMFWikipedia.Models.ViewModels
{
    public class SubjectViewModel
    {
        public long Id { get; set; }
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Semester {  get; set; }
        public bool Allowed { get; set; }
    }
}