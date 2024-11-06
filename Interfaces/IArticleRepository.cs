using News.DTOs;
using News.Models;

namespace News.Interfaces
{
    public interface IArticleRepository
    {
        public List<Article> GetAll();
        public Article GetById(int id);
        public Article GetByName(string ArticleTitle);
        public void Add(ArticleDTO articleDto);
        public void Update(int ArticleId,ArticleDTO articleDto);
        public void Delete(int ArticleId);
    }
}
