using System.Collections.Generic;
using System.Threading.Tasks;
using ApiRechargement.Web.Common;
using ApiRechargement.Web.Features.Components.Agency;
using ApiRechargement.Web.Pages.Base;
using ApiRechargement.Web.Services.Interfaces;
using ApiRechargement.Web.ViewModels.Agency;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace ApiRechargement.Web.Pages.Agency
{
    public partial class AgencyPageBase : PagedBasePage
    {
        [Inject]
        private IAgencyService _agencyService { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        public IEnumerable<AgencyViewModel> Agencies { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await RefreshList();
        }

        public async Task RefreshList()
        {
            IsLoading = true;
            Agencies = await _agencyService.GetAgencies();
            IsLoading = false;
        }

        public async Task PageChanged(int page)
        {
            _currentPage = page;
            await RefreshList();
        }

        public async Task Delete(int id)
        {
            if (!await _jsRuntime.Confirm($"êtes-vous sûr de vouloir l'agence #{id}"))
            {
                return;
            }

            await _agencyService.DeleteAgency(id);
            await RefreshList();
        }

        public async Task Edit(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(AgencyEdit.AgencyId), id);
            var editModal = Modal.Show<AgencyEdit>("Agence détail", parameters, new ModalOptions
            {
                Animation = ModalAnimation.FadeIn(1)
            });

            var result = await editModal.Result;
            if (!result.Cancelled)
            {
                await RefreshList();
            }
        }

        public override async Task SortByColumnAction()
        {
            await RefreshList();
        }
    }
}
