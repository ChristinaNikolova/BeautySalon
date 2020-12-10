namespace BeautySalon.Web.ViewModels.Stylists.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class SearchStylistCriteriaInputModel
    {
        [Required]
        public string CategoryId { get; set; }
    }
}
