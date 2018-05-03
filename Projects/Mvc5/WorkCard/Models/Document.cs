using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    public class Document:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Path { set; get; }
        public string GDriveId { set; get; }
        public IList<string> GOwners { set; get; }
        public string DownloadUrl { set; get; }
        public int CountOfDownload { set; get; } = 0;
        public double Size { set; get; } = 0;

        public Guid? ArticleId { set; get; }
        public Guid? IssueId { set; get; }
        public Guid? JobId { set; get; }
        //public string ImageLink { set; get; }
        [NotMapped]
        public File GFile { set; get; }

        public Document() : base() { }
        public Document(File file):base()
        {
            Title = file.Title;
            Description = file.Description;
            DownloadUrl = file.DownloadUrl;
            GDriveId = file.Id;
            Size = double.Parse(file.FileSize.ToString());
            GFile = file;
            GOwners = file.OwnerNames;
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
            //if(file.HasThumbnail.HasValue)
            //{
            //    ImageLink = file.ThumbnailLink;
            //}
        }
    }
}