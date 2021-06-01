

using System;
using Sample.Common.Service.Entity;

namespace Sample.Catalog.Service.Entity
{
    public class Item : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
