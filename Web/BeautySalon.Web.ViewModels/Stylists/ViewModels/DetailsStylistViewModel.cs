﻿namespace BeautySalon.Web.ViewModels.Stylists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BeautySalon.Data.Models;
    using BeautySalon.Services.Mapping;
    using BeautySalon.Web.ViewModels.Articles.ViewModel;

    public class DetailsStylistViewModel : StylistViewModel, IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public IEnumerable<ArticleStylistViewModel> Articles { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DetailsStylistViewModel>().ForMember(
               m => m.Articles,
               opt => opt.MapFrom(x => x.Articles.Select(y => new ArticleStylistViewModel()
               {
                   ArticleId = y.Id,
                   ArticleTitle = y.Title,
               })));
        }
    }
}
