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

        public void deleteProduct(int id)
        {
            var product = this.db.Products.SingleOrDefault(item => item.Id == id); 

            if (product != null)
            {
                this.db.Products.Remove(product);
                this.save();
            }
        }

        public Product GetProductByName(string name)
        {
            return Product.CreateCopy(this.db.Products
                .Where(p => p.Name.ToLower().Trim() == name.ToLower().Trim())
                .FirstOrDefault());
        }

        public bool CheckStocks(List<Product> products)
        {
            List<Product> uniqProducts = this.SortUniqList(products);
            foreach(Product product in uniqProducts)
            {
                Product dbProduct = this.GetProductByName(product.Name);
                if (product.Quantity > dbProduct.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public void SubstractProducts(List<Product> products)
        {
            List<Product> uniqProducts = this.SortUniqList(products);
            foreach (Product product in uniqProducts)
            {
                Product dbProduct = this.GetProductByName(product.Name);
                dbProduct.Quantity -= product.Quantity;
                this.updateProduct(dbProduct.Id, dbProduct);
            }
        }

        private List<Product> SortUniqList(List<Product> products)
        {
            products = products.Select(p => Product.CreateCopy(p)).ToList();
            List<Product> list = new List<Product>();

            while(products.Count != 0)
            {
                List<Product> indexes = new List<Product>();
                list.Add(Product.CreateCopy(products[0]));
                products.RemoveAt(0);
                for (int i = 0; i < products.Count; i++)
                {
                    if (products.Count == 0)
                    {
                        break;
                    }

                    if (products[i].Id == list[list.Count - 1].Id)
                    {
                        list[list.Count - 1].Quantity += products[i].Quantity;
                        indexes.Add(products[i]);
                    }
                }
                for (int i = 0; i < indexes.Count; i++)
                {
                    products.Remove(indexes[i]);
                }
            }
            return list;
        }

        private void save()
        {
            this.db.SaveChanges();
        }
    }
}
