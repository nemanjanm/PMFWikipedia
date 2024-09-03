﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace PMFWikipedia.Models.Entity
{
    public partial class User
    {
        public User()
        {
            ChatUser1Navigations = new HashSet<Chat>();
            ChatUser2Navigations = new HashSet<Chat>();
            FavoriteSubjects = new HashSet<FavoriteSubject>();
            NotificationAuthorNavigations = new HashSet<Notification>();
            NotificationReceiverNavigations = new HashSet<Notification>();
            PostAuthorNavigations = new HashSet<Post>();
            PostLastEditedByNavigations = new HashSet<Post>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Program { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public long LastModifiedBy { get; set; }
        public bool Verified { get; set; }
        public string RegisterToken { get; set; } = null!;
        public DateTime RegisterTokenExpirationTime { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpirationTime { get; set; }
        public string? PhotoPath { get; set; }
        public string? ConnectionId { get; set; }

        public virtual ICollection<Chat> ChatUser1Navigations { get; set; }
        public virtual ICollection<Chat> ChatUser2Navigations { get; set; }
        public virtual ICollection<FavoriteSubject> FavoriteSubjects { get; set; }
        public virtual ICollection<Notification> NotificationAuthorNavigations { get; set; }
        public virtual ICollection<Notification> NotificationReceiverNavigations { get; set; }
        public virtual ICollection<Post> PostAuthorNavigations { get; set; }
        public virtual ICollection<Post> PostLastEditedByNavigations { get; set; }
    }
}