using AutoMapper;
using PMFWikipedia.Common;
using PMFWikipedia.Common.EmailService;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
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

            string body = "" +
                "<h1 style=\"color: #333333;  margin-bottom: 20px;\">Rgistration</h1>" +
                " <p style=\"color: #333333;  line-height: 1.5; margin-bottom: 20px;\">Click the button below to take action:</p> " +
                "<a href=\"" + ConfigProvider.ChangePasswordPage + "?token=" + token + "\" style=\"background-color: #4CAF50; display: inline-block; color: #ffffff; " +
                "padding: 10px 20px;text-decoration: none;border-radius: 5px;\">Click</a>";

            await _emailService.SendEmail(registerInfo.Email, "Registration", body, "Registration");
            
            newUser.RegisterToken = token;
            newUser.RegisterTokenExpirationTime = DateTime.Now.AddMinutes(ConfigProvider.TokenExpirationTime);
            
            await _userDAL.Insert(newUser);
            await _userDAL.SaveChangesAsync();

            return new ActionResultResponse<User>(newUser, true, "");

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
    }
}