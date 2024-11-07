using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DTOs;
using News.Interfaces;
using News.Models;
using News.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository articleRepository;

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
            if (requestDTO == null)
            {
                return BadRequest("ERROR !!! Please Enter Valid Data !!");
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
                return Ok("Article Added Successfully!!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        
        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Update a new article with an image", Description = "Uploads an image along with article details.")]
        public async Task<IActionResult> Edit(int id,[FromForm] RequestDTO requestDTO)
        {
            Article article = articleRepository.GetById(id);
            if (article == null)
            {
                return NotFound($"Article with ID {id} not found.");
            }
            if (requestDTO == null)
            {
                return BadRequest("ERROR !!! Please Enter Valid Data !!");
            }

            try
            {
                var articleDTO = new ArticleDTO
                {
                    ArticleTitle = requestDTO.ArticleTitle,
                    ArticleContent = requestDTO.ArticleContent,
                    CategoryId = requestDTO.CategoryId
                };

                await articleRepository.Update(id,articleDTO, requestDTO.Image);
                return Ok("Article Updated Successfully!!");
            }
            catch (Exception ex)
            {
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
