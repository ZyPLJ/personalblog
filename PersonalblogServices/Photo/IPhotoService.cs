using Microsoft.AspNetCore.Http;
using Personalblog.Model.Entitys;
using Personalblog.Model.Photography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace PersonalblogServices
{
    public interface IPhotoService
    {
        /// <summary>
        /// 添加图片接口
        /// </summary>
        /// <param name="photo"></param>
        void InsertPhoto(Photo photo);
        /// <summary>
        /// 查询所有图片
        /// </summary>
        /// <returns></returns>
        Task<List<Photo>> GetAllPhotos();
        /// <summary>
        /// 根据id查询单张图片详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Photo GetPhoto(string id);
        /// <summary>
        /// 随机获取一张图片
        /// </summary>
        /// <returns></returns>
        Photo GetRandomPhoto();
        IPagedList<Photo> GetPagedList(int page = 1, int pageSize = 10);
    }
}
