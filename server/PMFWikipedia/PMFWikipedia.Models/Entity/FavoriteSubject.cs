﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace PMFWikipedia.Models.Entity
{
    public partial class FavoriteSubject
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SubjectId { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}