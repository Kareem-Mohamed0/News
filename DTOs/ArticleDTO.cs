namespace News.DTOs
{
    public class ArticleDTO
    {
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
    }
}
