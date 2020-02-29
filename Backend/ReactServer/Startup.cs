using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using React.TinyIoC;
using System.Reflection;
using JavaScriptEngineSwitcher.Core;
using React;

namespace ReactServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddReact();

			// Make sure a JS engine is registered, or you will get an error!
			services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
			  .AddV8();

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//var current = React.TinyIoC.TinyIoCContainer.Current;
			//var registeredTypesField = typeof(TinyIoCContainer).GetField("_RegisteredTypes", BindingFlags.NonPublic | BindingFlags.Instance);
			//var registeredTypes = registeredTypesField.GetValue(current);
			//registeredTypesField.FieldType.GetMethod("Clear").Invoke(registeredTypes, null);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			// Initialise ReactJS.NET. Must be before static files.
			app.UseReact(config =>
			{
				// If you want to use server-side rendering of React components,
				// add all the necessary JavaScript files here. This includes
				// your components as well as all of their dependencies.
				// See http://reactjs.net/ for more information. Example:
				//config
				//  .AddScript("~/js/Template.jsx");

				//config
				//  .SetReuseJavaScriptEngines(true)
				//  .SetMaxEngines(1)
				//  .SetMaxUsagesPerEngine(1)
				//  .SetAllowJavaScriptPrecompilation(false);

				//  .AddScript("~/js/Second.jsx");

				// If you use an external build too (for example, Babel, Webpack,
				// Browserify or Gulp), you can improve performance by disabling
				// ReactJS.NET's version of Babel and loading the pre-transpiled
				// scripts. Example:
				//config
				//  .SetLoadBabel(false)
				//  .AddScriptWithoutTransform("~/js/bundle.server.js");
			});


			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}

