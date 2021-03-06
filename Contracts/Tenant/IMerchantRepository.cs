using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Tenant
{
    public interface IMerchantRepository
    {
      Task<IEnumerable<Merchant>> GetTenantList(int settlementType,string account,string nickname,string qqNumber,int state);
    }
}
