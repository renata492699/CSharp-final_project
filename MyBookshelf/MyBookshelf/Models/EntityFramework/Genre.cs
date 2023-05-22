using System.Collections.Generic;

namespace MyBookshelf.Models.EntityFramework
{
    public class Genre
    {
        public int Id { get; set; }
        public GenreEnum Type { get; set; }
		public List<Book> Books { get; set; } = new();
	}
}
