
using Microsoft.AspNetCore.HttpOverrides;


using FluentValidation.AspNetCore;
using RPPP_WebApp.Models;

using Microsoft.EntityFrameworkCore;

namespace RPPP_WebApp
{


  public  static class StartupExtensions
  {
    public  static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {

     builder.Services.AddDbContext<RPPP05Context>(options =>
                                              options.UseSqlServer(
                                                  builder.Configuration.GetConnectionString("RPPP05")));

            builder.Services.AddControllersWithViews()
                            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());



      return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
      #region Needed for nginx and Kestrel (do not remove or change this region)
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                           ForwardedHeaders.XForwardedProto
      });
      string pathBase = app.Configuration["PathBase"];
      if (!string.IsNullOrWhiteSpace(pathBase))
      {
        app.UsePathBase(pathBase);
      }
      #endregion

      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles()
         .UseRouting()
         .UseEndpoints(endpoints =>
         {
           endpoints.MapDefaultControllerRoute();
         });

      return app;
    }
  }
}