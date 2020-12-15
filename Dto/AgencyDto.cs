namespace ApiRechargement.Web.Dto
{
    public class AgencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int Credit { get; set; }
    }
}
