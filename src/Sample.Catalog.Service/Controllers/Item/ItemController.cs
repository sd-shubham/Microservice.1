using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Catalog.Service.Dtos;
using Sample.Catalog.Service.Entity;
using Sample.Common.Service.Repositories;
using System.Collections.Generic;
namespace Sample.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IRepository<Item> _service;
        private readonly IMapper _mapper;
        public ItemController(IRepository<Item> service, IMapper mapper)
         => (_service, _mapper) = (service, mapper);
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<GetItemDto>>(await _service.GetAllAsync(x => x.Name == "shubham")));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemDto itemDto)
        {
            var item = new Item
            {
                CreatedDate = DateTimeOffset.Now,
                Description = "Test",
                Name = "shubham",
                Price = 100
            };
            await _service.CreateAsync(item);
            return Ok("item added successfully");
        }
    }
}
