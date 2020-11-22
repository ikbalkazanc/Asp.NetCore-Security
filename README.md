<p align="center">
  <a href="https://github.com/ikbalkazanc">
    <img src="https://lh3.googleusercontent.com/proxy/pUt-ybQ0BaBUWHh7cQsbTj33qb9gdPJFUhwCfb7eqi0JblCvN53sPhaSXXk0irULCByr_ZLiOcnAb5tMAmXd6KI" alt="Logo" width="420" height="140">
  </a>

  <h3 align="center">Asp.Net Core 3.1 Basic Attacks and Solutions</h3>
  <p align="center">
    This repository explain simple but effective attacks through examples. Asp.Net  can  resolving many problem. We need to do just let it.
    <br />
    <br />
    <br />
    <br />
  </p>
</p>

* [Security Precautions](#security-precautions)
  * [Data Protection](#data-protection)
  * [IP Control](#ip-control)
  * [Secret Protection](#installation)
  * [CORS](#installation)
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
```cs
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
IP control provide to define blacklist or whitelist for IPs. We will manegement IP lists. Thus we can block malicious. We will code as middleware in this sample.  
```sh
```
<!-- CONTACT -->
## Contact

Muhammet İkbal KAZANCI - [LinkedIn](https://www.linkedin.com/in/ikbalkazanc/) - mi.kazanci@hotmail.com

