using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dto;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       
        private readonly ICategoryRespository _categoryRepository;

        public CategoriesController( ICategoryRespository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateCategory(CategoryRequestDto requestDto) 
        {
            var category = new Category
            {
                Name = requestDto.Name,
                UrlHandle = requestDto.UrlHandle
            };

            var result = await _categoryRepository.CreateAsync(category);

            var response = new CategoryDto
            {
                Id = result.Id,
                Name = result.Name,
                UrlHandle = result.UrlHandle
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryRepository.GetAllCategoriesAsync();

            // Map the domain model to the Dto

            var result = new List<CategoryDto>();
            foreach (var category in response)
            {
                result.Add(new CategoryDto { Id = category.Id,Name = category.Name ,UrlHandle = category.UrlHandle});
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetCategoryById([FromRoute]  Guid id)
        {
            var existingCategory = await _categoryRepository.GetById(id);
            if(existingCategory is null)
            {
                return NotFound();
            }
            
                var result = new CategoryDto
                {
                    Id = existingCategory.Id,
                    Name = existingCategory.Name,
                    UrlHandle = existingCategory.UrlHandle
                };
            return Ok(result);
            
        }

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> EditCategory([FromRoute] Guid id,  CategoryRequestDto category)
        {
            // Convert Dto to Domain Model
            var categoryValue = new Category
            {
                Id = id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            categoryValue = await _categoryRepository.UpdateAsync(categoryValue);

            if(categoryValue is null)
            {
                return NotFound();
            }
            // Convert Domain Model to Dto

            var response = new CategoryDto { Id = categoryValue.Id, Name = categoryValue.Name, UrlHandle = categoryValue.UrlHandle };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var response = await _categoryRepository.DeleteAsync(id);
            if (response != null)
            {
                var result = new CategoryDto
                {
                    Id = response.Id,
                    Name = response.Name,
                    UrlHandle = response.UrlHandle

                };
                return Ok(result);
            }
            return NotFound();
        }
    }
}
