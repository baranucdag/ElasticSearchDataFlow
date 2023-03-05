using ElasticSearchDataFlow.Data.Entites;
using Nest;

namespace ElasticSearchDataFlow.Service.Mapping
{
    public static class Mapping
    {
        public static CreateIndexDescriptor BlogsMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<Blog>(m => m.Properties(p => p
                .Keyword(k => k.Name(n => n.Id))
                .Text(t => t.Name(n => n.Content))
                .Text(t => t.Name(n => n.Title))
                .Number(t => t.Name(n => n.CategoryId))
                .Date(t => t.Name(n => n.CreatedAt)))
            );
        }
    }

}