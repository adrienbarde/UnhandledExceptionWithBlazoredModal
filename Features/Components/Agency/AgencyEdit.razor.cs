using System.Threading.Tasks;
using ApiRechargement.Web.Common;
using ApiRechargement.Web.Services.Interfaces;
using ApiRechargement.Web.ViewModels.Agency;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace ApiRechargement.Web.Features.Components.Agency
{
    public partial class AgencyEdit : ComponentBase
    {
        [Inject]
        private IAgencyService _agencyService { get; set; }

        [Inject]
        private IToastService _toastService { get; set; }

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public int AgencyId { get; set; }

        public AgencyViewModel Agency { get; set; }
        public bool IsLoading { get; set; } = true;

        public AgencyEdit()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Agency = new AgencyViewModel();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AgencyId > 0)
            {
                Agency = await _agencyService.GetAgency(AgencyId);
            }
        }

        public async void HandleValidSubmit()
        {
                if (Agency.Id == 0 && await _agencyService.AddAgency(Agency))
                {
                    _toastService.ShowSuccess("Agence ajoutée avec succès");
                    await BlazoredModal.Close(ModalResult.Ok(Agency));
                }
                else if (Agency.Id > 0 && await _agencyService.UpdateAgency(Agency))
                {
                    _toastService.ShowSuccess("Agence enregistrée avec succès");
                    await BlazoredModal.Close(ModalResult.Ok(Agency));
                }
            
        }
    }
}
