using Project.Application.Common.Interfaces;
using Project.Domain.Entities;
using Project.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) => _db = db;

        public async Task<Domain.Entities.Product> GetByIdAsync(int id)
        {
            var efProduct = await _db.Products.FindAsync(id);
            if (efProduct == null) return null;

            var domainProduct = Domain.Entities.Product.Create(efProduct.Name, efProduct.Price, efProduct.Stock);
            domainProduct.SetId(efProduct.Id);
            return domainProduct;
        }

        public async Task<List<Domain.Entities.Product>> GetAllAsync()
        {
            var efProducts = await _db.Products.ToListAsync();
            return efProducts.Select(e => 
            {
                var domainProduct = Domain.Entities.Product.Create(e.Name, e.Price, e.Stock);
                domainProduct.SetId(e.Id);
                return domainProduct;
            }).ToList();
        }

        public async Task AddAsync(Domain.Entities.Product product)
        {   
            var ef = new Persistence.Product
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt
            };
            _db.Products.Add(ef);
            await _db.SaveChangesAsync();
            
            product.SetId(ef.Id);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain.Entities.Product product)
        {
            var efProduct = await _db.Products.FindAsync(product.Id);
            if (efProduct != null)
            {
                efProduct.Name = product.Name;
                efProduct.Price = product.Price;
                efProduct.Stock = product.Stock;
                // Don't update CreatedAt as it should remain unchanged
                
                await _db.SaveChangesAsync();
            }
        }
    }
}