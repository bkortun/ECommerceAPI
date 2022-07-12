using ECommerceAPI.Application.Repositories.CustomerRepository;
using ECommerceAPI.Application.Repositories.OrderRepository;
using ECommerceAPI.Application.Repositories.ProductRepository;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderReadRepository orderReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerReadRepository customerReadRepository, ICustomerWriteRepository customerWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _productWriteRepository = productWriteRepository;
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(_productReadRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> PostProducts(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price,
            });
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutProducts(VM_Update_Product model)
        {
            var product= await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
