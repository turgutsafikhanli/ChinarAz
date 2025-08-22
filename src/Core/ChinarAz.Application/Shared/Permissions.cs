namespace ChinarAz.Application.Shared;

public static class Permissions
{
    public static class Category
    {
        public const string Create = "Category.Create";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";
        public const string Get = "Category.Get";

        public static List<string> All => new List<string>
        {
            Create,
            Update,
            Delete,
            Get
        };
    }
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
