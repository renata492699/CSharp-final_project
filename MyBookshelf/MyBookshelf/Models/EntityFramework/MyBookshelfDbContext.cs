using System;
using Microsoft.EntityFrameworkCore;


namespace MyBookshelf.Models.EntityFramework
{
    public class MyBookshelfDbContext : DbContext
    {
        private string _connectionString =
            @"server=(localdb)\MSSQLLocalDB; Initial Catalog=MyBookshelfDb; Integrated Security=true";
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public MyBookshelfDbContext() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
	            new Category { Id = 1, Type = CategoryEnum.OnMyBookshelf },
                new Category { Id = 2, Type = CategoryEnum.NowReading },
                new Category { Id = 3, Type = CategoryEnum.Wish },
                new Category { Id = 4, Type = CategoryEnum.Read },
                new Category { Id = 5, Type = CategoryEnum.Favourite }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Type = GenreEnum.Fantasy },
                new Genre { Id = 2, Type = GenreEnum.SciFi },
                new Genre { Id = 3, Type = GenreEnum.Romance },
                new Genre { Id = 4, Type = GenreEnum.Detective },
                new Genre { Id = 5, Type = GenreEnum.Biography },
                new Genre { Id = 6, Type = GenreEnum.Classics },
                new Genre { Id = 7, Type = GenreEnum.Mystery },
                new Genre { Id = 8, Type = GenreEnum.ComicBook },
                new Genre { Id = 9, Type = GenreEnum.HistoricalFiction },
                new Genre { Id = 10, Type = GenreEnum.Poetry },
                new Genre { Id = 11, Type = GenreEnum.Horror },
                new Genre { Id = 12, Type = GenreEnum.Adventure },
                new Genre { Id = 13, Type = GenreEnum.Children },
                new Genre { Id = 14, Type = GenreEnum.Novel },
                new Genre { Id = 15, Type = GenreEnum.YoungAdult },
                new Genre { Id = 16, Type = GenreEnum.LiteraryFiction },
                new Genre { Id = 17, Type = GenreEnum.CookBook },
                new Genre { Id = 18, Type = GenreEnum.Thriller }
			);
            base.OnModelCreating(modelBuilder);
        }
    }
}
