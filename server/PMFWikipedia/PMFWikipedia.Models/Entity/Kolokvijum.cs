﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace PMFWikipedia.Models.Entity
{
    public partial class Kolokvijum
    {
        public Kolokvijum()
        {
            KolokvijumResenjes = new HashSet<KolokvijumResenje>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public long AuthorId { get; set; }
        public long SubjectId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public long LastModifiedBy { get; set; }
        public string? Year { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<KolokvijumResenje> KolokvijumResenjes { get; set; }
    }
}