using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DTOs;
using News.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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
        return Ok(articleRepository.GetAll());
        }

        [HttpGet("GetArticle/{id:int}")]
        public IActionResult GetByID(int id)
        {
            return Ok(articleRepository.GetById(id));
        }

        [HttpGet("{SearchTerm:alpha}")]
        public IActionResult GetByName(string SearchTerm)
        {
            return Ok(articleRepository.GetByName(SearchTerm));
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
        public IActionResult Edit(int id , [FromBody] ArticleDTO articleDTO)
        {
            if (articleDTO == null)
            {
                return BadRequest("ERROR !!! Please Enter Vaild Data !! ");
            }

            articleRepository.Update(id,articleDTO);
            return Ok("Article Updated Successfully!!");
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
            articleRepository.Delete(id);
            return Ok("Article Deleted Successfully!!");
        }













    }
}
