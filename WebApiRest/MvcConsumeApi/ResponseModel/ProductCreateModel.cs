using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcConsumeApi.ResponseModel
{
    public class ProductCreateModel
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string ImagePath { get; set; }
        public int? CategoryId { get; set; }
    }
}
