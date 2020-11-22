<p align="center">
  <a href="https://github.com/ikbalkazanc">
    <img src="https://lh3.googleusercontent.com/proxy/pUt-ybQ0BaBUWHh7cQsbTj33qb9gdPJFUhwCfb7eqi0JblCvN53sPhaSXXk0irULCByr_ZLiOcnAb5tMAmXd6KI" alt="Logo" width="420" height="140">
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
  * [CORS](##cors-cross-origin-resource-sharing)
* [Attacks](#usage)
  * [XSS](#prerequisites)
    * [Reflected](#prerequisites)
    * [Stored](#prerequisites)
  * [CSRF](#installation)
  * [Open Redirect Attacks](#installation)
  * [SQL Injection](#installation)
# Security Precautions
## Data Protection
Not need "TATAVA". in sum, Valuable data are require hide. Therefore we will encrypt data. Thus datas are store as encrypted for memory or cookies in client side.
```sh
www.domain.com/products/77
www.domain.com/products/PIjlIX2rVMPEsXyZ9rvhQJdJDxXRr5zyt_hiDhRRlBmtLo1npprgm2CMnQRcBWylcVWq8fjvwyngsfad
````

We will use `Microsoft.AspNetCore.DataProtection`. Have require two things us. Top secret key and (`IDataProtector`) dependency injection in controller. Key thinkable like door key. Key is require for opening door.
```sh
private readonly IDataProtector _dataProtector;

public ProductController(IDataProtectionProvider provider)
{
  _dataProtector = provider.CreateProtector("private_key_for_example_can_be_ProductController");            
}
````
And we can define encrypter in action input and output. I'm fondle .Net Core's eye. it is just that easy.  
```sh
public IActionResult Index()
{
int userId = 1001;
int encrypUserId = _dataProtector.Protect(userId);
return View(encrypUserId);
}
````
```sh
public IActionResult Index(string encryptedId)
{
int userPassword = Int32.Parse(_dataProtector.Unrotect(userPassword));
return View();
}
````
<bold>Note :</bold> Above all must add `services.AddDataProtection()` in `Startup.cs` services. Also all of these can be as middleware.

## IP Control
IP control provide to define blacklist or whitelist for IPs. We will manegement IP lists. Thus we can block malicious. We will code as middleware level in this sample. Therefore we need to define `RequestDelegate` in dependency injections.   
```sh
private readonly RequestDelegate _next;
  private readonly string[] _ipBlackList = {"127.0.0.1", "::1"};
  
  public IPSafeMiddleWare(RequestDelegate next)
  {
    _next = next;
  }
```
And coding `Invoke()` method. This method provide coding middleware to us. Required request context for method. And we check that it is in blacklist.
```sh
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
```
app.UseMiddleware<IPSafeMiddleWare>();
```
Actually you can do this checking process as filter too. Thus you can checking ip in controller level or action level. Maybe this can improve performance.  
## Secret Protection
Normally we are recording connection string to `appsettings.json`. But datas not be in safe there. Asp.Net Core is avert this situation. it's providing top secret file for top secret data. Thanks Bill Doors 🙏. <br/>
<div align="center">
<img src="https://github.com/ikbalkazanc/Asp.NetCore-Security/blob/master/resim_2020-11-23_022355.png" alt="Logo" width="420" height="140">
  </div>
<br/>

We can access by right click web project file then choice `Manage Users Secrets` so to top secret file. Now, We can write secret contexts inside of `appsettings.json` to this json file. Asp.NET Core add `secrets.json` inside to `appsettings.json` in compile time. Thinkable like one file.   

## CORS (Cross-Origin Resource Sharing)
<!-- CONTACT -->
## Contact
Muhammet İkbal KAZANCI - [LinkedIn](https://www.linkedin.com/in/ikbalkazanc/) - mi.kazanci@hotmail.com

