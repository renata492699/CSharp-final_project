using System.Collections.Generic;

namespace MyBookshelf.Models.EntityFramework
{
    public class Category
    {
        public int Id { get; set; }
        public CategoryEnum Type { get; set; }
        public List<Book> Books { get; set; } = new();
	}
}
