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





        public async Task AddAsync(ArticleDTO articleDto, IFormFile image)
        {
            // Create a new Article object from the ArticleDTO
            Article article = new Article
            {
                Title = articleDto.ArticleTitle,
                Content = articleDto.ArticleContent,
                PublishDate = DateTime.Now,
                CategoryId = articleDto.CategoryId
            };

            if (image != null && image.Length > 0)
            {
                // Validate file extension (only allow images)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
                }

                // Define the path to save the images
                string uploadFolder = Path.Combine(environment.WebRootPath, "images", "ArticleImages");

                // Ensure the directory exists
                Directory.CreateDirectory(uploadFolder);

                // Create a unique filename with timestamp
                string fileName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                string filePath = Path.Combine(uploadFolder, fileName);

                // Save the file on the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Save the relative path to the image in the Article model
                article.Image = Path.Combine("images", "ArticleImages", fileName);
            }

            // Add the article to the database and save changes
            await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();
        }



        public async Task Update(int ArticleId, ArticleDTO articleDto, IFormFile image)
        {
            article = GetById(ArticleId);
            article.Title = articleDto.ArticleTitle;
            article.Content = articleDto.ArticleContent;
            article.PublishDate = DateTime.Now;
            article.CategoryId = articleDto.CategoryId;

            if (image != null && image.Length > 0)
            {
                // Validate file extension (only allow images)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Invalid file type. Only image files are allowed.");
                }

                // Define the path to save the images
                string uploadFolder = Path.Combine(environment.WebRootPath, "images", "ArticleImages");

                // Ensure the directory exists
                Directory.CreateDirectory(uploadFolder);

                // Create a unique filename with timestamp
                string fileName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                string filePath = Path.Combine(uploadFolder, fileName);

                // Save the file on the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Save the relative path to the image in the Article model
                article.Image = Path.Combine("images", "ArticleImages", fileName);
            }

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
