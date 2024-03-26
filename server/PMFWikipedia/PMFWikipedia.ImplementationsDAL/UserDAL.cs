using Microsoft.EntityFrameworkCore;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;
using PMFWikipedia.InterfacesDAL;

namespace PMFWikipedia.ImplementationsDAL
{
    public class UserDAL : BaseDAL<User>, IUserDAL
    {
        public UserDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<bool> CheckEmail(string email)
        {
            return await table.AnyAsync(x=> x.Email == email && x.IsDeleted == false && x.Verified == true);
        }

        public async Task<User?> GetUserByToken(string registrationToken)
        {
            return await table.Where(x => x.RegisterToken==registrationToken && x.Verified == false && x.RegisterTokenExpirationTime > DateTime.Now).FirstOrDefaultAsync();
        }
    }
}