using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;
using ReactiveUI;

namespace MyBookshelf.ViewModels
{
	public class CategoryDetailsViewModel : ViewModelBase
	{
		private readonly MyBookshelfRepository _dbRepository;
		private readonly NavigationStore _navigationStore;

		private ObservableCollection<BookViewModel> _books;
		private BookViewModel? _selectedBook;
		private GenreViewModel? _selectedGenre;

		private List<Book> _allBooksInCat;

		public ObservableCollection<BookViewModel> Books
		{
			get => _books;
			set => this.RaiseAndSetIfChanged(ref _books, value);
		}

		public CategoryEnum Type { get; }

		public BookViewModel? SelectedBook
		{
			get => _selectedBook;
			set => this.RaiseAndSetIfChanged(ref _selectedBook, value);
		}

		public ObservableCollection<GenreViewModel> Genres { get; set; }

		public GenreViewModel? SelectedGenre
		{
			get => _selectedGenre;
			set => this.RaiseAndSetIfChanged(ref _selectedGenre, value);
		}

		public CategoryDetailsViewModel(Category selectedCategory, MyBookshelfRepository dbRepository,
			NavigationStore navigationStore)
		{
			_dbRepository = dbRepository;
			_navigationStore = navigationStore;
			_allBooksInCat = new List<Book>();

			Type = selectedCategory.Type;
			Genres = new ObservableCollection<GenreViewModel>();
			_books = new ObservableCollection<BookViewModel>();
			
			this.WhenAnyValue(m => m.SelectedBook)
				.Throttle(TimeSpan.FromMilliseconds(300))
				.Subscribe(BookSelectedAsync);


			this.WhenAnyValue(m => m.SelectedGenre)
				.Throttle(TimeSpan.FromMilliseconds(300))
				.Subscribe(FilterAsync);

			LoadAllBooksAsync(selectedCategory.Id);
			LoadGenresAsync();
		}

		private async void LoadAllBooksAsync(int catId)
		{
			_allBooksInCat = await _dbRepository.CategoryRepository.GetBooksInCategoryAsync(catId);
			LoadBooks(_allBooksInCat);
		}

		private async void FilterAsync(GenreViewModel? genre)
		{
			if (genre != null)
			{
				var allBooks = await _dbRepository.BookRepository
					.BooksByGenreInCategoryAsync(genre.Genre.Id, _allBooksInCat.Select(b => b.Id).ToList());
				LoadBooks(allBooks);
				return;
			}
			LoadBooks(_allBooksInCat);
		}

		private async void LoadGenresAsync()
		{
			var genres = await _dbRepository.GenreRepository.AllGenresAsync();
			foreach (var genre in genres)
			{
				Genres.Add(new GenreViewModel(genre));
			}
		}

		private void LoadBooks(List<Book> books)
		{
			Books = new ObservableCollection<BookViewModel>();
			foreach (var book in books)
			{
				Books.Add(new BookViewModel(book));
			}
		}

		private void ResetCommand()
		{
			SelectedGenre = null;
		}

		private void BackCommand()
		{
			_navigationStore.SwitchView(new HomePageViewModel(_dbRepository, _navigationStore));
		}

		private async void BookSelectedAsync(BookViewModel? book)
		{
			if (book == null)
			{
				return;
			}
			var realBook = await _dbRepository.BookRepository.GetBookByIdAsync(book.Book.Id);
			if (realBook != null)
			{
				_navigationStore.SwitchView(new BookDetailsViewModel(realBook, _dbRepository, _navigationStore));
			}
		}

	}
}
