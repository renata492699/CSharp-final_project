using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.Models
{
	public class CategoryRepository
	{
		public async Task<List<Category>> AllCategoriesAsync()
		{
			using var dbContext = new MyBookshelfDbContext();
			return await dbContext.Categories.Include(c => c.Books).ToListAsync();
		}

		public async Task<List<Book>> GetBooksInCategoryAsync(int categoryId)
		{
			using var dbContext = new MyBookshelfDbContext();
			var category = await dbContext.Categories
				.Include(c => c.Books)
				.FirstAsync(c => c.Id == categoryId);
			return category.Books;
		}
	}
}
