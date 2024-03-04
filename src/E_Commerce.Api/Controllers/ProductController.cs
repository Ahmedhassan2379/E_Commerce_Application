using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-products")]
        public async Task<ActionResult> Get()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(x=>x.category);
            var result = _mapper.Map<List<ProductDto>>(products);
            //var options = new JsonSerializerOptions
            //{
            //    ReferenceHandler = ReferenceHandler.Preserve
            //};
            //string json = JsonSerializer.Serialize(products, options);
            if (products is not  null )
            {
                return Ok(result);
            }
            return BadRequest("Something Error");
        }

        [HttpGet("get-product-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id,x=>x.category);
            if(product is not null)
            {
                var result = _mapper.Map<ProductDto>(product);
                return Ok(result);
            }
            return NotFound();
        }

        [HttpPost("add-product")]
        public async Task<ActionResult> Add(CreateProductDto product)
        {
            if(ModelState.IsValid)
            {
                var newProduct = _mapper.Map<Product>(product);
               await _unitOfWork.ProductRepository.AddAsync(newProduct);
                return Ok(newProduct);
            }
            return BadRequest();   
        }

        [HttpPut("update-product")]
        public async Task<ActionResult> Update(UpdateCategoryDto product)
        {
            if(ModelState.IsValid)
            {
                var existedProduct = await _unitOfWork.ProductRepository.GetAsync(product.Id);
                if(existedProduct is not null)
                {
                    _mapper.Map<UpdateCategoryDto>(existedProduct);
                    return Ok(existedProduct);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }
    }
}
