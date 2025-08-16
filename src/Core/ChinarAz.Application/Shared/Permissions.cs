using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinarAz.Application.Shared;

public static class Permissions
{
    public static class Role
    {
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string GetAllPermissions = "GetAllPermissions";



        public static List<string> All => new List<string>
        {
            Create,
            Update,
            Delete,
            GetAllPermissions
        };
    }
    public static class Account
    {
        public const string AddRole = "Account.AddRole";
        public const string Create = "Account.Create";
        public const string Update = "Account.Update";
        public const string Delete = "Account.Delete";




        public static List<string> All => new List<string>
        {
            AddRole,
            Create,
            Update,
            Delete
        };
    }
}
