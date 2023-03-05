﻿namespace ElasticSearchDataFlow.Data.Entites
{
    public class Blog : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
