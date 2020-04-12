using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using System;
namespace SqlDistributedCache.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        public TestController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [Route("add-cache-no-time-options")]
        [HttpGet]
        public async Task<IActionResult> AddCacheNoTimeOptions()
        {
            string key = "test1";
            string value = "naveen";
            await _distributedCache.SetStringAsync(key, value);
            return Ok("success");
        }

        [Route("add-cache")]
        [HttpGet]
        public async Task<IActionResult> AddCache()
        {
            string key = "test2";
            string value = "Naveen Bommindi";
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(1),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _distributedCache.SetStringAsync(key, value, options);
            return Ok("success");
        }

        [Route("get-cache")]
        [HttpGet]
        public async Task<IActionResult> GetCache()
        {
            string name = await _distributedCache.GetStringAsync("test2");
            return Ok(name);
        }

        [Route("delete-cache")]
        [HttpGet]
        public async Task<IActionResult> DeleteCache(string key)
        {
            await _distributedCache.RemoveAsync(key);
            return Ok();
        }
    }
}