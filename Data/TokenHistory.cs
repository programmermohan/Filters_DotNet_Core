using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Data
{
    public class TokenHistory
    {
        public long Id { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        [MaxLength(2500)]
        public string AccessToken { get; set; }

        [MaxLength(2500)]
        public string RefreshToken { get; set; }

        [MaxLength(250)]
        public string IPAddress { get; set; }
        public DateTime LoginDtTm { get; set; }
        public DateTime ModifyTokenDtTm { get; set; }
        public bool IsAcess_TokenAlive { get; set; }
        public bool IsRefreshToken_Alive { get; set; }
        public bool IsActive { get; set; }
        public DateTime AccessToken_Expiry { get; set; }
        public DateTime RefreshToken_Expiry { get; set; }
        public User User { get; set; }
    }
}
