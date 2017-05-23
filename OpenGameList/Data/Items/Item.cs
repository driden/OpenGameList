using System.ComponentModel.DataAnnotations;

namespace OpenGameList.Data.Items
{

    public class Item
    {
        #region Constructor
        public Item()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Required]
        public int Id { get; set; }
        #endregion  
    }


}
