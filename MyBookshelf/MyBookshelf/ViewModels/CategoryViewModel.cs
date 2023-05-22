using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;
using ReactiveUI;

namespace MyBookshelf.ViewModels
{
    public class CategoryViewModel : ViewModelBase
	{
		private readonly MyBookshelfRepository _dbRepository;
		private readonly NavigationStore _navigationStore;
		private ObservableCollection<BookViewModel> _books;
		private BookViewModel? _selectedBook;

		public Category Category { get; }
		public CategoryEnum Type { get; }

		public ObservableCollection<BookViewModel> Books
		{
			get => _books;
			set => this.RaiseAndSetIfChanged(ref _books, value);
		}

		public BookViewModel? SelectedBook
		{
			get => _selectedBook;
			set => this.RaiseAndSetIfChanged(ref _selectedBook, value);
		}

		public CategoryViewModel(Category category, MyBookshelfRepository dbRepository, NavigationStore navigationStore)
		{
			_dbRepository = dbRepository;
			_navigationStore = navigationStore;
			Type = category.Type;
			Category = category;
			_books = new ObservableCollection<BookViewModel>();
			LoadBooks(category.Books);

			this.WhenAnyValue(m => m.SelectedBook)
				.Throttle(TimeSpan.FromMilliseconds(300))
				.Subscribe(BookSelectedAsync);
		}

		private void LoadBooks(List<Book> books)
		{
			foreach (var book in books)
			{
				Books.Add(new BookViewModel(book));
			}
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
