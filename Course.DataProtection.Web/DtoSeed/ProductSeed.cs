using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.DataProtection.Web.DTOs;

namespace Course.DataProtection.Web.DtoSeed
{
    public class ProductSeed
    {
        public IEnumerable<ProductDto> Products;
        public ProductSeed()
        {
            Products = new List<ProductDto>
            {
                new ProductDto() {Id = 1,Name = "MPT-76",Price = 2000.20m,Stock = 60000,EncrypedId = String.Empty},
                new ProductDto() {Id = 2,Name = "Koral",Price = 200000.20m,Stock = 18,EncrypedId = String.Empty},
                new ProductDto() {Id = 3,Name = "ATAK",Price = 1200000.20m,Stock = 89,EncrypedId = String.Empty}
            };
        }

    }
}
