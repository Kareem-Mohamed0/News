using News.Data;
using News.DTOs;
using News.Models;
using News.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace News.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsDbContext context;

        public CategoryRepository(NewsDbContext context)
        {
            this.context = context;
        }

        public void Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Delete(int CategoryId)
        {
            var category = GetById(CategoryId);
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public List<Category> GetAll()
        {
            var categories = context.Categories.ToList();
            return categories;
        }

        public Category GetById(int id)
        {
            var category = context.Categories.FirstOrDefault(C => C.Id == id);
            return category;
        }

        public Category GetByName(string CategoryName)
        {
            var category = context.Categories.FirstOrDefault(C => C.Name == CategoryName);
            if (category != null)
                return category;
            throw new Exception(message: "No Category by this Name.");
        }

        public void Update(Category category)
        {
            context.Entry(category).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
