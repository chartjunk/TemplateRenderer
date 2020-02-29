using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactServer.Setups
{
	public class EntriesSetup
	{
		public EntriesSetup(Dictionary<string, string> entries) => Entries = entries;

		public Dictionary<string, string> Entries { get; }
	}
}
