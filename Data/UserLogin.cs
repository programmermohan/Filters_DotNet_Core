using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class UserLogin
    {
        public long Id { get; set; }

        [ForeignKey ("User")]
        public long UserId { get; set; }

        [MaxLength(250)]
        public string IPAddress { get; set; }
        public DateTime LoginDate { get; set; }
        public bool IsSuccess { get; set; }
        public User User { get; set; }
    }
}
