using ApiRechargement.Web.ViewModels.Agency;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiRechargement.Web.Services.Interfaces
{
    public interface IAgencyService
    {
        Task<IEnumerable<AgencyViewModel>> GetAgencies();

        Task<AgencyViewModel> GetAgency(int id);

        Task<bool> AddAgency(AgencyViewModel agency);

        Task<bool> UpdateAgency(AgencyViewModel agency);

        Task DeleteAgency(int id);
    }
}
