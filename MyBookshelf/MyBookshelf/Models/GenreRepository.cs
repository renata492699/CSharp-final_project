using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.Models
{
    public class GenreRepository
    {
		public async Task<List<Genre>> AllGenresAsync()
		{
			using var dbContext = new MyBookshelfDbContext();
			return await dbContext.Genres.ToListAsync();
		}
    }
}
