using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.ViewModels
{
	public class GenreViewModel : ViewModelBase
	{
		public GenreEnum Type { get; set; }
		public Genre Genre { get; set; }

		public GenreViewModel(Genre genre)
		{
			Type = genre.Type;
			Genre = genre;
		}
	}
}
