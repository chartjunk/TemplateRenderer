using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			IHost host = null;
			try
			{
				host = ReactServer.Program.CreateHostBuilder(38572, "test123", "Template", new Dictionary<string, string>
				{
					["key1"] = "lasku1"
				}).Build();
				await host.StartAsync();
				await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
				using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
				{
					Headless = true
				});
				using var page = await browser.NewPageAsync();
				await page.GoToAsync("http://localhost:38572/tr/test123/render?entryKey=key1");
				var stream = await page.PdfStreamAsync();
				return File(stream, "application/octet-stream", "file.pdf");
			}
			catch(Exception e)
			{
				throw;
			}
			finally
			{
				await host.StopAsync();
				await host.WaitForShutdownAsync();
				host.Dispose();
			}
		}
	}
}
