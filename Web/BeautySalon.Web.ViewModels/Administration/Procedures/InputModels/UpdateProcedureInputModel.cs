namespace BeautySalon.Web.ViewModels.Administration.Procedures.InputModels
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class UpdateProcedureInputModel : AddProcedureInputModel, IMapFrom<Procedure>, IHaveCustomMappings
    {
        [Required]
        public string Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Procedure, UpdateProcedureInputModel>().ForMember(
               m => m.IsSensitive,
               opt => opt.MapFrom(x => x.IsSensitive == true ? "Yes" : "No"));
        }
    }
}
