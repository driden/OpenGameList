using OpenGameList.Data.Comments;
using OpenGameList.Data.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenGameList.Data.Users
{
    public class ApplicationUser
    {
        public ApplicationUser()
        {

        }

        #region Properties

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Notes { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Flags { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        #endregion

        #region Related Properties

        /// <summary>
        /// A list of items wrote by this user: this property will be loaded on first use using EF's lazy-loading feature
        /// </summary>
        public virtual List<Item> Items { get; set; }

        /// <summary>
        /// A list of comments wrote by this user: this property will be loaded on first use using EF's lazy-loading feature
        /// </summary>
        public List<Comment> Comments { get; set; }
        #endregion
    }
}
