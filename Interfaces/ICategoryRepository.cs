using News.DTOs;
using News.Models;

namespace News.Interfaces
{
    public interface ICategoryRepository
    {
        public List<CategoryDTO> GetAll();
        public CategoryDTO GetById(int id);
        public CategoryDTO GetByName(string CategoryName);
        public void Add(CategoryDTO CategoryDto);
        public void Update(int CategoryId, CategoryDTO CategoryDto);
        public void Delete(int CategoryId);
    }
}
