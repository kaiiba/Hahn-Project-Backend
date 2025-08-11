using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Application.Common.Interfaces;
using Project.Domain.Entities;
using Project.Infrastructure.Persistence;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var productDtos = products.Select(p => new Infrastructure.Persistence.Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedAt = p.CreatedAt
                }).ToList();

                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                var productDto = new Infrastructure.Persistence.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt
                };

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Infrastructure.Persistence.Product createProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = Domain.Entities.Product.Create(createProductDto.Name, createProductDto.Price, createProductDto.Stock);
                await _productRepository.AddAsync(product);

                var productDto = new Infrastructure.Persistence.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt
                };

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id}/decrease-stock")]
        public async Task<IActionResult> DecreaseStock(int id, [FromBody] DecreaseStockRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                product.DecreaseStock(request.Quantity);
                await _productRepository.UpdateAsync(product);

                var productDto = new Infrastructure.Persistence.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt
                };

                return Ok(productDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class DecreaseStockRequest
    {
        public int Quantity { get; set; }
    }
}
