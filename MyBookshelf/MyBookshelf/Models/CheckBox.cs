using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBookshelf.Models.EntityFramework;

namespace MyBookshelf.Models
{
	public class CheckBox
	{
		public string Name { get; set; }

		public int Id { get; set; }
		public bool IsChecked { get; set; }

		public CheckBox(int id, string name, bool isChecked)
		{
			this.Id = id;
			Name = name;
			IsChecked = isChecked;
		}
	}
}
