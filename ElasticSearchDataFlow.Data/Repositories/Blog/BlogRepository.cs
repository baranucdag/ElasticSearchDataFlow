using ElasticSearchDataFlow.Data.Context;
using ElasticSearchDataFlow.Data.Entites;

namespace ElasticSearchDataFlow.Data.Repositories
{
    public class BlogRepository : EFGenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(DataContext context) : base(context)
        {
        }
    }
}
