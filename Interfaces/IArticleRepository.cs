using News.DTOs;
using News.Models;

namespace News.Interfaces
{
    public interface IArticleRepository
    {
        public List<Article> GetAll();
        public Article GetById(int id);
        public Task AddAsync(ArticleDTO articleDto, IFormFile Image);
        public Task Update(int ArticleId, ArticleDTO articleDto, IFormFile image);
        public void Delete(int ArticleId);
    }
}
