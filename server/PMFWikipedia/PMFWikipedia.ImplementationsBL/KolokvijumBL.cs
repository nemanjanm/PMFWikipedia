using PMFWikipedia.Common.StorageService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class KolokvijumBL : IKolokvijumBL
    {
        private readonly IJWTService _jwtService;
        private readonly IKolokvijumDAL _kolokvijumDAL;
        private readonly IStorageService _storageService;
        private readonly IUserDAL _userDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IKolokvijumResenjeDAL _kolokvijumResenjeDAL;
        public KolokvijumBL(IJWTService jWTService, IKolokvijumDAL kolokvijumDAL, IStorageService storageService, IUserDAL userDAL, ISubjectDAL subjectDAL, IKolokvijumResenjeDAL kolokvijumResenjeDAL)
        {
            _jwtService = jWTService;
            _kolokvijumDAL = kolokvijumDAL;
            _storageService = storageService;
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _kolokvijumResenjeDAL = kolokvijumResenjeDAL;
        }

        public async Task<ActionResultResponse<bool>> AddKolokvijum(KolokvijumModel kolokvijum)
        {
            Kolokvijum k = new Kolokvijum();

            var author = await _userDAL.GetById(kolokvijum.AuthorId);
            if(author==null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            var subject = await _subjectDAL.GetById(kolokvijum.SubjectId);
            if (subject == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            k.AuthorId = kolokvijum.AuthorId;
            k.SubjectId = kolokvijum.SubjectId;
            k.Title = kolokvijum.Title;
            k.FilePath = "";
            await _kolokvijumDAL.Insert(k);
            await _kolokvijumDAL.SaveChangesAsync();

            string path = _storageService.CreateFilePath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string extension = Path.GetExtension(kolokvijum.File.FileName);
            string fileName = "kolokvijum" + k.Id + extension;
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            k.FilePath = Path.Combine("Files", fileName);
            await _kolokvijumDAL.Update(k);
            await _kolokvijumDAL.SaveChangesAsync();

            using (FileStream stream = System.IO.File.Create(path))
            {
                kolokvijum.File.CopyTo(stream);
                stream.Flush();
            }

            return new ActionResultResponse<bool>(true, true, "");
        }

        public async Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllKolokvijum(long subjectId)
        {
            List<KolokvijumViewModel> lista = new List<KolokvijumViewModel>();

            var kolokvijumi = await _kolokvijumDAL.GetAllWithAuthor(subjectId);

            foreach (var kolokvijum in kolokvijumi) 
            {
                KolokvijumViewModel k = new KolokvijumViewModel();
                k.Title = kolokvijum.Title;
                k.AuthorId = kolokvijum.AuthorId;
                k.FilePath = kolokvijum.FilePath;
                k.Id = kolokvijum.Id;
                k.AuthorName = kolokvijum.Author.FirstName + " " + kolokvijum.Author.LastName;

                var resenja = await _kolokvijumResenjeDAL.GetAllWithAuthor(kolokvijum.Id);
                foreach(var resenje in resenja)
                {
                    KolokvijumResenjeViewModel krvm = new KolokvijumResenjeViewModel();
                    krvm.AuthorId = resenje.AuthorId;
                    krvm.Id = resenje.Id;
                    krvm.AuthorName = resenje.Author.FirstName + " " + resenje.Author.LastName;
                    krvm.FilePath = resenje.FilePath;
                    krvm.KolokvijumId = resenje.KolokvijumId;

                    k.Resenja.Add(krvm);
                }

                lista.Add(k);
            }

            return new ActionResultResponse<List<KolokvijumViewModel>>(lista, true, "");
        }
    }
}