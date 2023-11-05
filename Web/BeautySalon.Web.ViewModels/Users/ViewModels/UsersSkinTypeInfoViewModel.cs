namespace BeautySalon.Web.ViewModels.Users.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.SkinProblems.ViewModels;

    public class UsersSkinTypeInfoViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string SkinTypeId { get; set; }

        public string SkinTypeName { get; set; }

        public bool IsSkinSensitive { get; set; }

        public string SkinSensitive
            => this.IsSkinSensitive ? "Yes" : "No";

        public IEnumerable<SkinProblemName> SkinProblemNames { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UsersSkinTypeInfoViewModel>().ForMember(
               m => m.SkinProblemNames,
               opt => opt.MapFrom(x => x.ClientSkinProblems.Select(y => new SkinProblemName()
               {
                   Name = y.SkinProblem.Name,
               })
               .OrderBy(y => y.Name)));
        }
    }
}
