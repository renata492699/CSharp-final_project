using System.Collections.Generic;
using System.Linq;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.Models
{
    public class MyBookshelfRepository
    {
        public CategoryRepository CategoryRepository { get; set; }
        public BookRepository BookRepository { get; set; }

        public GenreRepository GenreRepository { get; set; }
        
        public MyBookshelfRepository()
        {
            CategoryRepository = new CategoryRepository();
            BookRepository = new BookRepository();
            GenreRepository = new GenreRepository();
        }
    }
}
