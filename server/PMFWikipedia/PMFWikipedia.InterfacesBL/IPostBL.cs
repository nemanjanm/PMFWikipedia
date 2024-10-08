﻿using PMFWikipedia.Models.ViewModels;
using PMFWikipedia.Models;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.InterfacesBL
{
    public interface IPostBL
    {
        public Task<ActionResultResponse<PostViewModel>> AddPost(PostModel post);
        public Task<ActionResultResponse<Post>> EditPost(PostModel post);
        public Task<ActionResultResponse<PostViewModel>> GetPost(long postId);
        public Task<ActionResultResponse<List<PostViewModel>>> GetAllPosts(long subjectId);
        public Task<ActionResultResponse<bool>> DeletePost(long postId);
    }
}