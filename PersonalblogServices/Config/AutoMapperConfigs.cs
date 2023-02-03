using AutoMapper;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using PersonalblogServices.Articels.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
