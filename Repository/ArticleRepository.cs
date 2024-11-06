using Microsoft.AspNetCore.Http.HttpResults;
using News.Data;
using News.DTOs;
using News.Interfaces;
using News.Models;

namespace News.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsDbContext context;
        private readonly IWebHostEnvironment environment;
        Article article = new Article();
        public ArticleRepository(NewsDbContext _context , IWebHostEnvironment environment)
        {
            context = _context;
            this.environment = environment;
        }

        public List<Article> GetAll()
        {
           return context.Articles.ToList();
        }


        public Article GetById(int id)
        {
           
     
            return context.Articles.FirstOrDefault(a=>a.Id == id);
        }


        public List<Article> GetByName(string ArticleTitle)
        {
            return context.Articles
       .Where(a => a.Title.Contains(ArticleTitle, StringComparison.OrdinalIgnoreCase))
       .ToList();
        }



        public async void Add(ArticleDTO articleDto)
        {
          Article article = new Article();
            article.Title = articleDto.ArticleTitle;
            article.Content= articleDto.ArticleContent;
            article.PublishDate = DateTime.Now;
            article.CategoryId = articleDto.CategoryId;

            if (articleDto.Image != null)
            {
                // Define the path to save the images (e.g., wwwroot/images/articles)
                string uploadFolder = Path.Combine(environment.WebRootPath, "images", "articles");

                // Ensure the directory exists
                Directory.CreateDirectory(uploadFolder);

                // Create a unique filename with timestamp to avoid overwriting
                string fileName = $"{Path.GetFileNameWithoutExtension(articleDto.Image.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(articleDto.Image.FileName)}";
                string filePath = Path.Combine(uploadFolder, fileName);

                // Save the file on the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await articleDto.Image.CopyToAsync(stream);
                }

                // Save the relative path to the image in the database
                article.Image = Path.Combine("images", "articles", fileName);
            }
            context.Articles.Add(article);
            await context.SaveChangesAsync();



        }


        public void Update(int ArticleId, ArticleDTO articleDto)
        {
            article = GetById(ArticleId);
            article.Title = articleDto.ArticleTitle;
            article.Content = articleDto.ArticleContent;
            article.PublishDate = DateTime.Now;
            article.CategoryId= articleDto.CategoryId;
            context.SaveChanges();
        }



        public void Delete(int ArticleId)
        {
            Article article = GetById(ArticleId);
            context.Articles.Remove(article);
            context.SaveChanges();
            
        }





    }
}
