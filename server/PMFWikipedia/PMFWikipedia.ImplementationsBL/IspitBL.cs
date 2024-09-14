using PMFWikipedia.Common.StorageService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
using System.Collections.Generic;

namespace PMFWikipedia.ImplementationsBL
{
    public class IspitBL : IIspitBL
    {
        private readonly IStorageService _storageService;
        private readonly IUserDAL _userDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IIspitDAL _ispitDAL;
        private readonly IIspitResenjeDAL _ispitResenjeDAL;
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly INotificationDAL _notificationDAL;
        private readonly IJWTService _jWTService;
        public IspitBL(IStorageService storageService, IUserDAL userDAL, ISubjectDAL subjectDAL, IIspitDAL ispitDAL, IIspitResenjeDAL ispitResenjeDAL, IFavoriteSubjectDAL favoriteSubjectDAL, INotificationDAL notificationDAL, IJWTService jWTService)
        {
            _storageService = storageService;
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _ispitDAL = ispitDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _notificationDAL = notificationDAL;
            _jWTService = jWTService;
        }

        public async Task<ActionResultResponse<long>> AddIspit(KolokvijumModel ispit)
        {
            Ispit i = new Ispit();

            var author = await _userDAL.GetById(ispit.AuthorId);
            if (author == null)
                return new ActionResultResponse<long>(0, false, "Došlo je do greške");

            var subject = await _subjectDAL.GetById(ispit.SubjectId);
            if (subject == null)
                return new ActionResultResponse<long>(0, false, "Došlo je do greške");

            var isp = await _ispitDAL.GetByTitle(ispit.Title);
            if (isp != null)
                return new ActionResultResponse<long>(0, false, "Ispit već postoji");

            i.AuthorId = ispit.AuthorId;
            i.SubjectId = ispit.SubjectId;
            i.Title = ispit.Title;
            i.FilePath = "";
            i.Year = ispit.Year;
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

            var favoriteSubjects = await _favoriteSubjectDAL.GetAllByFilter(x => x.SubjectId == ispit.SubjectId && x.UserId != ispit.AuthorId && x.IsDeleted == false);

            foreach (var s in favoriteSubjects)
            {
                Notification n = new Notification();
                n.Author = ispit.AuthorId;
                n.Post = i.Id;
                n.Subject = ispit.SubjectId;
                n.Receiver = s.UserId;
                n.NotificationId = 2;
                await _notificationDAL.Insert(n);
                await _notificationDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<long>(i.Id, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeleteIspit(long id)
        {
            var ispit = await _ispitDAL.GetById(id);
            if(ispit == null)
                return new ActionResultResponse<bool>(false, false, "");

            await _ispitDAL.Delete(id);
            await _ispitDAL.SaveChangesAsync();

            var notification = await _notificationDAL.GetAllByFilter(x => x.NotificationId == 2 && x.Post == id);

            foreach (var nott in notification)
            {
                await _notificationDAL.Delete(nott.Id);
                await _notificationDAL.SaveChangesAsync();
            }

            var resenja = await _ispitResenjeDAL.GetAllByFilter(x => x.IspitId == id);
            foreach (var resenje in resenja)
            {
                await _ispitResenjeDAL.Delete(resenje.Id);
                await _ispitResenjeDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<bool>(true, true, "");
        }

        public async Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllIspit(long subjectId)
        {
            List<KolokvijumViewModel> lista = new List<KolokvijumViewModel>();
            var ispiti = await _ispitDAL.GetAllWithAuthor(subjectId);

            bool allowed = true;
            var id = long.Parse(_jWTService.GetUserId());
            var favoriteSubject = await _favoriteSubjectDAL.GetByUser(id, subjectId);
            if (favoriteSubject == null)
                allowed = false;

            if (ispiti.Count == 0)
            {
                KolokvijumViewModel pvm = new KolokvijumViewModel();
                pvm.Allowed = allowed;
                lista.Add(pvm);
            }
            foreach (var ispit in ispiti)
            {
                KolokvijumViewModel k = new KolokvijumViewModel();
                k.Title = ispit.Title;
                k.AuthorId = ispit.AuthorId;
                k.FilePath = ispit.FilePath;
                k.Id = ispit.Id;
                k.AuthorName = ispit.Author.FirstName + " " + ispit.Author.LastName;
                k.Allowed = allowed;
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