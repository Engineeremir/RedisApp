using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisApp.Models;
using System.Text.Json.Serialization;

namespace RedisApp.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions distributedCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1)
            };
            
            Product product = new Product { Id = 1,Name="Table",Price = 300M };
            string jsonProduct = JsonConvert.SerializeObject(product);
            _distributedCache.SetString("product:1", jsonProduct, distributedCacheEntryOptions);
            return View();
        }

        public IActionResult Show()
        {
            var stringObject = _distributedCache.GetString("product:1");
            var deserializedObject = JsonConvert.DeserializeObject<Product>(stringObject);

            ViewBag.product = deserializedObject;

            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("product:1");
            return View();
        }
    }
}
