using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Catalog.Service.Dtos;
using Sample.Catalog.Service.Entity;
using Sample.Common.Service.Repositories;
using System.Collections.Generic;
using MassTransit;
using Sample.Catalog.Contract;

namespace Sample.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Item> _service;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;
        public ItemController(IRepository<Item> service, IMapper mapper, IPublishEndpoint publishEndpoint)
         => (_service, _mapper, this.publishEndpoint) = (service, mapper, publishEndpoint);
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<GetItemDto>>(await _service.GetAllAsync()));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemDto itemDto)
        {
            var item = new Item
            {
                CreatedDate = DateTimeOffset.Now,
                Description = "for increasing the lifespan",
                Name = "HealtKit",
                Price = 7
            };
            await _service.CreateAsync(item);
            await publishEndpoint.Publish(new CatalogItemCreate
            {
                Description = item.Description,
                Id = item.Id,
                Name = item.Name
            });
            return Ok("item added successfully");
        }
    }
}
