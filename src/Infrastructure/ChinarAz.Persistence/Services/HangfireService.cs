namespace ChinarAz.Persistence.Service;

public class HangfireAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
{
    public bool Authorize(Hangfire.Dashboard.DashboardContext context)
    {
        // Burada admin yoxlaması et
        return true;
    }
}