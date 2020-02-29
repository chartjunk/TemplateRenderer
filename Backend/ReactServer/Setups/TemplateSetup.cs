using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactServer.Setups
{
	public class TemplateSetup
	{
		public TemplateSetup(string templateKey) => TemplateKey = templateKey;

		public string TemplateKey { get; }
	}
}
