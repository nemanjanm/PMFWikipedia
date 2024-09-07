using PMFWikipedia.Common.StorageService;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.ImplementationsBL
{
    public class IspitBL : IIspitBL
    {
        private readonly IStorageService _storageService;
        private readonly IUserDAL _userDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IIspitDAL _ispitDAL;
        private readonly IIspitResenjeDAL _ispitResenjeDAL;

        public IspitBL(IStorageService storageService, IUserDAL userDAL, ISubjectDAL subjectDAL, IIspitDAL ispitDAL, IIspitResenjeDAL ispitResenjeDAL)
        {
            _storageService = storageService;
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _ispitDAL = ispitDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
        }

        public async Task<ActionResultResponse<bool>> AddIspit(KolokvijumModel ispit)
        {
            Ispit i = new Ispit();

            var author = await _userDAL.GetById(ispit.AuthorId);
            if (author == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            var subject = await _subjectDAL.GetById(ispit.SubjectId);
            if (subject == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            i.AuthorId = ispit.AuthorId;
            i.SubjectId = ispit.SubjectId;
            i.Title = ispit.Title;
            i.FilePath = "";
            await _ispitDAL.Insert(i);
            await _ispitDAL.SaveChangesAsync();

            string path = _storageService.CreateFilePath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string extension = Path.GetExtension(ispit.File.FileName);
            string fileName = "ispit" + i.Id + extension;
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            i.FilePath = Path.Combine("Files", fileName);
            await _ispitDAL.Update(i);
            await _ispitDAL.SaveChangesAsync();

            using (FileStream stream = System.IO.File.Create(path))
            {
                ispit.File.CopyTo(stream);
                stream.Flush();
            }

            return new ActionResultResponse<bool>(true, true, "");
        }

        public async Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllIspit(long subjectId)
        {
            List<KolokvijumViewModel> lista = new List<KolokvijumViewModel>();
            var ispiti = await _ispitDAL.GetAllWithAuthor(subjectId);

            foreach (var ispit in ispiti)
            {
                KolokvijumViewModel k = new KolokvijumViewModel();
                k.Title = ispit.Title;
                k.AuthorId = ispit.AuthorId;
                k.FilePath = ispit.FilePath;
                k.Id = ispit.Id;
                k.AuthorName = ispit.Author.FirstName + " " + ispit.Author.LastName;

                var resenja = await _ispitResenjeDAL.GetAllWithAuthor(ispit.Id);
                foreach (var resenje in resenja)
                {
                    KolokvijumResenjeViewModel krvm = new KolokvijumResenjeViewModel();
                    krvm.AuthorId = resenje.AuthorId;
                    krvm.Id = resenje.Id;
                    krvm.AuthorName = resenje.Author.FirstName + " " + resenje.Author.LastName;
                    krvm.FilePath = resenje.FilePath;
                    krvm.KolokvijumId = resenje.IspitId;

                    k.Resenja.Add(krvm);
                }

                lista.Add(k);
            }
            return new ActionResultResponse<List<KolokvijumViewModel>>(lista, true, "");
        }
    }
}