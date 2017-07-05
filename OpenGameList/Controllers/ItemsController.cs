using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nelibur.ObjectMapper;
using OpenGameList.Data;
using OpenGameList.Data.Items;
using OpenGameList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OpenGameList.Controllers
{

    public class ItemsController : BaseController
    {
        #region Constructor

        public ItemsController(ApplicationDbContext context)
        {
            base.DbContext = context;
        }

        #endregion Constructor

        #region Attribute based routes

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
            if (item != null)
                return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            else
                return NotFound(new { Error = $"Item ID {id} has not been found." });
        }

        #region CRUD Operations
        /// <summary>
        /// POST: api/items
        /// </summary>
        /// <param name="ivm">Item View Model</param>
        /// <returns>Creates a new Item, persists it, and returns it</returns>
        [HttpPost()]
        [Authorize]
        public IActionResult Add([FromBody]ItemViewModel ivm)
        {
            if (ivm != null)
            {
                // Create a new Item with the client-sent json data
                var item = TinyMapper.Map<Item>(ivm);
                // override any property that could be wise to set from server-side only
                item.CreatedDate = item.LastModifiedDate = DateTime.Now;

                item.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                // add the new item
                DbContext.Items.Add(item);

                // persist changes
                DbContext.SaveChanges();

                // return the new Item to the client
                return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
            }

            // if passed a null ivm
            return StatusCode(500);
        }

        /// <summary>
        /// PUT: api/items/{id}
        /// </summary>        
        /// <returns>Updates an existing Item and return it accordingly.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody]ItemViewModel ivm)
        {
            if (ivm != null)
            {
                var item = DbContext.Items.FirstOrDefault(i => i.Id == id);
                if (item != null)
                {
                    // handle the update (on per-property basis)
                    item.UserId = ivm.UserId;
                    item.Description = ivm.Description;
                    item.Flags = ivm.Flags;
                    item.Notes = ivm.Notes;
                    item.Text = ivm.Text;
                    item.Title = ivm.Title;
                    item.Type = ivm.Type;

                    // Override any property that could be wise to set from server-side only
                    item.LastModifiedDate = DateTime.Now;

                    // Persist changes to the DB
                    DbContext.SaveChanges();

                    //return the updated Item to the client
                    return new JsonResult(TinyMapper.Map<ItemViewModel>(item), DefaultJsonSettings);
                }

                // return an HTTP Status 404 (Not Found) if we couldn't find a suitable item.
                return NotFound(new { Error = $"Item ID {id} has not been found." });
            }
            return NotFound(new { Error = "Item's view model cannot be null" });
        }

        /// <summary>
        /// DELETE: api/items/{id}
        /// </summary>
        /// <returns>Deletes an Item, returning the HTTP staus 200 (OK) when done.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var item = DbContext.Items.FirstOrDefault(x => x.Id == id);

            if (item != null)
            {
                // Remove the item from the DbContext
                DbContext.Items.Remove(item);

                // Persist the changes to the DB
                DbContext.SaveChanges();

                // Return Http Status code of 200
                return new OkResult();
            }
            //return 404 status code
            return NotFound(new { Error = $"Couldn't find an item with ID {id}" });
        }

        #endregion CRUD Operations

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
            if (num > MaxNumberOfItems) num = MaxNumberOfItems;
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
            if (num > MaxNumberOfItems) num = MaxNumberOfItems;
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
            if (num > MaxNumberOfItems) num = MaxNumberOfItems;
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
            var date = new DateTime(2015, 12, 31).AddDays(-num);

            for (int id = 1; id <= num; id++)
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