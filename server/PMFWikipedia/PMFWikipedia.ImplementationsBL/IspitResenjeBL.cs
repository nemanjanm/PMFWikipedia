using PMFWikipedia.Common.StorageService;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsBL
{
    public class IspitResenjeBL : IIspitResenjeBL
    {
        private readonly IUserDAL _userDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IStorageService _storageService;
        private readonly IIspitDAL _ispitDAL;
        private readonly IIspitResenjeDAL _ispitResenjeDAL;

        public IspitResenjeBL(IUserDAL userDAL, ISubjectDAL subjectDAL, IStorageService storageService, IIspitDAL ispitDAL, IIspitResenjeDAL ispitResenjeDAL)
        {
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _storageService = storageService;
            _ispitDAL = ispitDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
        }

        public async Task<ActionResultResponse<bool>> AddIspitResenje(KolokvijumResenjeModel resenje)
        {
            IspitResenje ir = new IspitResenje();

            var author = await _userDAL.GetById(resenje.AuthorId);
            if (author == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            var subject = await _subjectDAL.GetById(resenje.SubjectId);
            if (subject == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            var klk = await _ispitDAL.GetById(resenje.KolokvijumId);
            if (klk == null)
                return new ActionResultResponse<bool>(false, false, "Something went wrong");

            ir.AuthorId = resenje.AuthorId;
            ir.SubjectId = resenje.SubjectId;
            ir.FilePath = "";
            ir.IspitId = resenje.KolokvijumId;
            await _ispitResenjeDAL.Insert(ir);
            await _ispitResenjeDAL.SaveChangesAsync();

            string path = _storageService.CreateFilePath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string extension = Path.GetExtension(resenje.File.FileName);
            string fileName = "ispitResenje" + ir.Id + extension;
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            ir.FilePath = Path.Combine("Files", fileName);
            await _ispitResenjeDAL.Update(ir);
            await _ispitResenjeDAL.SaveChangesAsync();

            using (FileStream stream = System.IO.File.Create(path))
            {
                resenje.File.CopyTo(stream);
                stream.Flush();
            }
            return new ActionResultResponse<bool>(true, true, "");
        }
    }
}