using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MovieExplorer.Client.Models
{
	public class Configuration
	{
		public Images Images { get; set; }

		[JsonProperty("change_keys")]
		public List<string> ChangeKeys { get; set; }
	}
}

