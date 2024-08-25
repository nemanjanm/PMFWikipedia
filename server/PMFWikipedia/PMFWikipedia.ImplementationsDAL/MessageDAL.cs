﻿using Microsoft.EntityFrameworkCore;
using PMFWikipedia.InterfacesDAL;
using PMFWikipedia.Models.Entity;

namespace PMFWikipedia.ImplementationsDAL
{
    public class MessageDAL : BaseDAL<Message>, IMessageDAL
    {
        public MessageDAL(PMFWikiContext context) : base(context)
        {
            
        }

        public async Task<List<Message?>> GetMessagesByChatId(long id)
        {
            return await table.Where(x=> x.ChatId == id).OrderBy(x=>x.TimeStamp).ToListAsync();
        }

        public async Task<List<Message?>> SetMessagesAsRead(long chatId, long userId)
        {
            return await table.Where(x => x.ChatId == chatId && x.SenderId != userId).ToListAsync();
        }
    }
}