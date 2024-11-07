namespace News.DTOs
{
    public class GetArticleDTO
    {
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
    }
}
