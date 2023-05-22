using MyBookshelf.Models;
using ReactiveUI;

namespace MyBookshelf.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public MyBookshelfRepository BookshelfRepository { get; set; }
		private readonly NavigationStore _navigationStore;

		private ViewModelBase? _content;

		public ViewModelBase? Content 
		{ 
			get => _content; 
			private set => this.RaiseAndSetIfChanged(ref _content, value); 
		}

		public MainWindowViewModel(MyBookshelfRepository dbRepository, NavigationStore navigationStore)
		{
			BookshelfRepository = dbRepository;
			_navigationStore = navigationStore;
			navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
			_content = navigationStore.CurrentViewModel;
		}

		private void OnCurrentViewModelChanged()
		{
			Content = _navigationStore.CurrentViewModel;
		}
	}
}