using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChuyenTin.vn.Mappers
{
    public static class Mapper
    {
        public static FileModel GoogleFileToModel(Google.Apis.Drive.v3.Data.File file)
        {
            FileModel fileModel = new FileModel();
            fileModel.Id = Guid.NewGuid();
            fileModel.FileName = file.Name;
            if (file.HasThumbnail.HasValue && file.HasThumbnail.Value)
                fileModel.AvatarPath = file.ThumbnailLink;
            fileModel.FullPath = file.Id;
            fileModel.SizeInB = file.Size.Value;
            fileModel.Description = file.Description;
            fileModel.CreatedBy = file.Owners[0].DisplayName;
            return fileModel;
        }
    }
}