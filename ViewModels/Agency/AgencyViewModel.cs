using ApiRechargement.Web.Dto;
using System.ComponentModel.DataAnnotations;

namespace ApiRechargement.Web.ViewModels.Agency
{
    public class AgencyViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Region { get; set; } = string.Empty;
        [Required]
        public int Credit { get; set; }

        public AgencyViewModel() { }

        public AgencyViewModel(AgencyDto agency)
        {
            Id = agency.Id;
            Code = agency.Code;
            Name = agency.Name;
            Region = agency.Region;
            Credit = agency.Credit;
        }
    }
}
