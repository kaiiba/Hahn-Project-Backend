using System;

namespace Project.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Product() { }

        public static Product Create(string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("The product name must be filled up.");
            if (price <= 0) 
                throw new ArgumentException("The product price must be positive.");
            
            return new Product 
            { 
                Name = name, 
                Price = price, 
                Stock = stock, 
                CreatedAt = DateTime.UtcNow 
            };
        }

        public void DecreaseStock(int qty)
        {
            if (qty <= 0 || qty > Stock) 
                throw new InvalidOperationException("The product quantity must be less or equal the quantity in stock.");
            Stock -= qty;
        }

        public void SetId(int id) => Id = id;
    }
}