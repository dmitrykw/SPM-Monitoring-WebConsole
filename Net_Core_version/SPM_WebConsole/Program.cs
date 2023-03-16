using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config/startup.json");
builder.Configuration.AddJsonFile("Config/spm_api_settings.json");


bool _listenHTTP = builder.Configuration.GetValue<bool>("ListenHTTP");
string _listenHTTP_URI = builder.Configuration.GetValue<string>("ListenHTTP_URI");
int _httpPort = builder.Configuration.GetValue<int>("HttpPort");
bool _listenHTTPS = builder.Configuration.GetValue<bool>("ListenHTTPS");
string _listenHTTPS_URI = builder.Configuration.GetValue<string>("ListenHTTPS_URI");
int _httpsPort = builder.Configuration.GetValue<int>("HttpsPort"); ;
bool _provideSSLCert = builder.Configuration.GetValue<bool>("ProvideSSLCert");
string _certificatePath = builder.Configuration.GetValue<string>("CertificatePath");
string _certificatePassword = builder.Configuration.GetValue<string>("CertificatePassword");

IPAddress _listenHTTP_IP;
if (_listenHTTP_URI.ToLower() == "any")
{
    _listenHTTP_IP = IPAddress.Any;
}
else if(_listenHTTP_URI.ToLower() == "loopback")
{
    _listenHTTP_IP = IPAddress.Loopback;
}
else
{
    if (!IPAddress.TryParse(_listenHTTP_URI, out _listenHTTP_IP))
    {
         _listenHTTP_IP = IPAddress.Any;
    }
}


IPAddress _listenHTTPS_IP;
if (_listenHTTPS_URI.ToLower() == "any")
{
    _listenHTTPS_IP = IPAddress.Any;
}
else if (_listenHTTPS_URI.ToLower() == "loopback")
{
    _listenHTTPS_IP = IPAddress.Loopback;
}
else
{
    if (!IPAddress.TryParse(_listenHTTPS_URI, out _listenHTTPS_IP))
    {
        _listenHTTPS_IP = IPAddress.Any;
    }
}


builder.WebHost.ConfigureKestrel(options =>
{
    if (_listenHTTP)
    {
        options.Listen(_listenHTTP_IP, _httpPort); // Listen for non-SSL connections on the port specified in the configuration file
    }
    if (_listenHTTPS)
    {
        options.Listen(_listenHTTPS_IP, _httpsPort, listenOptions =>
        {
            if (_provideSSLCert)
            {
                listenOptions.UseHttps(_certificatePath, _certificatePassword); // Listen for SSL connections on the port specified in the configuration file, using the certificate path and password specified in the configuration file
            }
            
        });
    }

});

builder.Services.AddControllersWithViews();

var startup = new Startup(builder.Configuration); //Add startup class
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method

// Add services to the container.


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Monitoring}/{action=Index}/{id?}");

app.Run();
