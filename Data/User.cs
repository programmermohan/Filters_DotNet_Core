using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(20)]
        [Required (ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Username is required")]
        public string Password { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }

        public Role Role { get; set; }
    }
}
