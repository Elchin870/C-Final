using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__Final
{
    public class Stock
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public Stock()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
        }

        public void AddProductToStock(string name, double price, int count)
        {
            Product product = new Product(name, price, count);
            Products.Add(product);
        }

        public void AddCategory(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Category category = new Category(name);
                Categories.Add(category);
            }
            else
            {
                throw new ArgumentException("Kategoriya adi bos ola bilmez");
            }
        }

        public Category GetCategoryByName(string name)
        {
            return Categories.Find(c => c.Name == name);
        }

        public void AddProductToCategory(string categoryName, string? productName, double price, int count)
        {
            Product product = new Product(productName, price, count);
            Category category = GetCategoryByName(categoryName);
            if (category != null)
            {
                category.AddProduct(product);
                AddProductToStock(productName, price, count);
            }
        }

    }
}
