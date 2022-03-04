using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class Role
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public bool IsActive { get; set; }
    }
}
