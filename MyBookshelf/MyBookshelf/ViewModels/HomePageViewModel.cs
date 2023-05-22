using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using MyBookshelf.Models;
using ReactiveUI;

namespace MyBookshelf.ViewModels
{
	public class HomePageViewModel : ViewModelBase
	{
		private readonly MyBookshelfRepository _dbRepository;
		private ObservableCollection<CategoryViewModel> _categories;
		private readonly NavigationStore _navigationStore;
		private CategoryViewModel? _selectedCategory;
		private bool _exceptionVisible;
		private string? _exceptionMessage;

		public string? ExceptionMessage
		{
			get => _exceptionMessage; 
			set => this.RaiseAndSetIfChanged(ref _exceptionMessage, value);
		}

		public bool ExceptionVisible
		{
			get => _exceptionVisible; 
			set => this.RaiseAndSetIfChanged(ref _exceptionVisible, value);
		}

		public ObservableCollection<CategoryViewModel> Categories
		{
			get => _categories; 
			set => this.RaiseAndSetIfChanged(ref _categories, value);
		}

		public CategoryViewModel? SelectedCategory
		{
			get => _selectedCategory;
			set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
		}

		public HomePageViewModel(MyBookshelfRepository dbRepository, NavigationStore navigationStore)
		{
			_navigationStore = navigationStore;
			_dbRepository = dbRepository;
			_categories = new ObservableCollection<CategoryViewModel>();
			LoadCategoriesAsync();

			this.WhenAnyValue(m => m.SelectedCategory)
				.Throttle(TimeSpan.FromMilliseconds(300))
				.Subscribe(SwitchView);
		}

		private void SwitchView(CategoryViewModel? category)
		{
			if (category == null)
			{
				return;
			}
			_navigationStore.SwitchView(new CategoryDetailsViewModel(category.Category, _dbRepository, _navigationStore));
		}

		private async void LoadCategoriesAsync()
		{
			var allCategories = await _dbRepository.CategoryRepository.AllCategoriesAsync();
			foreach (var category in allCategories)
			{
				Categories.Add(new CategoryViewModel(category, _dbRepository, _navigationStore));
			}
		}

		public void AddBookCommand()
		{
			_navigationStore.SwitchView(new AddBookViewModel(_dbRepository, _navigationStore));
		}

		public void ExceptionCommand()
		{
			ExceptionVisible = false;
		}
	}

}
