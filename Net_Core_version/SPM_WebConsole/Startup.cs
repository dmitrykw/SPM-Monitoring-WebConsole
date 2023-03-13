
using SPM_WebConsole.Models;

public class Startup {
    public IConfiguration configRoot { get; }
    public Startup(IConfiguration configuration) {
        configRoot = configuration;
    }
       
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure app here
        App_Globals.Initialize(app.ApplicationServices);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure services here
    }

}