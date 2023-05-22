using System.Collections.Generic;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.ViewModels
{
    public class BookViewModel : ViewModelBase
	{
		public Book Book { get; }
		public string? Title { get; }
		public List<Category> Categories { get; }

		public BookViewModel(Book book)
		{
			Book = book;
			Title = book.Title;
			Categories = book.Categories;
		}
	}
}
