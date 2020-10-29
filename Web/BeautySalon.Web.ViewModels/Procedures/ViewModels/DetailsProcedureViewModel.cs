namespace BeautySalon.Web.ViewModels.Procedures.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;

    public class DetailsProcedureViewModel : ProcedureViewModel, IMapFrom<Procedure>, IHaveCustomMappings
    {
        public string Description { get; set; }

        public string SkinTypeDescription { get; set; }

        public IEnumerable<SkinProblemProcedureViewModel> SkinProblems { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Procedure, DetailsProcedureViewModel>().ForMember(
                m => m.SkinProblems,
                opt => opt.MapFrom(x => x.SkinProblemProcedures.Select(y => new SkinProblemProcedureViewModel()
                {
                    Id = y.SkinProblem.Id,
                    Name = y.SkinProblem.Name,
                    Description = y.SkinProblem.Description,
                })));
        }
    }
}
