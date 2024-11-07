using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DTOs;
using News.Interfaces;
using News.Models;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new()
            {
                Description = categoryDTO.Description,
                Name = categoryDTO.Name
            };

            categoryRepository.Add(category);
            return Ok("The Category Added Successfully");
        }

        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var categories = categoryRepository.GetAll();
            var categoriesDto = categories.Select(c => new CategoryDTO
            {
                Description = c.Description,
                Name = c.Name
            }).ToList();

            return Ok(categoriesDto);
        }


        [HttpGet("GetCategory/{id:int}")]
        public IActionResult GetCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            var categoryDto = new CategoryDTO
            {
                Description = category.Description,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            category.Description = categoryDTO.Description;
            category.Name = categoryDTO.Name;
            categoryRepository.Update(category);

            return Ok("The Category Updated Successfully");
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            categoryRepository.Delete(id);
            return Ok("The category was deleted.");
        }
    }
}
