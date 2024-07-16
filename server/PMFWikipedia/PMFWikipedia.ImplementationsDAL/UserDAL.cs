using Microsoft.EntityFrameworkCore;
using PMFWikipedia.Models.Entity;
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

        public async Task<User?> GetUserByEmail(string email)
        {
            return await table.Where(x=>x.Email.Equals(email) && x.Verified).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByResetToken(string token)
        {
            return await table.Where(x => x.ResetToken == token && x.Verified && x.ResetTokenExpirationTime > DateTime.Now).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByToken(string registrationToken)
        {
            return await table.Where(x => x.RegisterToken==registrationToken && x.Verified == false && x.RegisterTokenExpirationTime > DateTime.Now).FirstOrDefaultAsync();
        }
    }
}