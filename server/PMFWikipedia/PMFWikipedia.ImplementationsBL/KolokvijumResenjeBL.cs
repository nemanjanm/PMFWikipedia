using PMFWikipedia.Common.StorageService;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsBL
{
    public class KolokvijumResenjeBL : IKolokvijumResenjeBL
    {
        private readonly IKolokvijumResenjeDAL _kolokvijumResenjeDAL;
        private readonly IUserDAL _userDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IStorageService _storageService;
        private readonly IKolokvijumDAL _kolokvijumDAL;
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly INotificationDAL _notificationDAL;

        public KolokvijumResenjeBL(IKolokvijumResenjeDAL kolokvijumResenjeDAL, IUserDAL userDAL, ISubjectDAL subjectDAL, IStorageService storageService, IKolokvijumDAL kolokvijumDAL, IFavoriteSubjectDAL favoriteSubjectDAL, INotificationDAL notificationDAL)
        {
            _kolokvijumResenjeDAL = kolokvijumResenjeDAL;
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _storageService = storageService;
            _kolokvijumDAL = kolokvijumDAL;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _notificationDAL = notificationDAL;
        }

        public async Task<ActionResultResponse<long>> AddKolokvijumResenje(KolokvijumResenjeModel resenje)
        {
            KolokvijumResenje kr = new KolokvijumResenje();

            var author = await _userDAL.GetById(resenje.AuthorId);
            if (author == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

            var subject = await _subjectDAL.GetById(resenje.SubjectId);
            if (subject == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

            var klk = await _kolokvijumDAL.GetById(resenje.KolokvijumId);
            if (klk == null)
                return new ActionResultResponse<long>(0, false, "Something went wrong");

            kr.AuthorId = resenje.AuthorId;
            kr.SubjectId = resenje.SubjectId;
            kr.FilePath = "";
            kr.KolokvijumId = resenje.KolokvijumId;
            await _kolokvijumResenjeDAL.Insert(kr);
            await _kolokvijumResenjeDAL.SaveChangesAsync();

            string path = _storageService.CreateFilePath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string extension = Path.GetExtension(resenje.File.FileName);
            string fileName = "kolokvijumResenje" + kr.Id + extension;
            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            kr.FilePath = Path.Combine("Files", fileName);
            await _kolokvijumResenjeDAL.Update(kr);
            await _kolokvijumResenjeDAL.SaveChangesAsync();

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
                n.Post = kr.Id;
                n.Subject = resenje.SubjectId;
                n.Receiver = s.UserId;
                n.NotificationId = 7;
                await _notificationDAL.Insert(n);
                await _notificationDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<long>(kr.Id, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeleteResenje(long id)
        {
            var resenje = await _kolokvijumResenjeDAL.GetById(id);
            if(resenje == null)
                return new ActionResultResponse<bool>(false, false, "");

            await _kolokvijumResenjeDAL.Delete(id);
            await _kolokvijumResenjeDAL.SaveChangesAsync();

            var nott = await _notificationDAL.GetAllKolokvijumResenjeNotification(id);
            foreach (var n in nott) 
            {
                await _notificationDAL.Delete(n.Id);
                await _notificationDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<bool>(true, true, "");
        }
    }
}