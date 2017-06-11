using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nelibur.ObjectMapper;
using OpenGameList.Data;
using OpenGameList.Data.Items;
using OpenGameList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenGameList.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        #region Private fields

        private ApplicationDbContext DbContext;

        #endregion Private fields

        #region Constructor

        public ItemsController(ApplicationDbContext context)
        {
            // DI
            DbContext = context;
        }

        #endregion Constructor

        #region Attribute based routes

        #region RESTful Conventions

        /// <summary>
        /// GET: api/items
        /// </summary>
        /// <returns>Nothing: this method will raise a HttpNotFound HTTP exception since
        /// we're not supporting this API call.</returns>
        [HttpGet()]
        public IActionResult Get()
        {
            return NotFound(new { Error = "not found" });
        }

        /// <summary>
        /// GET: api/items/{id}
        /// ROUTING TYPE: attribute-based
        /// </summary>
        /// <param name="id">id of the item to get</param>
        /// <returns>A Json-serialized object representing a single item.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = DbContext.Items.FirstOrDefault(i => i.Id == id);
            return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
        }

        #endregion

        /// <summary>
        /// GET api/items/GetLatest
        /// ROUTING TYPE: attribute-based
        /// </summary>        
        /// <returns>An array of a default number of Json-serialized objects representing the last inserted items.</returns>
        [HttpGet("GetLatest")]
        public IActionResult GetLatest()
        {
            return GetLatest(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET api/items/GetLatest/5
        /// </summary>
        /// <param name="num">Amount of latest items to fetch</param>
        /// <returns>An array of {num} a default number of Json-serialized objects representing the last inserted items.</returns>
        [HttpGet("GetLatest/{num}")]
        public IActionResult GetLatest(int num)
        {
            if ( num > MaxNumberOfItems ) num = MaxNumberOfItems;
            var items = DbContext.Items.OrderByDescending(i => i.CreatedDate).Take(num).ToArray();
            return new JsonResult(ToItemViewModelList(items), DefaultJsonSettings);
        }

        /// <summary>
        /// GET: api/items/GetMosViewed
        /// </summary>
        /// <param name="num"></param>
        /// <returns>An array of DefaultNumberOfItems Json-serialized objects representing the items with the most views</returns>        
        [HttpGet("GetMostViewed/{num}")]
        public IActionResult GetMostViewed(int num)
        {
            if ( num > MaxNumberOfItems) num = MaxNumberOfItems;
            var mostViewed = DbContext.Items.OrderByDescending(x => x.ViewCount).Take(num).ToArray();
            return new JsonResult(ToItemViewModelList(mostViewed), DefaultJsonSettings);
        }

        /// <summary>
        /// GET: api/items/GetMosViewed/{num}
        /// </summary>
        /// <param name="num"></param>
        /// <returns>An array of DefaultNumberOfItems of Json-serialized objects representing the items with the most views</returns>        
        [HttpGet("GetMostViewed")]
        public IActionResult GetMostViewed()
        {
            return GetMostViewed(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET: GetRandom/{num}
        /// </summary>
        /// <param name="num">amount of records to fetch</param>
        /// <returns>An array of DefaultNumberOfItems Json-serialized objects representing random selected items</returns>
        [HttpGet("GetRandom")]
        public IActionResult GetRandom()
        {
            return GetRandom(DefaultNumberOfItems);
        }

        /// <summary>
        /// GET: GetRandom/{num}
        /// </summary>
        /// <param name="num">amount of records to fetch</param>
        /// <returns>An array of <paramref name="num"/> Json-serialized objects representing random selected items</returns>
        [HttpGet("GetRandom/{num}")]
        public IActionResult GetRandom(int num)
        {
            if ( num > MaxNumberOfItems) num = MaxNumberOfItems;
            var rnd = DbContext.Items.OrderBy(x => Guid.NewGuid()).Take(num).ToArray();
            return new JsonResult(ToItemViewModelList(rnd), DefaultJsonSettings);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Maps a collection of Item entities into a list of ItemViewModel objects
        /// </summary>
        /// <param name="items">An IEnumerable collection of Item entities</param>
        /// <returns>A mapped list of ItemViewModel objects</returns>
        private List<ItemViewModel> ToItemViewModelList(IEnumerable<Item> items)
        {
            var lst = new List<ItemViewModel>();

            foreach (var item in items)
                lst.Add(TinyMapper.Map<ItemViewModel>(item));

            return lst;
        }

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

        /// <summary>
        /// Returns the default number of items to retrieve when using parameterless
        /// overloads of the API methods retrieving item lists.
        /// </summary>
        private int DefaultNumberOfItems { get { return 5; } }

        /// <summary>
        /// Returns the maximum number of items to retrieve when using
        /// the API methods retrieving item lists.
        /// </summary>
        private int MaxNumberOfItems { get { return 100; } }
        #endregion
    }
}