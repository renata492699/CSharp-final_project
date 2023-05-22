using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using MyBookshelf.ViewModels;
using ReactiveUI;

namespace MyBookshelf.Views
{
	public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
	{
		public MainWindow()
		{
			InitializeComponent();
		}
	}
}