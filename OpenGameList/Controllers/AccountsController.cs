using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenGameList.Data;
using OpenGameList.Data.Users;
using OpenGameList.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenGameList.Controllers
{
    public class AccountsController : BaseController
    {
        #region Constructor
        public AccountsController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> SignInManager,
            UserManager<ApplicationUser> UserManager
            ) : base(context, SignInManager, UserManager)
        { }
        #endregion

        #region External Authentication Providers

        // GET: /api/Accounts/ExternalLogin
        [HttpGet("ExternalLogin/{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            switch (provider.ToLower())
            {
                case "facebook":
                case "google":
                    // case "twitter":
                    // Request a redirect to the external login priovider.
                    var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
                    var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                    return Challenge(properties, provider);
                default:
                    return BadRequest(new { Error = $"Provider {provider} is not supported" });
            }
        }

        // GET: /api/Accounts/ExternalLoginCallBack
        [HttpGet("ExternalLoginCallBack")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            try
            {
                // Check if the external Provider returned an error and act accordingly
                if (remoteError != null)
                {
                    throw new Exception(remoteError);
                }

                // Extract the login info obtrained from the External Provider
                ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    // if there's none, emit an error
                    throw new Exception("ERROR: No login info available.");
                }

                // Check if this user already registered himself with this external provider before
                var user = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                if (user == null)
                {
                    // If we reach this point, it means that this user never tried to log in
                    // using this external provider. However, it could have been used other providers
                    // and /or have a local account.
                    // We can find out if that's the case by looking for his e-mail address.

                    // Retrieve the 'emailaddress' claim
                    var emailKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                    var email = info.Principal.FindFirst(emailKey).Value;

                    // Lookup if there's an username with this e-mail address in the Db
                    user = await UserManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        // No user has been found: register a new user using the info retrieved from the provider
                        var now = DateTime.Now;

                        // Create a unique username using the 'nameidentifier' claim
                        var idKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

                        var username = $"{info.LoginProvider}{info.Principal.FindFirst(idKey).Value}";

                        user = new ApplicationUser
                        {
                            UserName = username,
                            Email = email,
                            CreatedDate = now,
                            LastModifiedDate = now
                        };

                        // Add the user to the Db with a random password
                        await UserManager.CreateAsync(user, "Pass4External");

                        // Assign the user to the 'Registered' role
                        await UserManager.AddToRoleAsync(user, "Registered");

                        // Remove Lockout and E-Mail confirmation
                        user.EmailConfirmed = true;
                        user.LockoutEnabled = false;
                    }

                    // Register this external provider to the user
                    await UserManager.AddLoginAsync(user, info);

                    // Persist everything to the DB
                    await DbContext.SaveChangesAsync();
                }

                // create the auth JSON object
                var auth = new
                {
                    type = "External",
                    providerName = info.LoginProvider
                };

                // output a <SCRIPT> tag to call a JS function registered into the parent global
                // window scope
                return Content(
                    @"<script type=""text/javascript"">
                    window.opener.externalProviderLogin(" +
                    JsonConvert.SerializeObject(auth) + ");"
                    + "window.close(); </script>", "text/html");
            }
            catch (Exception ex)
            {
                // return a http status 400 (Bad Request) to the client
                return BadRequest(new { Error = ex.Message });
                throw;
            }
        }

        // POST: /api/Accounts/Logout
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                SignInManager.SignOutAsync().Wait();
            }
            return Ok();
        }
        #endregion

        #region RESTful Conventions

        /// <summary>
        /// GET: api/accounts
        /// </summary>
        /// <returns>A Json-serialized object representing the current account.</returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var id = await GetCurrentUserId();
            var user = DbContext.Users.FirstOrDefault(x => id == x.Id);

            if (user != null)
                return new JsonResult(
                    new UserViewModel
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        DisplayName = user.DisplayName
                    }, DefaultJsonSettings);
            else
                return NotFound(new { Error = $"User ID {id} has not been found" });
        }

        /// <summary>
        /// GET: api/accounts/{id}
        /// ROUTING TYPE: attribute-based
        /// </summary>
        /// <returns>A Json-serialized object representing a single account.</returns>
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return BadRequest(new { Error = "not implemented(yet)." });
        }

        public async Task<IActionResult> Add([FromBody] UserViewModel uvm)
        {
            if (uvm != null)
            {
                try
                {
                    // Check if the Username/Email already exists
                    ApplicationUser user = await UserManager.FindByNameAsync(uvm.UserName);
                    if (user != null) throw new Exception("UserName already exists.");
                    user = await UserManager.FindByEmailAsync(uvm.Email);
                    if (user != null) throw new Exception("E-mail already exists.");
                    var now = DateTime.Now;

                    // Create a new User with the client-sent json data
                    user = new ApplicationUser
                    {
                        UserName = uvm.UserName,
                        Email = uvm.Email,
                        CreatedDate = now,
                        LastModifiedDate = now
                    };

                    // Add the user to the db with a random password
                    await UserManager.CreateAsync(user, uvm.Password);

                    // Assigned the user to the 'Registered' role
                    await UserManager.AddToRoleAsync(user, "Registered");

                    // Remove lockout and E-mail confirmation
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;

                    // Persist changes to the DB
                    DbContext.SaveChanges();

                    // return the newly-created User to the client
                    return new JsonResult(new UserViewModel
                    {
                        UserName = user.UserName,
                        DisplayName = user.DisplayName,
                        Email = user.Email
                    }, DefaultJsonSettings);
                }
                catch (Exception ex)
                {
                    //return error
                    return new JsonResult(new { error = ex.Message });
                }
            }

            // Return a generic HTTP status 500 if the client payload is invalid
            return new StatusCodeResult(500);
        }

        /// <summary>
        /// PUT: api/accounts/{id}
        /// </summary>
        /// <param name="uvm"></param>
        /// <returns>Updates current User and return it accodingly.</returns>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserViewModel uvm)
        {
            if (uvm != null)
            {
                try
                {
                    // Retrieve User
                    var id = await GetCurrentUserId();
                    ApplicationUser user = await UserManager.FindByIdAsync(id);
                    if (user == null) throw new Exception("User not found.");

                    // Check for current password
                    if (await UserManager.CheckPasswordAsync(user, uvm.Password))
                    {
                        // Current password ok, perform changes (if any)
                        bool hadChanges = false;

                        if (user.Email != uvm.Email)
                        {
                            // Check if the Email already exists
                            var user2 = await UserManager.FindByEmailAsync(uvm.Email);
                            if (user2 != null && user.Id != user2.Id)
                                throw new Exception("E-mail already exists");
                            else
                                await UserManager.SetEmailAsync(user, uvm.Email);
                        }

                        if (!string.IsNullOrEmpty(uvm.PasswordNew))
                        {
                            await UserManager.ChangePasswordAsync(user, uvm.Password, uvm.PasswordNew);
                            hadChanges = true;
                        }

                        if (user.DisplayName != uvm.DisplayName)
                        {
                            user.DisplayName = uvm.DisplayName;
                            hadChanges = true;
                        }

                        if (hadChanges)
                        {
                            // Persist changes to the db
                            user.LastModifiedDate = DateTime.Now;
                            DbContext.SaveChanges();
                        }

                        // Return the updated User to the client
                        return new JsonResult(
                            new UserViewModel
                            {
                                UserName = user.UserName,
                                DisplayName = user.DisplayName,
                                Email = user.Email
                            },
                            DefaultJsonSettings);
                    }
                    else throw new Exception("Old Password mismatch.");
                }
                catch (Exception e)
                {
                    //throw error
                    return new JsonResult(new { error = e.Message });
                }
            }

            // Return HTTP status 404 (Not Found) if we couldn't find a suitable item.
            return NotFound(new { error = "Current user has not been found" });

        }

        /// <summary>
        /// DELETE: api/accounts
        /// </summary>
        /// <returns>Deletes the current user, returning a HTTP 200 (ok) when done.</returns>
        [HttpDelete()]
        [Authorize]
        public IActionResult Delete()
        {
            return BadRequest(new { error = "not implemented (yet)." });
        }

        /// <summary>
        /// DELETE: api/accounts/{id}
        /// </summary>
        /// <param name="id">id of the user to delete</param>
        /// <returns>Deletes an User, returning an HTTP 200 status code when done.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(string id)
        {
            return BadRequest(new { error = "not implemented (yet)." });
        }
        #endregion
    }
}
