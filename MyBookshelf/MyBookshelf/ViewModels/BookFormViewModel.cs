using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using MyBookshelf.Models;
using MyBookshelf.Models.EntityFramework;
using ReactiveUI;
using CheckBox = MyBookshelf.Models.CheckBox;

namespace MyBookshelf.ViewModels
{
    public class BookFormViewModel : ViewModelBase
	{
		private readonly MyBookshelfRepository _dbRepository;
		private string? _title;
		private string? _author;
		private int? _pages;
		private int? _rating;
		private string? _note;
		private DateTimeOffset? _readingStart;
		private DateTimeOffset? _readingFinish;
		private DateTimeOffset? _selectedDate;
		public ReactiveCommand<Unit, Unit> SubmitCommand { get; set; }
		public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

		public DateTimeOffset? SelectedDate
		{
			get => _selectedDate;
			set => this.RaiseAndSetIfChanged(ref _selectedDate, value);
		}

		public string? Title
		{
			get => _title; 
			set => this.RaiseAndSetIfChanged(ref _title, value);
		}

		public string? Author
		{
			get => _author;
			set => this.RaiseAndSetIfChanged(ref _author, value);
		}

		public int? Pages
		{
			get => _pages;
			set => this.RaiseAndSetIfChanged(ref _pages, value);
		}

		public DateTimeOffset? ReadingStart
		{
			get => _readingStart;
			set => this.RaiseAndSetIfChanged(ref _readingStart, value);
		}

		public DateTimeOffset? ReadingFinish
		{
			get => _readingFinish;
			set => this.RaiseAndSetIfChanged(ref _readingFinish, value);
		}

		public int? Rating
		{
			get => _rating;
			set => this.RaiseAndSetIfChanged(ref _rating, value);
		}

		public string? Note
		{
			get => _note; 
			set => this.RaiseAndSetIfChanged(ref _note, value);
		}

		public ObservableCollection<int> Categories { get; set; }
		public ObservableCollection<int> Genres { get; set; }
		public ObservableCollection<CheckBox> CategoryCheckBoxes { get; set; }
		public ObservableCollection<CheckBox> GenreCheckBoxes { get; set; }


		public BookFormViewModel(MyBookshelfRepository dbRepository, Action submit, Action cancel)
		{
			_dbRepository = dbRepository;
			
			var submitCanExecute = this.WhenAnyValue(m => m.Title, m => !string.IsNullOrWhiteSpace(m));
			SubmitCommand = ReactiveCommand.Create(submit, submitCanExecute);
			CancelCommand = ReactiveCommand.Create(cancel);
			
			CategoryCheckBoxes = new ObservableCollection<CheckBox>();
			GenreCheckBoxes = new ObservableCollection<CheckBox>();
			Categories = new ObservableCollection<int>();
			Genres = new ObservableCollection<int>();
			LoadCategoriesAsync();
			LoadGenresAsync();
		}

		private async void LoadCategoriesAsync()
		{
			var allCategories = await _dbRepository.CategoryRepository.AllCategoriesAsync();
			foreach (var category in allCategories)
			{
				//OnMyBooshelf je automaticky
				if (category.Type != CategoryEnum.OnMyBookshelf)
				{
					CategoryCheckBoxes.Add(new CheckBox(category.Id, category.Type.ToString(), Categories.Contains(category.Id)));
				}
			}
		}

		private async void LoadGenresAsync()
		{
			var allGenres = await _dbRepository.GenreRepository.AllGenresAsync();
			foreach (var genre in allGenres)
			{
				GenreCheckBoxes.Add(new CheckBox(genre.Id, genre.Type.ToString(), Genres.Contains(genre.Id)));
			}
		}

		public List<int> GetChecked(ObservableCollection<CheckBox> checkBoxList)
		{
			return checkBoxList
				.Where(x => x.IsChecked)
				.Select(x => x.Id)
				.ToList();
		}

	}
}
