using PMFWikipedia.ImplementationsDAL.PMFWikipedia.Models;

namespace PMFWikipedia.InterfacesDAL
{
    public interface IUserDAL : IBaseDAL<User>
    {
        public Task<bool> CheckEmail(string email);
    }
}