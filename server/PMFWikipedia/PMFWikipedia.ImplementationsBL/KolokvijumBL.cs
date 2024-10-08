﻿using PMFWikipedia.Common.StorageService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
using System.Collections.Generic;

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
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly INotificationDAL _notificationDAL;
        public KolokvijumBL(IJWTService jWTService, IKolokvijumDAL kolokvijumDAL, IStorageService storageService, IUserDAL userDAL, ISubjectDAL subjectDAL, IKolokvijumResenjeDAL kolokvijumResenjeDAL, IFavoriteSubjectDAL favoriteSubjectDAL, INotificationDAL notificationDAL)
        {
            _jwtService = jWTService;
            _kolokvijumDAL = kolokvijumDAL;
            _storageService = storageService;
            _userDAL = userDAL;
            _subjectDAL = subjectDAL;
            _kolokvijumResenjeDAL = kolokvijumResenjeDAL;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _notificationDAL = notificationDAL;
        }

        public async Task<ActionResultResponse<long>> AddKolokvijum(KolokvijumModel kolokvijum)
        {
            Kolokvijum k = new Kolokvijum();

            var author = await _userDAL.GetById(kolokvijum.AuthorId);
            if(author==null)
                return new ActionResultResponse<long>(0, false, "Došlo je do greške");

            var subject = await _subjectDAL.GetById(kolokvijum.SubjectId);
            if (subject == null)
                return new ActionResultResponse<long>(0, false, "Došlo je do greške");

            var klk = await _kolokvijumDAL.GetByTitle(kolokvijum.Title);
            if (klk != null)
                return new ActionResultResponse<long>(0, false, "Kolokvijum već postoji");

            k.Year = kolokvijum.Year;
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

            var favoriteSubjects = await _favoriteSubjectDAL.GetAllByFilter(x => x.SubjectId == kolokvijum.SubjectId && x.UserId != kolokvijum.AuthorId && x.IsDeleted == false);

            foreach (var s in favoriteSubjects)
            {
                Notification n = new Notification();
                n.Author = kolokvijum.AuthorId;
                n.Post = k.Id;
                n.Subject = kolokvijum.SubjectId;
                n.Receiver = s.UserId;
                n.NotificationId = 1;
                await _notificationDAL.Insert(n);
                await _notificationDAL.SaveChangesAsync();
            }
            return new ActionResultResponse<long>(k.Id, true, "");
        }

        public async Task<ActionResultResponse<bool>> DeleteKolokvijum(long id)
        {
            var klk = await _kolokvijumDAL.GetById(id);
            if(klk == null)
                return new ActionResultResponse<bool>(false, false, "");

            await _kolokvijumDAL.Delete(id);
            await _kolokvijumDAL.SaveChangesAsync();

            var notification = await _notificationDAL.GetAllByFilter(x => x.NotificationId == 1 && x.Post == id);
            
            foreach(var nott in notification)
            {
                await _notificationDAL.Delete(nott.Id);
                await _notificationDAL.SaveChangesAsync();
            }

            var resenja = await _kolokvijumResenjeDAL.GetAllByFilter(x => x.KolokvijumId == id);

            foreach(var resenje in resenja)
            {
                await _kolokvijumResenjeDAL.Delete(resenje.Id);
                await _kolokvijumResenjeDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<bool>(true, true, "");
        }

        public async Task<ActionResultResponse<List<KolokvijumViewModel>>> GetAllKolokvijum(long subjectId)
        {
            List<KolokvijumViewModel> lista = new List<KolokvijumViewModel>();
            var kolokvijumi = await _kolokvijumDAL.GetAllWithAuthor(subjectId);

            bool allowed = true;
            var id = long.Parse(_jwtService.GetUserId());
            var favoriteSubject = await _favoriteSubjectDAL.GetByUser(id, subjectId);
            if (favoriteSubject == null)
                allowed = false;

            if (kolokvijumi.Count == 0)
            {
                KolokvijumViewModel pvm = new KolokvijumViewModel();
                pvm.Allowed = allowed;
                lista.Add(pvm);
            }
            else
            {
                foreach (var kolokvijum in kolokvijumi)
                {
                    KolokvijumViewModel k = new KolokvijumViewModel();
                    k.Title = kolokvijum.Title;
                    k.AuthorId = kolokvijum.AuthorId;
                    k.FilePath = kolokvijum.FilePath;
                    k.Id = kolokvijum.Id;
                    k.Allowed = allowed;
                    k.AuthorName = kolokvijum.Author.FirstName + " " + kolokvijum.Author.LastName;

                    var resenja = await _kolokvijumResenjeDAL.GetAllWithAuthor(kolokvijum.Id);
                    foreach (var resenje in resenja)
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
            }

            return new ActionResultResponse<List<KolokvijumViewModel>>(lista, true, "");
        }
    }
}