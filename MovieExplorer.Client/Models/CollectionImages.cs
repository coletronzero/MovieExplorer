﻿using System.Diagnostics;
using System.Collections.Generic;

namespace MovieExplorer.Client.Models
{
	[DebuggerDisplay("Id={Id}")]
	public class CollectionImages
	{
		public int Id { get; set; }
		public List<Image> Backdrops { get; set; }
		public List<Image> Posters { get; set; }
	}
}

