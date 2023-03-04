using ElasticSearchDataFlow.Data.Entites;
using Nest;

namespace ElasticSearchDataFlow.Data
{
    public static class Mapping
    {
        public static CreateIndexDescriptor BlogsMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Blog>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.BlogContent))
                .Text(t => t.Name(n => n.BlogTitle))
                .Number(t => t.Name(n => n.UserId))
                .Number(t => t.Name(n => n.CategoryId))
                .Date(t => t.Name(n => n.CreatedAt)))
            );
        }
    }

}