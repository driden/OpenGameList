using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenGameList.Data;
using OpenGameList.Data.Users;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OpenGameList.Classes
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtProvider
    {
        #region Private Members

        private readonly RequestDelegate _next;

        private TimeSpan TokenExpiration;
        private SigningCredentials SigningCredentials;

        private ApplicationDbContext DbContext;
        private UserManager<ApplicationUser> UserManager;
        private SignInManager<ApplicationUser> SignInManager;

        #endregion Private Members

        #region Static Members
        private static readonly string PrivateKey = "private_key_1234567890";
        public static readonly SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(PrivateKey));
        public static readonly string Issuer = "OpenGameListWebApp";
        public static string TokenEndpoint = "/api/connect/token";
        #endregion Static Members

        #region Constructor

        public JwtProvider(RequestDelegate next,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _next = next;

            TokenExpiration = TimeSpan.FromMinutes(10);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            // With DI
            DbContext = dbContext;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        #region Public Methods

        public Task Invoke(HttpContext httpContext)
        {
            // Check if the request path matches our TokenEndpoint
            if (!httpContext.Request.Path.Equals(TokenEndpoint,StringComparison.Ordinal))
                return _next(httpContext);

            // Check if the current request is a valid POST with the appropiate content type
            // (application/x-www-form-urlencoded)
            if( httpContext.Request.Method.Equals("POST") && 
                httpContext.Request.HasFormContentType)
            {
                // OK: generate token and send it via a json-formatted string
                return CreateToken(httpContext);
            }
            else
            {
                // Not OK: output a 400 - Bad request HTTP error.
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad request.");
            }
        }

        private Task CreateToken(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtProviderExtensions
    {
        public static IApplicationBuilder UseJwtProvider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtProvider>();
        }
    }
}
