using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReactServer.Setups;

namespace ReactServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(38572, "test123", "Template", new Dictionary<string, string>
			{
				["key1"] = "lasku1"
			}).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(int port, string processKey, string templateKey, Dictionary<string, string> entries)
		{
			return Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(builder =>
				{
					builder
						.UseStartup<Startup>()						
						.UseHttpSys(options =>
						{
							options.Authentication.AllowAnonymous = true;							
							options.UrlPrefixes.Add($"http://+:{port}/tr/{processKey}");
						});
				})
				.ConfigureServices(s => s.AddSingleton(new EntriesSetup(entries)).AddSingleton(new TemplateSetup(templateKey)));
		}
	}
}
