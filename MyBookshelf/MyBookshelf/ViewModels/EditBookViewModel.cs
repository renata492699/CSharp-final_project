using System;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.ViewModels
{
	public class EditBookViewModel : ViewModelBase
	{
		private readonly NavigationStore _navigationStore;
		private readonly MyBookshelfRepository _dbRepository;
		private Book _bookToEdit;
		public BookFormViewModel BookForm { get; set; }

		public EditBookViewModel(MyBookshelfRepository dbRepository, NavigationStore navigationStore, Book bookToEdit)
		{
			_navigationStore = navigationStore;
			_bookToEdit = bookToEdit;
			_dbRepository = dbRepository;
			BookForm = new BookFormViewModel(dbRepository, UpdateBook, GoBack)
			{
				Title = bookToEdit.Title,
				Author = bookToEdit.Author,
				Pages = bookToEdit.Pages,
				ReadingStart = bookToEdit.ReadingStart,
				ReadingFinish = bookToEdit.ReadingFinish,
				Rating = bookToEdit.Rating,
				Note = bookToEdit.Note,
				Categories = new ObservableCollection<int>() {bookToEdit.Categories.Select(c => c.Id)},
				Genres = new ObservableCollection<int>() {bookToEdit.Genres.Select(g => g.Id) }
			};
		}

		private async void UpdateBook()
		{
			var categories =
				BookForm.GetChecked(BookForm.CategoryCheckBoxes);

			var genres =
				BookForm.GetChecked(BookForm.GenreCheckBoxes);

			_bookToEdit.Title = BookForm.Title;
			_bookToEdit.Author = BookForm.Author;
			_bookToEdit.Pages = BookForm.Pages;
			_bookToEdit.ReadingStart = BookForm.ReadingStart;
			_bookToEdit.ReadingFinish = BookForm.ReadingFinish;
			_bookToEdit.Rating = BookForm.Rating;
			_bookToEdit.Note = BookForm.Note;

			try
			{
				_bookToEdit = await _dbRepository.BookRepository.UpdateBookAsync(_bookToEdit, categories, genres);
				_navigationStore.SwitchView(new BookDetailsViewModel(_bookToEdit, _dbRepository, _navigationStore));

			}
			catch (Exception)
			{
				_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore)
				{
					ExceptionVisible = true,
					ExceptionMessage = "Something went wrong while editing a book. Please try again."
				});
			}
		}

		private void GoBack()
		{
			_navigationStore.SwitchView(new BookDetailsViewModel(_bookToEdit, _dbRepository, _navigationStore));
		}
	}
}
