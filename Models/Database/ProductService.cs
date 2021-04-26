using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chef.Models.Database
{
    public class ProductService
    {
        private DatabaseContext db;
        public ProductService(DatabaseContext databaseContext)
        {
            this.db = databaseContext;
        }
        public DbSet<Product> loadProducts()
        {
            return this.db.Products;
        }

        public void saveProducts(List<Product> products)
        {
            foreach(Product product in products)
            {
                this.db.Products.Add(product);
            }
            this.save();
        }

        public void updateProduct(int id, Product product)
        {
            var entity = this.db.Products.Where(w => w.Id == id).FirstOrDefault();
            entity.Name = product.Name;
            entity.Price = product.Price;
            entity.Quantity = product.Quantity;
            this.save();
        }

        public void saveProductsUniq(List<Product> products)
        {
            DbSet<Product> dbProducts = this.loadProducts();
            foreach (Product product in products)
            {
                bool isUniq = true;
                foreach (Product dbProduct in dbProducts)
                {
                    if (dbProduct.Name == product.Name)
                    {
                        dbProduct.Quantity += product.Quantity;
                        dbProduct.Price = product.Price;
                        isUniq = false;
                    }
                }

                if (isUniq)
                {
                    dbProducts.Add(product);
                }
                this.save();
            }
        }

        private void save()
        {
            this.db.SaveChanges();
        }
    }
}
