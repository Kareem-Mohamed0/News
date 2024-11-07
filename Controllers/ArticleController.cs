using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DTOs;
using News.Interfaces;
using News.Models;
using News.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository articleRepository;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB in bytes
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

        public ArticleController(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }
        
        [HttpGet("GetAllArticles")]

        public IActionResult GetAll() 
        {
            List<Article> articles = articleRepository.GetAll();
            var articlesDTO = articles.Select(A => new GetArticleDTO
            {
                Id = A.Id,
                ArticleTitle =  A.Title,
               ArticleContent = A.Content,
                ImagePath = A.Image,
                CategoryId = A.CategoryId }).ToList();
            return Ok(articlesDTO);
        }

        [HttpGet("GetArticle/{id:int}")]
        public IActionResult GetByID(int id)
        {
            Article article = articleRepository.GetById(id);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found.");
            }
            var articleDTO = new GetArticleDTO
            {
                Id = article.Id,
                ArticleTitle = article.Title,
                ArticleContent = article.Content,
                ImagePath = article.Image,
                CategoryId = article.CategoryId
            };
            return Ok(articleDTO);
        }

       

        [HttpPost]
        [SwaggerOperation(Summary = "Add a new article with an image", Description = "Uploads an image along with article details.")]
        public async Task<IActionResult> Add([FromForm] RequestDTO requestDTO)
        {
            // Define the maximum allowed file size (5 MB) and allowed file extensions
            const long MaxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
            string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

            // Step 1: Validate Model Properties
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Step 2: Validate Image Presence and Properties
            if (requestDTO.Image == null || requestDTO.Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Image is required.");
            }
            else
            {
                if (requestDTO.Image.Length > MaxFileSize)
                {
                    ModelState.AddModelError("Image", "Image size must be less than 5MB.");
                }
                var extension = Path.GetExtension(requestDTO.Image.FileName).ToLower();
                if (!AllowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Only JPG and PNG files are allowed.");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var articleDTO = new ArticleDTO
                {
                    ArticleTitle = requestDTO.ArticleTitle,
                    ArticleContent = requestDTO.ArticleContent,
                    CategoryId = requestDTO.CategoryId
                };

                await articleRepository.AddAsync(articleDTO, requestDTO.Image);
                return Ok("Article added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update a new article with an image", Description = "Uploads an image along with article details.")]
        public async Task<IActionResult> Edit(int id, [FromForm] RequestDTO requestDTO)
        {
            // Define max file size and allowed file extensions
            const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
            string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

            // Step 1: Verify the article exists
            Article article = articleRepository.GetById(id);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found.");
            }

            // Step 2: Validate the RequestDTO object
            if (requestDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Please enter valid data.");
            }

            // Step 3: Validate image file
            if (requestDTO.Image == null || requestDTO.Image.Length == 0)
            {
                ModelState.AddModelError("Image", "Image is required.");
            }
            else
            {
                // Validate file size
                if (requestDTO.Image.Length > MaxFileSize)
                {
                    ModelState.AddModelError("Image", "Image size must be less than 5MB.");
                }

                // Validate file extension
                var extension = Path.GetExtension(requestDTO.Image.FileName).ToLower();
                if (!AllowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("Image", "Only JPG and PNG files are allowed.");
                }
            }

            // If any validation errors exist, return them as BadRequest
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Step 4: Attempt to update the article
            try
            {
                var articleDTO = new ArticleDTO
                {
                    ArticleTitle = requestDTO.ArticleTitle,
                    ArticleContent = requestDTO.ArticleContent,
                    CategoryId = requestDTO.CategoryId
                };

                // Update the article in the repository
                await articleRepository.Update(id, articleDTO, requestDTO.Image);
                return Ok("Article updated successfully!");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
            Article article = articleRepository.GetById(id);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found.");
            }
            articleRepository.Delete(id);
            return Ok("Article Deleted Successfully!!");
        }













    }
}
