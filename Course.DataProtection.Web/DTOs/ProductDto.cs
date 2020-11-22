using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.DataProtection.Web.DTOs
{
    public class ProductDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }  
        public decimal Price { get; set; }
        public int Stock  { get; set; }
        public string EncrypedId { get; set; } 
    }
}
