using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.Models
{
    public class BookRepository
    {
	    public async Task<Book?> GetBookByIdAsync(int id)
		{
			using var dbContext = new MyBookshelfDbContext();
			return await dbContext.Books
				.Include(b => b.Categories)
				.Include(b => b.Genres)
				.FirstAsync(b => b.Id == id);
		}

	    public async Task<int> AddBookAsync(Book book, List<int> categories, List<int> genres)
		{
			using var dbContext = new MyBookshelfDbContext();
			var defaultCat = await dbContext.Categories.FirstAsync(c => c.Type == CategoryEnum.OnMyBookshelf);

			book.Categories = await dbContext.Categories.Where(c => categories.Contains(c.Id)).ToListAsync();
			book.Genres = await dbContext.Genres.Where(g => genres.Contains(g.Id)).ToListAsync();

			book.Categories.Add(defaultCat);
			dbContext.Books.Add(book);
			await dbContext.SaveChangesAsync();
			return book.Id;
		}

		public async Task<Book> UpdateBookAsync(Book book, List<int> categories, List<int> genres)
		{
			using var dbContext = new MyBookshelfDbContext();
			var defaultCat = await dbContext.Categories.FirstAsync(c => c.Type == CategoryEnum.OnMyBookshelf);
			var realBook = await dbContext.Books
				.Include(b => b.Categories)
				.Include(b => b.Genres)
				.FirstAsync(b => b.Id == book.Id);

			realBook.Title = book.Title;
			realBook.Author = book.Author;
			realBook.Pages = book.Pages;
			realBook.Rating = book.Rating;
			realBook.ReadingStart = book.ReadingStart;
			realBook.ReadingFinish = book.ReadingFinish;
			realBook.Note = book.Note;
			realBook.Categories = await dbContext.Categories.Where(c => categories.Contains(c.Id)).ToListAsync();
			realBook.Categories.Add(defaultCat);
			realBook.Genres = await dbContext.Genres.Where(g => genres.Contains(g.Id)).ToListAsync();

			dbContext.Books.Update(realBook);
			await dbContext.SaveChangesAsync();
			return realBook;
		}

		public async Task<List<Book>> BooksByGenreInCategoryAsync(int genreId, List<int> bookIds)
		{
			using var dbContext = new MyBookshelfDbContext();
			var result = new List<Book>();
			foreach (var bookId in bookIds)
			{
				var book = await dbContext.Books
					.Include(b => b.Genres)
					.FirstAsync(b => b.Id == bookId);
				if (book.Genres.Select(g => g.Id).Contains(genreId))
				{
					result.Add(book);
				}
			}
			return result;
		}

		public async Task<bool> DeleteBookAsync(int id)
		{
			using var dbContext = new MyBookshelfDbContext();
			var book = await dbContext.Books.FindAsync(id);
			if (book != null)
			{
				dbContext.Books.Remove(book);
			}

			await dbContext.SaveChangesAsync();
			return true;
		}
	}
}