
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Web.Models;

namespace Mvc5.CafeT.vn.Models
{
    public enum FileType
    {
        Avatar = 1,
        Pdf = 2,
        Zip = 3,
        Rar = 4,
        Word = 5,
        Photo
    }

    public class FileModel : BaseObject
    {
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType FileType { get; set; }

        [NotMapped]
        public FileInfo Info { set; get; }
        public string FullPath { set; get; }
        public string FileName { get; set; }

        public string Root { get; set; }
        public string Folder { get; set; }
        public string Extension { set; get; }

        public long SizeInB { set; get; }
        public long SizeInKB { set; get; }
        public long SizeInMb { set; get; }
        public long SizeInGb { set; get; }

        public FileModel() : base() { }

        public FileModel(string fullPath):base()
        {
            FullPath = fullPath;
            if (!IsExits()) return;
            else
            {
                Info = new FileInfo(FullPath);
                //FileName = GetFileName();
                Folder = GetFolder();
                //Extension = GetExtension();
                //Root = GetRoot();
                SizeInB = GetSize();
                SizeInKB = SizeInB / 1024;
                SizeInMb = SizeInKB / 1024;
                SizeInGb = SizeInMb / 1024;
            }
        }

        public string GetSizeAsString()
        {
            if (this.SizeInGb > 0) return this.SizeInGb.ToString() + "Gb";
            else if (this.SizeInMb > 0) return this.SizeInMb.ToString() + "Mb";
            else if (this.SizeInKB > 0) return this.SizeInKB.ToString() + "Kb";
            else if (this.SizeInB > 0) return this.SizeInB.ToString() + "B";
            else return string.Empty;
        }

        public bool IsExits()
        {
            return System.IO.File.Exists(FullPath);
        }

        public bool IsExits(string fullPath)
        {
            return System.IO.File.Exists(fullPath);
        }

        public long GetSize()
        {
            FileInfo _fi = new FileInfo(FullPath);
            return _fi.Length;
        }

        //public string GetRoot()
        //{
        //    string _extension = Path.GetPathRoot(FullPath);
        //    return _extension;
        //}

        //public string GetExtension()
        //{
        //    string _extension = Path.GetExtension(FullPath);
        //    return _extension;
        //}

        //public string GetFileName()
        //{

        //    string _filename = Path.GetFileName(FullPath);
        //    return _filename;
        //}

        public string GetFolder()
        {
            string _dirName = new DirectoryInfo(FullPath).Name;
            return _dirName;
        }

        public string Rename(string newFileName)
        {
            try
            {
                Info.MoveTo(newFileName);
                Info.Delete();
                FileName = newFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }

        //public void Zip()
        //{
        //    if (!IsExits()) return;

        //    using (ZipFile zip = new ZipFile())
        //    {
        //        // add this map file into the "images" directory in the zip archive
        //        //zip.AddFile(FullPath, Folder);

        //        // add the report into a different directory in the archive
        //        zip.AddFile(FullPath);
        //        //zip.AddFile("ReadMe.txt");
        //        zip.Save(FileName + ".zip");
        //    }
        //}

        public void ToHtml()
        {
            if ((Extension == ".doc") || (Extension == ".docx"))
            {
            }
            return;
        }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Tags { set; get; }
        public int? CountDownloads { set; get; }
        public string SizeToString { set;get; }
        //public ImageObject Image { set; get; }
        public string AvatarPath { set; get; }
        public string Path { set; get; }
        public string GDriveId { set; get; }
        public IList<string> GOwners { set; get; }
        public string DownloadUrl { set; get; }
        public int CountOfDownload { set; get; } = 0;
        public double Size { set; get; } = 0;
        public int CountViews { set; get; } = 0;

        [NotMapped]
        public Google.Apis.Drive.v2.Data.File GFile { set; get; }

        //public FileModel():base()
        //{
        //}

        public FileModel(Google.Apis.Drive.v2.Data.File file):base()
        {
            Title = file.Title;
            Description = file.Description;
            DownloadUrl = file.DownloadUrl;
            GDriveId = file.Id;
            Size = double.Parse(file.FileSize.ToString());
            GFile = file;
            GOwners = file.OwnerNames;
        }


        public Guid? ArticleId { set; get; }
        public Guid? ProjectId { set; get; }
        public Guid? CourseId { set; get; }
        public Guid? QuestionId { set; get; }
        public Guid? AnswerId { set; get; }

       
        public void Load(Google.Apis.Drive.v2.Data.File file)
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