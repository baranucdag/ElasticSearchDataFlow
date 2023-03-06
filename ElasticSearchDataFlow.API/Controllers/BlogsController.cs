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

        [HttpGet("GetBlogsFromElastic")]
        public async Task<IActionResult> GetBlogs()
        {
            var result = _elasticSearchService.GetAllDocuments<Blog>(new SearchParameters { IndexName = "blogs", From = 0, Size = 100 });
            return Ok(result.Result);
        }

        [HttpGet("GetBlogsFromDb")]
        public async Task<IActionResult> GetBlogsFromDb()
        {
            var result = _blogService.GetBlogList(0, 100); ;
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert()
        {
            //only single data should be insert
            var dataToInsert = _blogService.GetBlogList(0, 100);
            await _elasticSearchService.InsertAsync<Blog>("blogs", dataToInsert);
            return Ok();
        }

        [HttpPost("InsertMany")]
        public async Task<IActionResult> InsertMany()
        {
            var dataToInsert = _blogService.GetBlogList(0, 100);
            await _elasticSearchService.InsertBulkAsync("blogs", dataToInsert.ToArray());
            return Ok();
        }

        [HttpPost("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] string id)
        {
            _blogService.DeleteById(id);
            await _elasticSearchService.DeleteByElasticIdAsync(new ElasticSearchModel { ElasticId = 1, IndexName = "blogs" });
            return Ok();
        }
    }
}
