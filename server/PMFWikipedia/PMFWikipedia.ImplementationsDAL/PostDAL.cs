﻿using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class PostDAL : BaseDAL<Post>, IPostDAL
    {
        public PostDAL(PMFWikiContext context) : base(context)
        {
        }

        public async Task<List<Post>> GetAllPostsBySubject(long subjectId)
        {
            return await table.Include(c => c.AuthorNavigation).Include(c => c.SubjectNavigation).Where(x => x.Subject == subjectId).OrderByDescending(x=> x.DateCreated).ToListAsync();
        }

        public async Task<Post> GetPostById(long id)
        {
            return await table.Include(c => c.AuthorNavigation).Include(c=>c.SubjectNavigation).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}