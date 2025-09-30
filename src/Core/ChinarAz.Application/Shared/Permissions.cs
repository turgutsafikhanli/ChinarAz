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
    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Update = "Product.Update";
        public const string Delete = "Product.Delete";

        public static List<string> All => new List<string>
        {
            Create,
            Update,
            Delete
        };
    }
    public static class Order
    {
        public const string Create = "Order.Create";
        public const string GetAll = "Order.GetAll";
        public const string Update = "Order.Update";
        public const string Delete = "Order.Delete";
        public const string Get = "Order.Get";
        public const string GetByIdAdmin = "Order.GetByIdAdmin";
        public const string UpdateAdmin = "Order.UpdateAdmin";
        public const string DeleteAdmin = "Order.DeleteAdmin";
        public static List<string> All => new List<string>
        {
            Create,
            GetAll,
            Update,
            Delete,
            Get,
            GetByIdAdmin,
            UpdateAdmin,
            DeleteAdmin
        };
    }
    public static class Blog
    {
        public const string Create = "Blog.Create";
        public const string Update = "Blog.Update";
        public const string Delete = "Blog.Delete";
        public const string Get = "Blog.Get";
        public static List<string> All => new List<string>
        {
            Create,
            Update,
            Delete,
            Get
        };
    }
    public static class Bio
    {
        public const string Create = "Bio.Create";
        public const string Update = "Bio.Update";
        public const string Delete = "Bio.Delete";
        public const string Get = "Bio.Get";
        public static List<string> All => new List<string>
        {
            Update,
            Get
        };
    }
}
