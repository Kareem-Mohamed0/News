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
        public IActionResult AddCategory(CategoryDTO categoryDTO)
        {
            Category category = new()
            {
                Description = categoryDTO.Description,
                Name = categoryDTO.Name
            };
            categoryRepository.Add(category);
            return Created();
        }
        [HttpGet]
        [Route("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var categories = categoryRepository.GetAll().ToList();
            return Ok(categories);
        }
        [HttpGet]
        [Route("GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var category = categoryRepository.GetById(id);
            return Ok(category);
        }
        [HttpPut]
        public IActionResult UpdateCategory(int id,CategoryDTO categoryDTO)
        {
            Category category = categoryRepository.GetById(id);
            category.Description = categoryDTO.Description;
            category.Name = categoryDTO.Name;
            categoryRepository.Update(category);
            return Ok(category);
        }
    }
}
