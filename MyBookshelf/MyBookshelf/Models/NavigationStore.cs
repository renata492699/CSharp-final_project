using System;
using MyBookshelf.ViewModels;

namespace MyBookshelf.Models
{
	public class NavigationStore
	{
		public event Action? CurrentViewModelChanged;
		private ViewModelBase? _currentViewModel;

		public ViewModelBase? CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				OnCurrentViewModelChanged();
			}
		}

		private void OnCurrentViewModelChanged()
		{
			CurrentViewModelChanged?.Invoke();
		}

		public void SwitchView(ViewModelBase viewModel)
		{
			CurrentViewModel = viewModel;
		}
	}
}
