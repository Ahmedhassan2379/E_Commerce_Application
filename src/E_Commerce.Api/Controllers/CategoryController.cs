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
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("allcategory")]
        public async Task<ActionResult> Get() 
        {

                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                if (categories is not null)
                {
                    return Ok(categories);
                }
                return BadRequest("Not Found");

        }


        [HttpGet("category-by-id/{id}")]
        public ActionResult Get(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetAsync(id);
            if (category is not null)
            {
                return Ok(category);
            }
            return BadRequest("this category not Found");
        }



        [HttpPost("add-new-category")]
        public async Task<ActionResult> Add(CategoryDto category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newCategory = new Category
                    {
                        Name = category.Name,
                        Description = category.Description,
                    };
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


        [HttpPut("update-by-id/{id}")]
        public async Task<ActionResult> Update(int id,CategoryDto category)
        {
            var ExistedCategory = await _unitOfWork.CategoryRepository.GetAsync(id);
            if(ExistedCategory is not null)
            {
                ExistedCategory.Name = category.Name;
                ExistedCategory.Description = category.Description;
                 await _unitOfWork.CategoryRepository.UpdateAsync(id, ExistedCategory);
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
