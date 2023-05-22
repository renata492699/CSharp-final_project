using System;
using System.Collections.ObjectModel;
using DynamicData;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;
using ReactiveUI;

namespace MyBookshelf.ViewModels
{
    public class BookDetailsViewModel : ViewModelBase
	{
		private readonly Book _book;
		private readonly MyBookshelfRepository _dbRepository;
		private readonly NavigationStore _navigationStore;

		private string? _author;
		private decimal? _pages;
		private decimal? _rating;
		private string? _note;
		private bool _confirmDeleteVisible;
		private bool _deleteVisible;

		public bool ConfirmDeleteVisible
		{
			get => _confirmDeleteVisible;
			set => this.RaiseAndSetIfChanged(ref _confirmDeleteVisible, value);
		}

		public bool DeleteVisible
		{
			get => _deleteVisible;
			set => this.RaiseAndSetIfChanged(ref _deleteVisible, value);
		}

		public string? Title { get; set; }

		public string? Author
		{
			get => _author ?? "Unknown";
			set => _author = value;
		}

		public decimal? Pages
		{
			get => _pages ?? 0;
			set => _pages = value;
		}

		public DateTimeOffset? ReadingStart { get; set; }

		public string ReadingStartString
		{
			get
			{
				if (ReadingStart == null)
				{
					return "Have not started yet.";
				}

				return $"{ReadingStart:d}";
			}
		}

		public DateTimeOffset? ReadingFinish { get; set; }

		public string ReadingFinishString
		{
			get
			{
				if (ReadingFinish == null)
				{
					return "Have not finished yet.";
				}
				return $"{ReadingFinish:d}";
			}
		}

		public decimal? Rating
		{
			get => _rating ?? 0;
			set => _rating = value;
		}

		public string RatingString => string.Format($"{Rating}/ 10");

		public string PagesString => string.Format($"{Pages} pages");

		public string? Note
		{
			get => _note ?? string.Empty;
			set => _note = value;
		}

		public ObservableCollection<Genre> Genres { get; set; }

		public BookDetailsViewModel(Book book, MyBookshelfRepository dbRepository, NavigationStore navigationStore)
		{
			_book = book;
			_dbRepository = dbRepository;
			_navigationStore = navigationStore;

			_confirmDeleteVisible = true;
			_deleteVisible = false;

			Title = book.Title;
			Author = book.Author;
			Pages = book.Pages;
			ReadingStart = book.ReadingStart;
			ReadingFinish = book.ReadingFinish;
			Rating = book.Rating;
			Note = book.Note;

			Genres = new ObservableCollection<Genre>();
			Genres.AddRange(book.Genres);
		}

		public void GoHomeCommand()
		{
			_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore));
		}

		public void EditBookCommand()
		{
			_navigationStore.SwitchView(new EditBookViewModel(_dbRepository, _navigationStore, _book));
		}

		public void ConfirmDeleteCommand()
		{
			ConfirmDeleteVisible = false;
			DeleteVisible = true;
		}

		public void CancelDeleteCommand()
		{
			ConfirmDeleteVisible = true;
			DeleteVisible = false;
		}

		public async void DeleteCommandAsync()
		{
			try
			{
				await _dbRepository.BookRepository.DeleteBookAsync(_book.Id);
				_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore));
			}
			catch (Exception)
			{
				_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore)
				{
					ExceptionVisible = true,
					ExceptionMessage = "Something went wrong while deleting a book. Please try again."
				});
			}
		}
	}
}
