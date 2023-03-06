using Core.ElasticSearch.Models;
using ElasticSearchDataFlow.Core.ElasticSearch;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchDataFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticController : ControllerBase
    {
        private readonly IElasticSearchService _elasticService;

        public ElasticController(IElasticSearchService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpGet("GetIndexList")]
        public async Task<IActionResult> GetIndexList()
        {
            var result = _elasticService.GetIndexList();
            return Ok(result);
        }

        [HttpPost("DeleteById/{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            await _elasticService.DeleteByElasticIdAsync(new ElasticSearchModel { ElasticId = id, IndexName = "blogs" });
            return Ok();
        }

        [HttpPost("CreateIndex")]
        public async Task<IActionResult> CreateIndex([FromBody] string indexName)
        {
            var result = await _elasticService.CreateNewIndex(new IndexModel { IndexName = indexName, });
            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }

        [HttpPost("DeleteIndex")]
        public async Task<IActionResult> DeleteIndex([FromBody] string indexName)
        {
            var result = await _elasticService.DeleteIndex(indexName);
            if (!result.Success)
                return BadRequest(result);

            return Ok();
        }
    }
}
