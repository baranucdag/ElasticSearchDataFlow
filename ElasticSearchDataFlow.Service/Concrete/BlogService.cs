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

        public List<Blog> GetBlogList()
        {
            return _blogRepository.GetAll().ToList();
        }


        public async void AddBlog()
        {
            List<Blog> datas = new List<Blog>();

            for (int x = 100000; x < 2500000; x++)
            {
                datas.Add(new Blog
                {
                    Content = $"{x} Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                    Title = $"{x} {x} Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                    CategoryId = 2,
                    CreatedAt = DateTime.Now.AddHours(x),
                    Id = Guid.NewGuid(),
                });
            }

            _blogRepository.AddRange(datas);
        }
    }
}
