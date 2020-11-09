namespace BeautySalon.Web.ViewModels.Administration.Stylists.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Administration.Procedures.InputModels;
    using BeautySalon.Web.ViewModels.Administration.Products.ViewModels;

    public class ManageStylistProceduresViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [StringLength(DataValidation.ProcedureNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProcedureNameMinLenght)]
        public string ProcedureName { get; set; }

        public IEnumerable<ProcedureStylistViewModel> Procedures { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ManageStylistProceduresViewModel>().ForMember(
                m => m.Procedures,
                opt => opt.MapFrom(x => x.StylistProcedures.Select(y => new ProcedureStylistViewModel()
                {
                    Id = y.Procedure.Id,
                    Name = y.Procedure.Name,
                })));
        }
    }
}
