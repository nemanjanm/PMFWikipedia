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
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly INotificationDAL _notificationDAL;

        public IspitResenjeBL(IUserDAL userDAL, ISubjectDAL subjectDAL, IStorageService storageService, IIspitDAL ispitDAL, IIspitResenjeDAL ispitResenjeDAL, IFavoriteSubjectDAL favoriteSubjectDAL, INotificationDAL notificationDAL)
        {
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _storageService = storageService;
            _ispitDAL = ispitDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _notificationDAL = notificationDAL;
        }

        public async Task<ActionResultResponse<long>> AddIspitResenje(KolokvijumResenjeModel resenje)
        {
            IspitResenje ir = new IspitResenje();

            var author = await _userDAL.GetById(resenje.AuthorId);
            if (author == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

            var subject = await _subjectDAL.GetById(resenje.SubjectId);
            if (subject == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

            var klk = await _ispitDAL.GetById(resenje.KolokvijumId);
            if (klk == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

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

            var favoriteSubjects = await _favoriteSubjectDAL.GetAllByFilter(x => x.SubjectId == resenje.SubjectId && x.UserId != resenje.AuthorId && x.IsDeleted == false);

            foreach (var s in favoriteSubjects)
            {
                Notification n = new Notification();
                n.Author = resenje.AuthorId;
                n.Post = ir.Id;
                n.Subject = resenje.SubjectId;
                n.Receiver = s.UserId;
                n.NotificationId = 8;
                await _notificationDAL.Insert(n);
                await _notificationDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<long>(ir.Id, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeleteResenje(long id)
        {
            var resenje = await _ispitResenjeDAL.GetById(id);
            if (resenje == null)
                return new ActionResultResponse<bool>(false, false, "");

            await _ispitResenjeDAL.Delete(id);
            await _ispitResenjeDAL.SaveChangesAsync();

            var nott = await _notificationDAL.GetAllIspitResenjeNotification(id);
            foreach (var n in nott)
            {
                await _notificationDAL.Delete(n.Id);
                await _notificationDAL.SaveChangesAsync();
            }
            return new ActionResultResponse<bool>(true, true, "");
        }
    }
}