using Core.ElasticSearch.Models;
using Microsoft.Extensions.Configuration;
using Nest;

namespace ElasticSearchDataFlow.Core.ElasticSearch
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly IConfiguration _configuration;

        public ElasticSearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _elasticClient = CreateInstance(); ;
        }
        private ElasticClient CreateInstance()
        {
            string host = _configuration.GetSection("ElasticsearchServer:Host").Value;
            string port = _configuration.GetSection("ElasticsearchServer:Port").Value;
            string username = _configuration.GetSection("ElasticsearchServer:Username").Value;
            string password = _configuration.GetSection("ElasticsearchServer:Password").Value;
            var settings = new ConnectionSettings(new Uri(host + ":" + port));
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                settings.BasicAuthentication(username, password);

            return new ElasticClient(settings);
        }


        public async Task<ElasticSearchResult> CreateNewIndex(IndexModel indexModel)
        {
            var any = await _elasticClient.Indices.ExistsAsync(indexModel.IndexName);
            if (any.Exists)
                return new ElasticSearchResult(false, "Index already exist");

            CreateIndexResponse? response = await _elasticClient.Indices.CreateAsync(indexModel.IndexName, se =>
                                           se.Settings(a => a.NumberOfReplicas(
                                                                 indexModel.NumberOfReplicas)
                                                             .NumberOfShards(
                                                                 indexModel.NumberOfShards))
                                             .Aliases(x => x.Alias(indexModel.AliasName)));

            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }


        public IEnumerable<IndexName> GetIndexList()
        {
            return _elasticClient.Indices.Get(new GetIndexRequest(Indices.All)).Indices.Keys;
        }


        public async Task<ElasticSearchResult> DeleteIndex(string indexName)
        {
            await _elasticClient.Indices.DeleteAsync(indexName);
            return new ElasticSearchResult(true);
        }


        public async Task<List<ElasticSearchGetModel<T>>> GetAllDocuments<T>(SearchParameters parameters) where T : class
        {
            Type type = typeof(T);
            ISearchResponse<T>? response = await _elasticClient.SearchAsync<T>(s => s
                                            .Index(Indices.Index(parameters.IndexName))
                                            .From(parameters.From)
                                            .Size(parameters.Size));

            List<ElasticSearchGetModel<T>> list = response.Hits.Select(x => new ElasticSearchGetModel<T>
            {
                ElasticId = x.Id,
                Item = x.Source
            }).ToList();

            return list;
        }

        public async Task<T> GetDocumentById<T>(ElasticSearchModel elasticSearchModel) where T : class
        {
            var result = await _elasticClient.GetAsync<T>(elasticSearchModel.ElasticId, q => q.Index(elasticSearchModel.IndexName));
            return result.Source;
        }

        public async Task<ElasticSearchResult> InsertAsync<T>(string indexName, object document)
        {
            var result = await _elasticClient.CreateAsync(document, x => x.Index(indexName));

            //todo: index zaten mevcutsa update gerçekleştirme işlemi yapılabilir.
            //if (result.ApiCall?.HttpStatusCode == 409)
            //    await _elasticClient.UpdateAsync(, a => a.Index(indexName).Doc(indexName));

            return new ElasticSearchResult(true);
        }

        public async Task<ElasticSearchResult> InsertBulkAsync(string indexName, object[] documents)
        {
            var result = await _elasticClient.BulkAsync(x => x.Index(indexName).IndexMany(documents));

            return new ElasticSearchResult(result.IsValid,
                        result.IsValid ? "Success" : result.ServerError.Error.Reason);
        }

        public async Task<ElasticSearchResult> UpdateByElasticIdAsync(Id elasticId, string indexName, object document)
        {
            UpdateResponse<object>? response =
                await _elasticClient.UpdateAsync<object>(elasticId, u => u.Index(indexName).Doc(document));
            return new ElasticSearchResult(
                response.IsValid,
                response.IsValid ? "Success" : response.ServerError.Error.Reason);
        }


        public async Task<ElasticSearchResult> DeleteByElasticIdAsync(ElasticSearchModel model)
        {
            var document = await _elasticClient.GetAsync<object>(model.ElasticId, q => q.Index(model.IndexName));
            if (document.Source != null)
            {
                var result = await _elasticClient.DeleteAsync<object>(model.ElasticId, x => x.Index(model.IndexName));

                return new ElasticSearchResult(
                result.IsValid,
                result.IsValid ? "Success" : result.ServerError.Error.Reason);

            }

            return new ElasticSearchResult(false, "There is no document with this id number !");
        }
    }
}

/*
 public async Task<List<Blog>> GetDocuments(string indexName)
        {
            #region Wildcard aradaki harfi kendi tamamlıyor            
            //var response = await _client.SearchAsync<Blog>(s => s
            //        .From(0)
            //        .Take(10)
            //        .Index(indexName)
            //        .Query(q => q
            //        .Bool(b => b
            //        .Should(m => m
            //        .Wildcard(w => w
            //        .Field("city")
            //        .Value("r*ze"))))));
            #endregion

            #region Fuzzy kelime kendi tamamlar parametrikde olabilir
            //var response = await _client.SearchAsync<Blog>(s => s
            //                  .Index(indexName)
            //                  .Query(q => q
            //        .Fuzzy(fz => fz.Field("city")
            //            .Value("anka").Fuzziness(Fuzziness.EditDistance(4))
            //        )
            //    ));
            //harflerin yer değiştirmesi
            //var response = await _client.SearchAsync<Blog>(s => s
            //                  .Index(indexName)
            //                  .Query(q => q
            //        .Fuzzy(fz => fz.Field("city")
            //            .Value("rie").Transpositions(true))
            //        ));
            #endregion

            #region MatchPhrasePrefix  aradaki harfi kendi tamamlıyor Wildcard göre performans olarak daha yüksek
            //var response = await _client.SearchAsync<Blog>(s => s
            //                    .Index(indexName)
            //                    .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.City).Query("iz").MaxExpansions(10)))
            //                   );
            #endregion

            #region MultiMatch çoklu  büyük küçük duyarlığı olmaz
            // MultiMatch
            //    var response = await _client.SearchAsync<Blog>(s => s
            //                   .Index(indexName)
            //                   .Query(q => q
            //.MultiMatch(mm => mm
            //    .Fields(f => f
            //        .Field(ff => ff.City)
            //        .Field(ff => ff.Region)
            //    )
            //    .Type(TextQueryType.PhrasePrefix)
            //    .Query("iz")
            //    .MaxExpansions(10)
            //)));
            #endregion

            #region Term burada tamamı küçük harf olmalı
            //var response = await _client.SearchAsync<Blog>(s => s
            //                    .Index(indexName)
            //                  .Size(10000)
            //                   .Query(query => query.Term(f => f.City, "rize"))
            //                   );
            #endregion

            #region Match büyük küçük duyarlığı olmaz
            //var response = await _client.SearchAsync<Blog>(s => s
            //                      .Index(indexName)
            //                    .Size(10000)
            //                    .Query(q => q
            //                    .Match(m => m.Field("city").Query("ankara")
            //                     )));
            #endregion

            #region AnalyzeWildcard like sorgusu mantıgında çalışmakta
            var response = await _elasticClient.SearchAsync<Blog>(s => s
                                  .Index(indexName)
                                        .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query("*" + "iz" + "*")
                                   .Fields(fs => fs
                                       .Fields(f1 => f1.BlogContent
                                               )

                                ))));
            #endregion         

            return response.Documents.ToList();
        }
 */