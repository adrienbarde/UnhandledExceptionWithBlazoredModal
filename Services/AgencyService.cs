using ApiRechargement.Web.Services.Interfaces;
using ApiRechargement.Web.ViewModels.Agency;
using Blazored.Toast.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiRechargement.Web.Services
{
    public class AgencyService : IAgencyService
    {
        private readonly IToastService _toastService;

        public AgencyService(IToastService toastService)
        {
            _toastService = toastService;
        }

        public async Task<AgencyViewModel> GetAgency(int id)
        {
            return new AgencyViewModel { Id = 1, Code = "code", Credit = 99, Name = "name", Region = "region" };
        }

        public async Task<IEnumerable<AgencyViewModel>> GetAgencies()
        {
            return new List<AgencyViewModel>
            {
                new AgencyViewModel { Id = 1, Code = "code", Credit = 99, Name = "name", Region = "region" },
                new AgencyViewModel { Id = 2, Code = "code2", Credit = 199, Name = "name2", Region = "region2" }
            };
        }

        public async Task<bool> AddAgency(AgencyViewModel agency)
        {
            return true;
        }

        public async Task<bool> UpdateAgency(AgencyViewModel agency)
        {
            //method called by component into the modal
            throw new Exception("error NOT catched");
        }

        public async Task DeleteAgency(int id)
        {
            //called by the page
            throw new Exception("error catched!");
        }
    }
}
