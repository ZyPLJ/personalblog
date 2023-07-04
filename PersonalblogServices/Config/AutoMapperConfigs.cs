using AutoMapper;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using PersonalblogServices.Articels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.LinkExchange;
using Personalblog.Model.ViewModels.Links;

namespace PersonalblogServices.Config
{
    public class AutoMapperConfigs:Profile
    {
        /// <summary>
        /// 在构造函数中设置映射关系
        /// </summary>
        public AutoMapperConfigs()
        {
            //从A 映射到 B
            CreateMap<Post, ArticelRes>();
            CreateMap<PostCreationDto, Post>();
            CreateMap<PostUpdateDto, Post>();
            CreateMap<LinkExchangeAddViewModel, LinkExchange>();
            CreateMap<LinkCreationDto, Link>();
            CreateMap<CategoryCreationDto, Category>();
        }
    }
}
