using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SqlDistributedCache.API.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using SqlDistributedCache.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace SqlDistributedCache.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GadgetController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly MyWorldDbContext _myWorldDbContext;
        public GadgetController(
            IDistributedCache distributedCache,
            MyWorldDbContext myWorldDbContext)
        {
            _distributedCache = distributedCache;
            _myWorldDbContext = myWorldDbContext;
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cachedGadgets = await _distributedCache.GetStringAsync("myGadgets");
            if (!string.IsNullOrEmpty(cachedGadgets))
            {
                List<Gadgets> result = JsonConvert.DeserializeObject<List<Gadgets>>(cachedGadgets);
                return Ok(new {IsCache = true,result});
            }
            else
            {
                List<Gadgets> result = await _myWorldDbContext.Gadgets.ToListAsync();
                cachedGadgets = JsonConvert.SerializeObject(result);
                await _distributedCache.SetStringAsync("myGadgets", cachedGadgets, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });
                return Ok(new {IsCache = false,result});
            }
        }

    }
}