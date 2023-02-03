using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FPhoto
{
    public interface IFPhotoService
    {
        //查询推荐图片
        List<Photo> GetFeaturePhotos();
    }
}
