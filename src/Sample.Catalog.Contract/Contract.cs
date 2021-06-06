using System;
namespace Sample.Catalog.Contract
{
    public class CatalogItemCreate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
    public class CatalogItemUpdated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
    public class CatalogItemDeleted
    {
        public Guid Id { get; set; }

    }
}