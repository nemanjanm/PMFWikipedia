using AutoMapper;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsBL
{
    public class PostBL : IPostBL
    {
        private IPostDAL _postDAL;
        private IMapper _mapper;
        public PostBL(IPostDAL postDAL, IMapper mapper)
        {
            _postDAL = postDAL;
            _mapper = mapper;
        }

        public async Task<ActionResultResponse<PostModel>> AddPost(PostModel post)
        {
            Post p = _mapper.Map<Post>(post);
            p.LastEditedBy = post.Author;
            await _postDAL.Insert(p);
            await _postDAL.SaveChangesAsync();
            return new ActionResultResponse<PostModel>(post, true, "");
        }
    }
}