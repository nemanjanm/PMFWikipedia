using AutoMapper;
using PMFWikipedia.Common;
using PMFWikipedia.Common.EmailService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.ViewModels;
using System.Text.RegularExpressions;

namespace PMFWikipedia.ImplementationsBL
{
    public class UserBL : IUserBL
    {
        public readonly IUserDAL _userDAL;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        
        public UserBL(IUserDAL userDAL, IMapper mapper, IEmailService emailService)
        {
            _userDAL = userDAL;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo)
        {
            //string pattern = @"@pmf\.kg\.ac\.rs$";

            if (await _userDAL.CheckEmail(registerInfo.Email))
            {
                return new ActionResultResponse<User>(null, false, "Email Taken");
            }
            /*else if (!Regex.IsMatch(registerInfo.Email, pattern))
            {
                return new ActionResultResponse<User>(null, false, "Email Invalid");
            }*/

            User newUser = _mapper.Map<User>(registerInfo);
            newUser.Password = PasswordService.HashPass(registerInfo.Password);
            newUser.Verified = false;

            string token = Guid.NewGuid().ToString();

            string body = _emailService.GetInitTemplate("Register", token);

            await _emailService.SendEmail(registerInfo.Email, "Registration", body, "Registration");
            
            newUser.RegisterToken = token;
            newUser.RegisterTokenExpirationTime = DateTime.Now.AddMinutes(ConfigProvider.TokenExpirationTime);

            await _userDAL.Insert(newUser);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<User>(newUser, true, "");
        }

        public async Task<ActionResultResponse<string>> CreateResetToken(string email)
        {
            var user = await _userDAL.GetUserByEmail(email);
            if (user == null)
            {
                return new ActionResultResponse<string>(null, false, "Email Taken");
            }

            string token = Guid.NewGuid().ToString();

            string body = _emailService.GetInitTemplate("Change Password", token);

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

            return new ActionResultResponse<User>(user, false, "Successfullt changed password");
        }
    }
}