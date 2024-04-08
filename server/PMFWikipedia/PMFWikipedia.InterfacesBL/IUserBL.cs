using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IUserBL
    {
        public Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo);
        public Task<ActionResultResponse<string>> ValidateUser(string registrationToken);
        public Task<ActionResultResponse<LoginResponse>> Login(LoginInfo loginInfo);
        public Task<ActionResultResponse<string>> CreateResetToken(string email);
        public Task<ActionResultResponse<User>> ResetPassword(ResetPasswordInfo info);
    }
}
