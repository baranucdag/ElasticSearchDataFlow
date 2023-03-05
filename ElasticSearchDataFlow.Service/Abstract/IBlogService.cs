using ElasticSearchDataFlow.Data.Entites;

namespace ElasticSearchDataFlow.Service.Abstract
{
    public interface IBlogService
    {
        List<Blog> GetBlogList();
        void AddBlog();
    }
}
