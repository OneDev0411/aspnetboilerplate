using Abp.Modules.Core.Domain.Entities;

namespace Abp.Modules.Core.Entities.NHibernate.Mappings
{
    public class TenantMap : EntityMap<Tenant>
    {
        public TenantMap()
            : base("AbpTenants")
        {
            Map(x => x.CompanyName);
            Map(x => x.Subdomain);
        }
    }
}