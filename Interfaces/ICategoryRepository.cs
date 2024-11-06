using News.DTOs;
using News.Models;

namespace News.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public Category GetByName(string CategoryName);
        public void Add(Category category);
        public void Update(Category category);
        public void Delete(int CategoryId);
    }
}
