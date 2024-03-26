using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesBL
{
    public interface IUserBL
    {
        public Task<ActionResultResponse<User>> Register(RegisterInfo registerInfo);
        public Task<ActionResultResponse<string>> ValidateUser(string registrationToken);
    }
}
