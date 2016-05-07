using System.Diagnostics;

namespace MovieExplorer.Client.Models
{
	[DebuggerDisplay("Id={Id}; Name={Name}")]
	public class Genre
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}

