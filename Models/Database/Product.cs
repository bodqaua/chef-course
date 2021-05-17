using System;

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

        public static Product createWithId(int id, string name, string price, string quantity)
        {
            Product product = new Product
            {
                Id = id,
                Name = name.Trim(),
                Price = Convert.ToDouble(price),
                Quantity = Convert.ToDouble(quantity)
            };
            return product;
        }

        public static Product CreateCopy(Product product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price,
            };
        }
    }
}
