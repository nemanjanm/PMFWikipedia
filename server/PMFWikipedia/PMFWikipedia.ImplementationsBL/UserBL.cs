﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using PMFWikipedia.Common;
using PMFWikipedia.Common.EmailService;
using PMFWikipedia.Common.StorageService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;
using System.Text.RegularExpressions;

namespace PMFWikipedia.ImplementationsBL
{
    public class UserBL : IUserBL
    {
        private readonly IJWTService _jwtService;
        public readonly IUserDAL _userDAL;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IStorageService _storageService;
        private readonly IFavoriteSubjectDAL _favoriteSubjectDAL;
        private readonly ISubjectDAL _subjectDAL;
        private readonly IChatDAL _chatDAL;
        private readonly IKolokvijumResenjeDAL _kolokvijumResenjeDAL;
        private readonly IIspitResenjeDAL _ispitResenjeDAL;
        public UserBL(IChatDAL chatDAL, IJWTService jwtService, ISubjectDAL subjectDAL, IFavoriteSubjectDAL favoriteSubjectDAL, IUserDAL userDAL, IMapper mapper, IEmailService emailService, IStorageService storageService, IIspitResenjeDAL ispitResenjeDAL, IKolokvijumResenjeDAL kolokvijumResenjeDAL)
        {
            _userDAL = userDAL;
            _mapper = mapper;
            _emailService = emailService;
            _storageService = storageService;
            _favoriteSubjectDAL = favoriteSubjectDAL;
            _subjectDAL = subjectDAL;
            _jwtService = jwtService;
            _chatDAL = chatDAL;
            _kolokvijumResenjeDAL = kolokvijumResenjeDAL;
            _ispitResenjeDAL = ispitResenjeDAL;
        }

        public async Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo)
        {
            string pattern = @"@pmf\.kg\.ac\.rs$";
            
            if (await _userDAL.CheckEmail(registerInfo.Email))
            {
                return new ActionResultResponse<User>(null, false, "Email Taken");
            }
            else if (!Regex.IsMatch(registerInfo.Email, pattern))
            {
                return new ActionResultResponse<User>(null, false, "Email Invalid");
            }

            User newUser = _mapper.Map<User>(registerInfo);
            newUser.Password = PasswordService.HashPass(registerInfo.Password);
            newUser.Verified = false;

            string token = Guid.NewGuid().ToString();

            string body = _emailService.GetInitTemplate("Register", token, ConfigProvider.ConfirmEmail);

            await _emailService.SendEmail(registerInfo.Email, "Registration", body, "Registration");
            
            newUser.RegisterToken = token;
            newUser.RegisterTokenExpirationTime = DateTime.Now.AddMinutes(ConfigProvider.TokenExpirationTime);

            await _userDAL.Insert(newUser);
            await _userDAL.SaveChangesAsync();

            var photoChangeUser = await _userDAL.GetUserForPhoto(newUser.Email);
            if(photoChangeUser != null)
            {
                photoChangeUser.PhotoPath = _storageService.SetDefaultPhoto(photoChangeUser.Id);
                await _userDAL.SaveChangesAsync();
            }

            return new ActionResultResponse<User>(newUser, true, "");
        }

        public async Task<ActionResultResponse<string>> CreateResetToken(string email)
        {
            var user = await _userDAL.GetUserByEmail(email);
            if (user == null)
            {
                return new ActionResultResponse<string>(null, false, "Email Doesn't exists");
            }

            string token = Guid.NewGuid().ToString();

            string body = _emailService.GetInitTemplate("Change Password", token, ConfigProvider.ChangePasswordPage);

            await _emailService.SendEmail(email, "Change Password", body, "Change Password");

            user.ResetToken = token;
            user.ResetTokenExpirationTime = DateTime.Now.AddMinutes(ConfigProvider.TokenExpirationTime);

            await _userDAL.Update(user);
            await _userDAL.SaveChangesAsync();
            return new ActionResultResponse<string>(null, true, "Check your email");
        }
        public async Task<ActionResultResponse<string>> ValidateUser(string registrationToken)
        {
            var user = await _userDAL.GetUserByToken(registrationToken);
            if (user == null)
            {
                return new ActionResultResponse<string>(null, false, "Token Expired");
            }

            user.Verified = true;
            await _userDAL.Update(user);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<string>("Successfully Verified", true, "");
        }

        public async Task<ActionResultResponse<LoginResponse>> Login(LoginInfo loginInfo)
        {
            if (loginInfo == null) 
            {
                return new ActionResultResponse<LoginResponse>(null, false, "Credentials not sent");
            }

            var user = await _userDAL.GetUserByEmail(loginInfo.Email);
            if (user == null)
            {
                return new ActionResultResponse<LoginResponse>(null, false, "Wrong credentials");
            }

            if(!PasswordService.VerifyPassword(user.Password, loginInfo.Password))
            {
                return new ActionResultResponse<LoginResponse>(null, false, "Wrong credentials");
            }

            LoginResponse loginResponse = new LoginResponse();
            
            loginResponse.User = _mapper.Map<UserViewModel>(user);
            loginResponse.Token = AuthService.GetJWT(loginResponse.User);

            return new ActionResultResponse<LoginResponse>(loginResponse, true, "Successfully login");
        }

        public async Task<ActionResultResponse<User>> ResetPassword(ResetPasswordInfo info)
        {
            var user = await _userDAL.GetUserByResetToken(info.token);
            if(user == null)
            {
                return new ActionResultResponse<User>(null, false, "Token Expired");
            }

            if(!info.password.Equals(info.repeatPassword))
            {
                return new ActionResultResponse<User>(null, false, "Password are not same");
            }

            user.Password = PasswordService.HashPass(info.password);
            await _userDAL.Update(user);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<User>(user, true, "Successfullt changed password");
        }

        public async Task<ActionResultResponse<User>> ChangeConnectionId(long id, string connId)
        {
            var user = await _userDAL.GetById(id);
            if (user == null)
            {
                return new ActionResultResponse<User>(null, false, "Token Expired");
            }

            user.ConnectionId = connId;
            await _userDAL.Update(user);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<User>(user, true, "Successfullt changed password");
        }

        public async Task<ActionResultResponse<string>> GetConnectionId(long id)
        {
            var user = await _userDAL.GetById(id);
            if (user == null)
            {
                return new ActionResultResponse<string>(null, false, "Token expired");
            }
            
            if(user.ConnectionId == null || user.ConnectionId == "")
            {
                return new ActionResultResponse<string>(null, false, "User is not connected");
            }

            return new ActionResultResponse<string>(user.ConnectionId, true, "Successfully");
        }

        public async Task<ActionResultResponse<string>> ChangePhoto(IFormFile photo)
        {
            var id = long.Parse(_jwtService.GetUserId());
            var user = await _userDAL.GetById(id);
            if(user == null)
                return new ActionResultResponse<string>("user not found", false, "Failed to change photo");

            string path = _storageService.CreatePhotoPath();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string photoName = "user" + id + ".jpg";
            path = Path.Combine(path, photoName);

            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            
            user.PhotoPath = Path.Combine("Images", photoName);
            await _userDAL.SaveChangesAsync();

            using (FileStream stream = System.IO.File.Create(path))
            {
                photo.CopyTo(stream);
                stream.Flush();
            }

            return new ActionResultResponse<string>("Success", true, "Successfully changed photo");

        }
        public async Task<ActionResultResponse<List<UserViewModel>>> GetUsers(long programId)
        {
            var id = long.Parse(_jwtService.GetUserId());

            List<UserViewModel> users = new List<UserViewModel>();
            var realUsers = await _userDAL.GetAllByFilter(x => x.Program == programId && x.Id != id && x.Verified && x.Email != "admin@pmf.kg.ac.rs");
            if(realUsers == null)
                return new ActionResultResponse<List<UserViewModel>>(null, false, "There aro no useres yet");

            foreach (var user in realUsers)
            {
                UserViewModel uvm = new UserViewModel();
                uvm = _mapper.Map<UserViewModel>(user);
                uvm.FullName = user.FirstName + " " + user.LastName;
                users.Add(uvm);
            }
            
            return new ActionResultResponse<List<UserViewModel>> (users, true, "All users");
        }

        public async Task<ActionResultResponse<UserProfileViewModel>> GetUser(long id)
        {
            var user = await _userDAL.GetUserForPage(id);
            if(user == null)
                return new ActionResultResponse<UserProfileViewModel>(null, false, "User doesnt exists");

            UserProfileViewModel upvm = new UserProfileViewModel();
            upvm.FullName = user.FirstName + " " + user.LastName;
            upvm.Id = user.Id;
            upvm.Email = user.Email;
            upvm.PhotoPath = user.PhotoPath;
            upvm.ConnectionId = user.ConnectionId;
            upvm.Ispits = user.IspitResenjes.Count();
            upvm.Kolokvijums = user.KolokvijumResenjes.Count();
            List<FavoriteSubjectViewModel> lista = new List<FavoriteSubjectViewModel>();
            foreach(var s in user.FavoriteSubjects)
            {
                var name = await _subjectDAL.GetById(s.SubjectId);
                FavoriteSubjectViewModel fs = new FavoriteSubjectViewModel();
                fs.subjectId = s.SubjectId;
                fs.name = name.Name;

                lista.Add(fs);
            }
            upvm.FavoriteSubjects = lista;
            return new ActionResultResponse<UserProfileViewModel>(upvm, true, "Success");
        }
    }
}