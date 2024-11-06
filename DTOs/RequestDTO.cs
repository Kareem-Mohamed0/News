namespace News.DTOs
{
    public class RequestDTO
    {
        public string ArticleTitle { get; set; }
        public string ArticleContent { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image {  get; set; }
    }
}
