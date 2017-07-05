using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenGameList.Data;
using OpenGameList.Data.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameList.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        #region Common Fields
        protected ApplicationDbContext DbContext;
        protected SignInManager<ApplicationUser> SignInManager;
        protected UserManager<ApplicationUser> UserManager;
        #endregion

        #region Constructor
        public BaseController(ApplicationDbContext context,
            SignInManager<ApplicationUser> SignInManager,
            UserManager<ApplicationUser> UserManager)
        {
            // DI
            this.DbContext = context;
            this.SignInManager = SignInManager;
            this.UserManager = UserManager;
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Retrieves the .NET Core Identity User Id
        /// for the current ClaimsPrincipal
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentUserId()
        {
            // If the user is not authenticated, throw an exception
            if (!User.Identity.IsAuthenticated)
                throw new NotSupportedException();

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                // Internal provider
                return User.FindFirst(ClaimTypes.NameIdentifier).Value;
            else
            {
                // External Provider
                var user = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null) throw new NotSupportedException();
                return user.Id;
            }
        }
        #endregion

        #region Common Properties
        /// <summary>
        /// Returns a suitable JsonSerializerSettings object that can be used to 
        /// generate the JsonResult return value for this controller's methods
        /// </summary>
        protected JsonSerializerSettings DefaultJsonSettings
        {
            get { return new JsonSerializerSettings() { Formatting = Formatting.Indented }; }
        }
        #endregion
    }
}
