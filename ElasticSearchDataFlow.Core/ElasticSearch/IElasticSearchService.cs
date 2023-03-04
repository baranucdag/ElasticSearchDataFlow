using Core.ElasticSearch.Models;
using Nest;

namespace ElasticSearchDataFlow.Core.ElasticSearch
{
    public interface IElasticSearchService
    {
        Task<ElasticSearchResult> CreateNewIndex(IndexModel indexModel);
        IReadOnlyDictionary<IndexName, IndexState> GetIndexList();
        Task<ElasticSearchResult> DeleteIndex(string indexName);
        Task<List<ElasticSearchGetModel<T>>> GetAllDocuments<T>(SearchParameters parameters) where T : class;
        Task<T> GetDocumentById<T>(ElasticSearchModel elasticSearchModel) where T : class;
        Task<ElasticSearchResult> InsertAsync<T>(string indexName, object document);
        Task<ElasticSearchResult> InsertBulkAsync(string indexName, object[] documents);
        Task<ElasticSearchResult> UpdateByElasticIdAsync(Id elasticId, string indexName, object document);
        Task<ElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model);


    }

}
