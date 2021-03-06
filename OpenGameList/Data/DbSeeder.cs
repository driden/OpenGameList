﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using OpenGameList.Data.Comments;
using OpenGameList.Data.Items;
using OpenGameList.Data.Users;
using OpenIddict.Core;
using OpenIddict.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenGameList.Data
{
    public class DbSeeder
    {
        #region Private Members

        private ApplicationDbContext DbContext;
        private RoleManager<IdentityRole> RoleManager;
        private UserManager<ApplicationUser> UserManager;
        private IConfiguration Configuration;
        private OpenIddictApplicationManager<OpenIddictApplication> OpenIddictManager;

        #endregion Private Members

        #region Constructor

        public DbSeeder(ApplicationDbContext dbContext, 
            RoleManager<IdentityRole> RoleManager, 
            UserManager<ApplicationUser> UserManager, 
            IConfiguration Configuration, 
            OpenIddictApplicationManager<OpenIddictApplication> OpenIddictManager)
        {
            DbContext = dbContext;
            this.RoleManager = RoleManager;
            this.UserManager = UserManager;
            this.Configuration = Configuration;
            this.OpenIddictManager = OpenIddictManager;
        }

        #endregion Constructor

        #region Public Methods

        public async Task SeedAsync()
        {
            //Create the db if it doesn't exist
            DbContext.Database.EnsureCreated();

            if ((await OpenIddictManager.FindByClientIdAsync(Configuration["Authentication:OpenIddict:ClientId"], new CancellationToken())) == null)
            {
                CreateApplication().GetAwaiter().GetResult();
            }

            //Create Default users
            if (await DbContext.Users.CountAsync() == 0)
              await CreateUsersAsync();

            //Create default items (if there are none) and comments
            if (await DbContext.Items.CountAsync() == 0)
                CreateItems();
        }
        #endregion Public Methods

        #region Private Methods

        private async Task CreateApplication()
        {
            await OpenIddictManager.CreateAsync(new OpenIddictApplication
            {
                Id = Configuration["Authentication:OpenIddict:ApplicationId"],
                DisplayName = Configuration["Authentication:OpenIddict:DisplayName"],
                RedirectUri = Configuration["Authentication:OpenIddict:Authority"] + Configuration["Authentication:OpenIddict:TokenEndPoint"],
                LogoutRedirectUri = Configuration["Authentication:OpenIddict:Authority"],
                ClientId = Configuration["Authentication:OpenIddict:ClientId"],
                // ClientSecret = Crypto.HashPassword(Configuration["Authentication:OpenIddict:ClientSecret"]),
                Type = OpenIddictConstants.ClientTypes.Public
            }, new CancellationToken());
            //This is the new way of adding a client secret, although it won't work if the application is marked as public.
            //}, Configuration["Authentication:OpenIddict:ClientSecret"], new CancellationToken());
        }   

        #endregion

        #region Seed Methods

        private async Task CreateUsersAsync()
        {
            //local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            string role_Administrators = "Administrators";
            string role_Registered = "Registered";

            // Create roles (if they don't exist yet)
            if (!await RoleManager.RoleExistsAsync(role_Administrators))
                await RoleManager.CreateAsync(new IdentityRole(role_Administrators));

            if (!await RoleManager.RoleExistsAsync(role_Registered))
                await RoleManager.CreateAsync(new IdentityRole(role_Registered));

            //Create the "Admin" ApplicationUser account(if it doesn't exist already)
            var admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            //Insert Admin into the db and assign "Administrator" role to him
            await AddUserWithRoleAsync(role_Administrators, "Pass4Admin", admin);

#if DEBUG
            //Create some sample registered user accounts(if they don't exist already)
            var ryan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var solice = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var vodan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@opengamelist.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            //Insert sample accounts into the db
            await AddUserWithRoleAsync(role_Registered, "Pass4Ryan", ryan );
            await AddUserWithRoleAsync(role_Registered, "Pass4Solice", solice);
            await AddUserWithRoleAsync(role_Registered, "Pass4Vodan", vodan);

#endif
            await DbContext.SaveChangesAsync();
        }

        private async Task AddUserWithRoleAsync(string role, string password, ApplicationUser user)
        {
            if (await UserManager.FindByIdAsync(user.Id) == null)
            {
                await UserManager.CreateAsync(user, password);
                await UserManager.AddToRoleAsync(user, role);

                //Remove Lockout and E-mail confirmation
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
            }
        }

        private void CreateItems()
        {
            //local variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            var authorId = DbContext.Users.FirstOrDefault(u => u.UserName == "Admin").Id;

#if DEBUG
            var num = 1000; //num = amount of items
            for (int id = 1; id <= num; id++)
            {
                DbContext.Items.Add(GetSampleItem(id, authorId, num - id, new DateTime(2015, 12, 31).AddDays(-num)));
            }
#endif
            EntityEntry<Item> e1 = DbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Magarena",
                Description = "Single-player fantasy card game similar to Magic: The Gathering",
                Text = @"Loosely based on Magic: The Gathering, the game lets you play agains a computer opponent or another human being.
The game features a well-developed AI, an intuitive and clear interface and an enticing level of gameplay.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e2 = DbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Minetest",
                Description = "Open-Source alternative to Minecraft",
                Text = @"The Minetest gameplay is very similar to Minecraft's: you are playing in a 3D open world, where you can create and/or remove various types of blocks.
Minetest features both single-player and multi-player game modes.
It also has support for custom mods, additional texture packs and other custom/personalization options.
Minetest has been released in 2015 under the GNU Lesser Genereral Public License",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e3 = DbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Relic Hunters Zero",
                Description = "A free game about shooting evil space ducks with tiny, cute guns.",
                Text = @"Relic Hunters Zero is fast, tactical and also very smooth to play.
It also enables the users to look at the source code, so they can gent creative and keep this game alive, fun and free for years to come.
The game is also available on Steam.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e4 = DbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "SuperTux",
                Description = "A classic 2D jump and run, side-scrolling game similar to the Super Mario series.",
                Text = @"The game is currently under Milestone 3. The Milestone 2, which is currently out, features the following:
- a nearly completely rewritten game engine based on OpenGL, OpenAL, SDL2, ...
- support for translations
- in-game manager for downloadable add-ons and translations
- Bonus Island III, a for now unfinished Forest Island and the developement levels in Incubator Island
- a final Boss in Icy Island
- new and improved sountracks and sound effects
... and much more!
The game has been released unted the GNU GPL license.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 9602,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Item> e5 = DbContext.Items.Add(new Item()
            {
                UserId = authorId,
                Title = "Scrabble3D",
                Description = "a 3D-based revamp to the classic Scrabble game.",
                Text = @"Scrabble3D extends the gameplay of the classic game Scrabble by adding a new whole third dimension.
Other than playing left to right or top to bottom, you'll be able to place your tiles above or beyond other tiles.
Sicne the game features more fields, it also uses a larger letter set.
You can either play against the computer, players from your LAN or from the Internet.
The game also features a set of game servers where you can challenge players from all over the world and get ranked into an official, ELO-based rating/ladder system.",
                Notes = "This is a sample record created by the Code-First Configuration class",
                ViewCount = 6754,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });


            //Create default Comments (if there are none)
            if (DbContext.Comments.Count() == 0)
            {
                int numComments = 10; //comments per item
                for (int i = 1; i <= numComments; i++)
                    DbContext.Comments.Add(GetSampleComment(i, e1.Entity.Id, authorId, createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    DbContext.Comments.Add(GetSampleComment(i, e2.Entity.Id, authorId, createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    DbContext.Comments.Add(GetSampleComment(i, e3.Entity.Id, authorId, createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    DbContext.Comments.Add(GetSampleComment(i, e4.Entity.Id, authorId, createdDate.AddDays(i)));
                for (int i = 1; i <= numComments; i++)
                    DbContext.Comments.Add(GetSampleComment(i, e5.Entity.Id, authorId, createdDate.AddDays(i)));
            }

            DbContext.SaveChanges();
        }

        #endregion Seed Methods

        #region Utility Methods

        /// <summary>
        /// Generate a sample array of Comments (for testing purposes only)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="itemId"></param>
        /// <param name="authorId"></param>
        /// <param name="createdDate"></param>
        /// <returns></returns>
        private Comment GetSampleComment(int n, int itemId, string authorId, DateTime createdDate)
        {
            return new Comment()
            {
                ItemId = itemId,
                UserId = authorId,
                ParentId = null,
                Text = $"Sample comment #{n} for the item {itemId}",
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
        }


        /// <summary>
        /// Generate a sample item to populate the DB
        /// </summary>
        /// <param name="id">the item id</param>
        /// <param name="authorId">the author id</param>
        /// <param name="viewCount"></param>
        /// <param name="createdDate">the item CreatedDate</param>
        /// <returns></returns>
        private Item GetSampleItem(int id, string authorId, int viewCount, DateTime createdDate)
        {
            return new Item()
            {
                UserId = authorId,
                Title = $"Item {id} Title",
                Description = $"This is a sample description for item {id}: Lorem Ipsum dolor sit amet.",
                Notes = "This is a sample record created by the Code-First Configuration Class",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
        }

        #endregion Utility Methods



    }
}
