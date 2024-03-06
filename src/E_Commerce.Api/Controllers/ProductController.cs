using AutoMapper;
using E_Commerce.Api.Errors;
using E_Commerce.Core.Dtos;
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
            return NotFound(new BaseCommonResponse(404));
        }

        [HttpPost("add-product")]
        public async Task<ActionResult> Add([FromForm]CreateProductDto product)
        {
            if(ModelState.IsValid)
            {
              var result =  await _unitOfWork.ProductRepository.AddAsync(product);
                return result ? Ok(product):BadRequest();
            }
            return BadRequest();   
        }

        [HttpPut("update-product/{id}")]
        public async Task<ActionResult> Update(int id,[FromForm]UpdateProductDto product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.ProductRepository.UpdateAsync(id,product);
                    return Ok($"{result} Updated Succefully");
                }
                return BadRequest(); 
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("delete-product/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _unitOfWork.ProductRepository.DeleteWithPicAsync(id);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
        }
    }
}
