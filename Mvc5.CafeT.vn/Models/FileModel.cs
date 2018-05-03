using CafeT.BusinessObjects;
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Mvc5.CafeT.vn.Models
{
    public class FileModel : FileObject
    {
        public string Title { set; get; }
        public string Description { set; get; }
        public string Tags { set; get; }
        public int? CountDownloads { set; get; }
        public string SizeToString { set;get; }
        public ImageObject Image { set; get; }
        public string AvatarPath { set; get; }
        public string Path { set; get; }
        public string GDriveId { set; get; }
        public IList<string> GOwners { set; get; }
        public string DownloadUrl { set; get; }
        public int CountOfDownload { set; get; } = 0;
        public double Size { set; get; } = 0;


        [NotMapped]
        public File GFile { set; get; }

        public FileModel():base()
        {
        }

        public FileModel(File file):base()
        {
            Title = file.Title;
            Description = file.Description;
            DownloadUrl = file.DownloadUrl;
            GDriveId = file.Id;
            Size = double.Parse(file.FileSize.ToString());
            GFile = file;
            GOwners = file.OwnerNames;
        }

        public FileModel(string fullPath) : base(fullPath)
        {
            CountDownloads = 0;
            GetInfo();
        }

        public void GetInfo()
        {
            if(IsExits())
            {
                Title = FileName;
                SizeToString = base.GetSizeAsString();
            }
        }

        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? AnswerId { set; get; }

        public bool IsImage()
        {
            string[] _imgExts = new string[] { ".png", ".jpg"};
            string _ext = this.GetExtension();
            if (_imgExts.ToList().Contains(_ext)) return true;
            return false;
        }

        public bool IsZiped()
        {
            string[] _imgExts = new string[] { ".zip", ".rar", ".7zip" };
            string _ext = this.GetExtension();
            if (_imgExts.ToList().Contains(_ext)) return true;
            return false;
        }

        public void Load(File file)
        {
            Title = file.Title;
            Description = file.Description;
            DownloadUrl = file.DownloadUrl;
            GDriveId = file.Id;
            Size = double.Parse(file.FileSize.ToString());
            GFile = file;
            GOwners = file.OwnerNames;
        }
    }
}