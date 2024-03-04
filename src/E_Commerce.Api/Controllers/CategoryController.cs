using AutoMapper;
using E_Commerce.Api.Dtos;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("allcategory")]
        public async Task<ActionResult> Get() 
        {

                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
         
                if (categories is not null)
                {
                var result = _mapper.Map<IReadOnlyList<Category>,IReadOnlyList<ListingCategoryDto>>(categories);
                return Ok(result);
                }
                return BadRequest("Not Found");

        }


        [HttpGet("category-by-id/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAsync(id);
            if (category is not null)
            {
                var newCategory = _mapper.Map<Category, ListingCategoryDto>(category);
                return Ok(newCategory);
            }
            return BadRequest("this category not Found  ");
        }



        [HttpPost("add-new-category")]
        public async Task<ActionResult> Add(CategoryDto category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newCategory = _mapper.Map<Category>(category);
                    await _unitOfWork.CategoryRepository.AddAsync(newCategory);
                    return Ok("Added Successfully");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
        }


        [HttpPut("update-by-id")]
        public async Task<ActionResult> Update(UpdateCategoryDto category)
        {
            var ExistedCategory = await _unitOfWork.CategoryRepository.GetAsync(category.Id);
            if(ExistedCategory is not null)
            {
               _mapper.Map(category, ExistedCategory);
                 await _unitOfWork.CategoryRepository.UpdateAsync(category.Id, ExistedCategory);
                return Ok("Updated Successfully");
            }
            return BadRequest();
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var category =await _unitOfWork.CategoryRepository.GetAsync(id);
                    if (category is not null)
                    {
                        await _unitOfWork.CategoryRepository.DeleteAsync(id);
                        return Ok("Deleted Successfully");
                    }
                    return BadRequest("this Category not Found");
                }
                return BadRequest("Error in Request");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
           
            
        }
    }
}
