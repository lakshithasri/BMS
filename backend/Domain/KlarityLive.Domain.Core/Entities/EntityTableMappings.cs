namespace KlarityLive.Domain.Core.Entities
{
    public static class EntityTableMappings
    {
        public static Dictionary<string, string> Mappings = new Dictionary<string, string>
        {
            { "User","[Admin].[User]"},
            { "Building","[BMS].[Building]"},
            { "Tenant","[BMS].[Tenant]"},
            { "Tenancy","[BMS].[Tenancy]"},
            { "Meter","[BMS].[Meter]"},
            { "TenantTenancy","[BMS].[TenantTenancy]"},
        };
    }
}
