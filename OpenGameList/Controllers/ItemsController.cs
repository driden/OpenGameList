using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGameList.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        #region Attribute based routes
        
        /// <summary>
        /// GET api/items/GetLatest/5
        /// </summary>
        /// <param name="num">Amount of latest items to fetch</param>
        /// <returns></returns>
        [HttpGet("GetLatest/{num}")]
        public IActionResult GetLatest(int num)
        {
            var items = GetSampleItems().OrderByDescending(x => x.CreatedDate).Take(num);
            return new JsonResult(items, DefaultJsonSettings);
        }

        /// <summary>
        /// GET: api/items/GetMosViewed/{num}
        /// </summary>
        /// <param name="num"></param>
        /// <returns>An array of {num} Json-serialized objects representing the items with the most views</returns>        
        [HttpGet("api/items/GetMostViewed/{num}")]
        public IActionResult GetMostViewed(int num)
        {
            var mostViewed = GetSampleItems().OrderByDescending(x => x.ViewCount).Take(num);
            return new JsonResult(mostViewed, DefaultJsonSettings);
        }

        [HttpGet("api/items/GetRandom/{num}")]
        public IActionResult GetRandom(int num)
        {
            var rnd = GetSampleItems().OrderBy(x => Guid.NewGuid()).Take(num);
            return new JsonResult(rnd, DefaultJsonSettings);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Generate a sample array of source Items to emulate a database (for testing only)
        /// </summary>
        /// <param name="num">Number of items to generate</param>
        /// <returns>a defined number of mock items (for testing only)</returns>
        private List<ItemViewModel> GetSampleItems(int num = 999)
        {
            var lst = new List<ItemViewModel>();
            var date = new DateTime(2015,12,31).AddDays(-num);

            for ( int id = 1 ; id <= num ; id++ )
            {
                lst.Add(new ItemViewModel()
                {
                    Id = id,
                    Title = $"Item {id} Title",
                    Description = $"This is a Sample Description for item {id}:Lorem ipsum dolor sit amet.",
                    CreatedDate = date.AddDays(id),
                    LastModifiedDate = date.AddDays(id),
                    ViewCount = num - id
                });
            }
            return lst;
        }

        /// <summary>
        /// Returns a suitable JsonSerializerSettings object that can be used to 
        /// generate the JsonResult return value for this controller's methods
        /// </summary>
        private JsonSerializerSettings DefaultJsonSettings
        {
            get { return new JsonSerializerSettings() { Formatting = Formatting.Indented }; }
        }
        #endregion
    }
}