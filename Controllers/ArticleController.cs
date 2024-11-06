using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.DTOs;
using News.Interfaces;

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
        
        [HttpGet]
        public IActionResult GetAll() 
        {
        return Ok(articleRepository.GetAll());
        }

        [HttpGet("{id:int}")]
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
        public IActionResult Add([FromBody]ArticleDTO articleDTO)
        {
            if (articleDTO == null) 
            {
                return BadRequest("ERROR !!! Please Enter Vaild Data !! ");
            }

            articleRepository.Add(articleDTO);
            return Ok("Article Added Successfully!!");
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
