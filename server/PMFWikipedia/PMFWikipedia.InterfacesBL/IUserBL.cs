using Microsoft.AspNetCore.Http;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;
using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.InterfacesBL
{
    public interface IUserBL
    {
        public Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo);
        public Task<ActionResultResponse<string>> ValidateUser(string registrationToken);
        public Task<ActionResultResponse<LoginResponse>> Login(LoginInfo loginInfo);
        public Task<ActionResultResponse<string>> CreateResetToken(string email);
        public Task<ActionResultResponse<User>> ResetPassword(ResetPasswordInfo info);
        public Task<ActionResultResponse<string>> ChangePhoto(IFormFile photo);
        public Task<ActionResultResponse<List<UserViewModel>>> GetUsers();
        public Task<ActionResultResponse<UserViewModel>> GetUser(long id);
    }
}
