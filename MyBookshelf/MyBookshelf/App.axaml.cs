using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MyBookshelf.Models;
using MyBookshelf.ViewModels;
using MyBookshelf.Views;

namespace MyBookshelf
{
	public partial class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted()
		{
			base.OnFrameworkInitializationCompleted();
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var dbRepository = new MyBookshelfRepository();
				var navigation = new NavigationStore();
				navigation.CurrentViewModel = new HomePageViewModel(dbRepository, navigation);
				desktop.MainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(dbRepository, navigation),
				};
			}
		}
	}
}