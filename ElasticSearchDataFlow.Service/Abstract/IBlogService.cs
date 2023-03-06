using ElasticSearchDataFlow.Data.Entites;

namespace ElasticSearchDataFlow.Service.Abstract
{
    public interface IBlogService
    {
        List<Blog> GetBlogList(int index, int size);
        void AddBlogs(List<Blog> datas);
        Task<bool> DeleteById(string id);

    }
}
