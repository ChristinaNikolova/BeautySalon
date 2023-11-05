namespace BeautySalon.Web.ViewModels.Administration.Procedures.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Common;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class ManageProcedureProductsViewModel : IMapFrom<Procedure>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(DataValidation.ProductNameMaxLenght, ErrorMessage = ErrorMessages.InputModel, MinimumLength = DataValidation.ProductNameMinLenght)]
        public string ProductName { get; set; }

        public IEnumerable<ProcedureProductAdministrationViewModel> Products { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Procedure, ManageProcedureProductsViewModel>().ForMember(
                 m => m.Products,
                 opt => opt.MapFrom(x => x.ProcedureProducts.Select(y => new ProcedureProductAdministrationViewModel()
                 {
                     Id = y.Product.Id,
                     Name = y.Product.Name,
                 })));
        }
    }
}
