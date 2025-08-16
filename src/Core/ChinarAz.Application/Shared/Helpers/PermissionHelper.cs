using System.Reflection;

namespace ChinarAz.Application.Shared.Helpers;

public static class PermissionHelper
{
    public static Dictionary<string, List<string>> GetAllPermissions()
    {
        var result = new Dictionary<string, List<string>>();
        var nestedTypes = typeof(Permissions).GetNestedTypes(BindingFlags.Public | BindingFlags.Static);

        foreach (var moduleType in nestedTypes)
        {
            var allProperty = moduleType.GetProperty("All", BindingFlags.Public | BindingFlags.Static);

            if (allProperty != null)
            {
                var permissions = allProperty.GetValue(null) as List<string>;
                if (permissions != null && permissions.Any())
                {
                    result.Add(moduleType.Name, permissions);
                }
            }
        }
        return result;
    }

    public static List<string> GetAllPermissionsList()
    {
        return GetAllPermissions()
            .SelectMany(x => x.Value)
            .ToList();
    }
}
