using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using OpenGameList.Data;
using OpenGameList.Data.Users;
using System.Linq;
using OpenGameList.ViewModels;

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

        #endregion
    }
}
