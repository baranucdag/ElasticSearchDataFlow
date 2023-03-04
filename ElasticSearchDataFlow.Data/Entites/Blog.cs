namespace ElasticSearchDataFlow.Data.Entites
{
    public class Blog
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogContent { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
