using ElasticSearchDataFlow.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ElasticSearchDataFlow.Data.Context
{
    public class DataContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }

        public DataContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        public DbSet<Blog> Blogs { get; set; }

    }
}
