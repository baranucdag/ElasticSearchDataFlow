using Core.ElasticSearch.Models;
using ElasticSearchDataFlow.Core.ElasticSearch;
using ElasticSearchDataFlow.Data.Entites;
using ElasticSearchDataFlow.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchDataFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IBlogService _blogService;

        public BlogsController(IElasticSearchService elasticSearchService, IBlogService blogService)
        {
            _elasticSearchService = elasticSearchService;
            _blogService = blogService;
        }

        [HttpGet("GetBlogs")]
        public async Task<IActionResult> GetBlogs()
        {
            _blogService.AddBlog();
            return Ok();
        }

        [HttpGet("GetIndexList")]
        public async Task<IActionResult> GetIndexList()
        {
            var result = _elasticSearchService.GetIndexList();
            return Ok(result);
        }

        [HttpGet("GetAllDocuments")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var result = await _elasticSearchService.GetAllDocuments<Blog>(new SearchParameters { IndexName = "blogs", From = 0, Size = 10 });
            return Ok(result);
        }


        [HttpPost("Insert")]
        public async Task<IActionResult> Insert()
        {
            await _elasticSearchService.InsertAsync<Blog>("blogs", CreateBlog());
            return Ok();
        }

        [HttpPost("InsertMany")]
        public async Task<IActionResult> InsertMany()
        {
            await _elasticSearchService.InsertBulkAsync("blogs", CreateBlogs().ToArray());
            return Ok();
        }

        [HttpPost("DeleteById")]
        public async Task<IActionResult> DeleteById()
        {
            await _elasticSearchService.DeleteByElasticIdAsync(new ElasticSearchModel { ElasticId = 1, IndexName = "blogs" });
            return Ok();
        }

        [HttpPost("CreateIndex")]
        public async Task<IActionResult> CreateIndex()
        {
            var result = await _elasticSearchService.CreateNewIndex(new IndexModel { IndexName = "blogs", });
            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }

        [HttpPost("DeleteIndex")]
        public async Task<IActionResult> DeleteIndex()
        {
            var result = await _elasticSearchService.DeleteIndex("blogs");
            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }



        private List<Blog> CreateBlogs()
        {
            List<Blog> blogs = new(){
                new Blog
                {
                    Content= "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                    Title= "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir." ,
                    CategoryId=1,
                    CreatedAt= DateTime.Now,
                    Id=Guid.NewGuid(),
                },
                 new Blog
                 {
                    Content= "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                    Title= "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir." ,
                    CategoryId=1,
                    CreatedAt= DateTime.Now,
                    Id=Guid.NewGuid(),
                }
            };
            return blogs;
        }

        private Blog CreateBlog()
        {
            return new Blog
            {
                Content = "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                Title = "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir.",
                CategoryId = 1,
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
            };
        }
    }
}
