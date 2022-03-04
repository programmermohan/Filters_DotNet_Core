using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore_Filters.Auth
{
    public class AuthorizedRole
    {
        public const string Administrator = "Administrator";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string AdministratorOrUser = Administrator + "," + User;
        public const string ManagerOrAdministrator = Administrator + "," + Manager;
    }
}
