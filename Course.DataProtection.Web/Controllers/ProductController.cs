using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.DataProtection.Web.DtoSeed;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Course.DataProtection.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductSeed _productSeed;
        private readonly IDataProtector _dataProtector;
        private readonly IDataProtector _dataProtector2;

        public ProductController(IDataProtectionProvider provider)
        {
            _dataProtector = provider.CreateProtector("private_key_for_example_can_be_ProductController");
            _dataProtector2 = provider.CreateProtector("even_can_define_more_than_one");
            _productSeed = new ProductSeed();
        }
        public IActionResult Index()
        {
            var products = _productSeed.Products.ToList();

            var timeLimitedProtector =  _dataProtector.ToTimeLimitedDataProtector();

            products.ForEach(x =>
            {
                //x.EncrypedId = _dataProtector.Protect(x.Id.ToString());
                x.EncrypedId = timeLimitedProtector.Protect(x.Id.ToString(),TimeSpan.FromSeconds(5));
                x.Id = null;
            });
            return View(products);
        }

        public IActionResult Details(string encryptedId)
        {
            if (encryptedId == null)
            {
                return NotFound();
            }

            //int productId = Int32.Parse(_dataProtector.Unprotect(encryptedId));

            //if unproject() func not called in 5 seconds, IDataProtector will not let it
            var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();
            int productId = Int32.Parse(timeLimitedProtector.Unprotect(encryptedId));

            var product = _productSeed.Products.FirstOrDefault(x => x.Id == productId);
            product.EncrypedId = encryptedId;
            product.Id = null;
            return View(product);
        }
    }
}
