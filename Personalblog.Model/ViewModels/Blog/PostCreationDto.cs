﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.ViewModels.Blog
{
    public class PostCreationDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 梗概
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 内容（markdown格式）
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
    }
}
