using ElasticSearchDataFlow.Data.Entites;
using ElasticSearchDataFlow.Data.Repositories;
using ElasticSearchDataFlow.Service.Abstract;

namespace ElasticSearchDataFlow.Service.Concrete
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public List<Blog> GetBlogList(int index, int size)
        {
            return _blogRepository.GetAllPaginated(index, size).ToList();
        }

        public void AddBlogs(List<Blog> datas)
        {
            _blogRepository.AddRange(datas);
        }

        public async Task<bool> DeleteById(string id)
        {
            await _blogRepository.RemoveAsync(id);
            await _blogRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(string id)
        {
            var entity = await _blogRepository.GetByIdAsync(id);
            _blogRepository.Update(entity);
            _blogRepository.SaveChangesAsync();
            return true;
        }

    }
}
