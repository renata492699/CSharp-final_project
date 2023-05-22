using System;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.ViewModels
{
	public class AddBookViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;
		private readonly MyBookshelfRepository _dbRepository;
		public BookFormViewModel BookForm { get; set; }

		public AddBookViewModel(MyBookshelfRepository dbRepository, NavigationStore navigationStore) 
		{
			_navigationStore = navigationStore;
			_dbRepository = dbRepository;
			BookForm = new BookFormViewModel(dbRepository, AddBook, GoBack);
		}

		private async void AddBook()
		{
			var categories =
				BookForm.GetChecked(BookForm.CategoryCheckBoxes);

			var genres =
				BookForm.GetChecked(BookForm.GenreCheckBoxes);

			var book = new Book()
			{
				Title = BookForm.Title!,
				Author = BookForm.Author,
				Pages = BookForm.Pages,
				ReadingStart = BookForm.ReadingStart,
				ReadingFinish = BookForm.ReadingFinish,
				Rating = BookForm.Rating,
				Note = BookForm.Note
			};
			try
			{
				await _dbRepository.BookRepository.AddBookAsync(book, categories, genres);
				_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore));
			}
			catch (Exception)
			{
				_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore)
				{
					ExceptionVisible = true, 
					ExceptionMessage = "Something went wrong while adding a book. Please try again."
				});
			}

		}

		private void GoBack()
		{
			_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore));
		}
	}
}
