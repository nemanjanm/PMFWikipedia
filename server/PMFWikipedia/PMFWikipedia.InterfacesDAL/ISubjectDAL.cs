using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesDAL
{
    public interface ISubjectDAL : IBaseDAL<Subject>
    {
        public Task<List<Subject>?> GetByProgramId(long id);
    }
}