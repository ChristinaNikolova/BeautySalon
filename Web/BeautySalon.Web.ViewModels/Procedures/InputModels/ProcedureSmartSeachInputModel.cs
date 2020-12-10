namespace BeautySalon.Web.ViewModels.Procedures.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class ProcedureSmartSeachInputModel
    {
        //test!!!!!!!!!!!!
        [Required]
        public string ClientSkinTypeId { get; set; }

        [Required]
        public string IsSkinSensitive { get; set; }

        [Required]
        public string StylistId { get; set; }
    }
}
