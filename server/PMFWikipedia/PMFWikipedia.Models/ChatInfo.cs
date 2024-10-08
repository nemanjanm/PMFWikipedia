﻿using PMFWikipedia.Models.ViewModels;

namespace PMFWikipedia.Models
{
    public class ChatInfo
    {
        public long Id { get; set; }
        public long User1Id { get; set; }
        public long User2Id { get; set; }
        public int Unread {  get; set; }
        public DateTime TimeStamp { get; set; }
        public UserViewModel User { get; set; }
    }
}