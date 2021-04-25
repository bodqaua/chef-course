using Chef.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        public static Product create(string name, string price, string quantity)
        {
            Product product = new Product
            {
                Name = name.Trim(),
                Price = Convert.ToDouble(price),
                Quantity = Convert.ToDouble(quantity)
            };
            return product;
        }
    }
}
