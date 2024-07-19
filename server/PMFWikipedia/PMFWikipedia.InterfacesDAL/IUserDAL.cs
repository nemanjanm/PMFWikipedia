using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IUserDAL : IBaseDAL<User>
    {
        public Task<bool> CheckEmail(string email);
        public Task<User?> GetUserByToken(string registrationToken);
        public Task<User?> GetUserByEmail(string email);
        public Task<User?> GetUserForPhoto(string email);
        public Task <User?> GetUserByResetToken(string token);
    }
}