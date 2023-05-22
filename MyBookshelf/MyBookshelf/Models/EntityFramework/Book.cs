using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyBookshelf.Models.EntityFramework
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? Pages { get; set; }
        public DateTimeOffset? ReadingStart { get; set; }
        public DateTimeOffset? ReadingFinish { get; set; }
        public List<Genre> Genres { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public int? Rating { get; set; }
        public string? Note { get; set; }

    }
}
