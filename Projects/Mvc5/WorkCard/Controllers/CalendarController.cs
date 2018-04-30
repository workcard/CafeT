﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
//using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Models;
using Web.Services;
using Google.Apis.Download;
using CafeT.Text;
using System.Data.Entity;
using Google.Apis.Drive.v2.Data;
//using System.IO;

namespace Web.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "WorkCard.vn"
                });

                var list = await service.Files.List().ExecuteAsync();
                List<Document> projects = new List<Document>();
                if (list.Items.Count > 0)
                {
                    foreach(var item in list.Items)
                    {
                        projects.Add(new Document()
                        {
                            Id = Guid.NewGuid(),
                            Title = item.Title,
                            DownloadUrl = item.DownloadUrl,
                            GDriveId = item.Id,
                            Description = item.Description,
                            //Size = double.Parse(item.FileSize.ToString()),
                            UpdatedBy = item.LastModifyingUserName
                    });
                    }
                }
                
                ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                return View("Files", projects);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        public async Task<ActionResult> GetFilesAsync(string keyword, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "WorkCard.vn"
                });

                var list = await service.Files.List().ExecuteAsync();
                var files = list.Items.Where(t => t.Title.ToLower().Contains(keyword.ToLower()));

                List<Document> docs = new List<Document>();
                if (files != null && files.Count() > 0)
                {
                    foreach (var item in list.Items)
                    {
                        Document doc = new Document();
                        doc.GDriveId = item.Id;
                        doc.Title = item.Title;
                        doc.Description = item.Description;
                        doc.Path = item.DownloadUrl;
                        docs.Add(doc);
                    }
                }
                ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
                return View("Files", docs);
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }



        /// Download a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/get
        /// 

        /// a Valid authenticated DriveService
        /// File resource of the file to download
        /// location of where to save the file including the file name to save it as.
        /// 
        public static Boolean DownloadFile(DriveService _service, File _fileResource, string _saveTo)
        {

            if (!String.IsNullOrEmpty(_fileResource.DownloadUrl))
            {
                try
                {
                    var x = _service.HttpClient.GetByteArrayAsync(_fileResource.DownloadUrl);
                    byte[] arrBytes = x.Result;
                    System.IO.File.WriteAllBytes(_saveTo, arrBytes);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return false;
                }
            }
            else
            {
                // The file doesn't have any content stored on Drive.
                return false;
            }
        }

        public async Task<ActionResult> DownloadFromGoogleAsync(string id, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "WorkCard.vn"
                });
                var list = await service.Files.List().ExecuteAsync();
                var _item = list.Items.Where(t => t.Id == id).FirstOrDefault();

                var request = service.Files.Get(id);


                if (_item != null)
                {
                    string saveTo = @"C:\FilesFromGoogle\" + _item.Title;
                    var _isDownloaded = DownloadFile(service, _item, saveTo);
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_NotifyMessage", "Downloaded");
                }
                return View("_NotifyMessage", "Downloaded");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        //private static void UploadFile(DriveService service)
        //{
        //    Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
        //    body.Title = "test upload";
        //    body.Description = "test upload";
        //    body.MimeType = "application/vnd.ms-excel";


        //    // File's content.
        //    byte[] byteArray = System.IO.File.ReadAllBytes("/Temp/testUploadExcel.xlsx");
        //    MemoryStream stream = new MemoryStream(byteArray);
        //    try
        //    {
        //        FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, "application/vnd.google-apps.spreadsheet");

        //        request.Upload();

        //        Google.Apis.Drive.v2.Data.File file = request.ResponseBody;

        //        // Uncomment the following line to print the File ID.
        //        // Console.WriteLine("File ID: " + file.Id);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }
        //}

        public async Task<ActionResult> UpdateGoogleFileAsync(string id, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                 AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "WorkCard.vn"
                });
                var list = await service.Files.List().ExecuteAsync();
                var _item = list.Items.Where(t => t.Id == id).FirstOrDefault();

                try
                {
                    _item.Description += "WorkCard.vn";
                    _item.Shared = true;
                    _item.CanComment = true;
                    _item.Capabilities.CanDownload = true;
                    _item.Capabilities.CanShare = true;
                    // Send the request to the API.
                    FilesResource.UpdateRequest request = service.Files.Update(_item,_item.Id);
                    request.NewRevision = true;
                    await request.ExecuteAsync();

                    //service.Files.Update(_item, _item.Id);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if(!_item.DownloadUrl.IsNullOrEmptyOrWhiteSpace())
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        var file = context.Documents.Where(t => t.GDriveId == id).FirstOrDefault();
                        file.DownloadUrl = _item.DownloadUrl;
                        context.Entry(file).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_NotifyMessage", "Downloaded");
                }
                return View("_NotifyMessage", "Downloaded");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
        //public async Task<ActionResult> Details(string id, CancellationToken cancellationToken)
        //{
        //    using(Ap)
        //    var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
        //         AuthorizeAsync(cancellationToken);

        //    if (result.Credential != null)
        //    {
        //        var service = new DriveService(new BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = result.Credential,
        //            ApplicationName = "WorkCard.vn"
        //        });
        //        var list = await service.Files.List().ExecuteAsync();
        //        var _item = list.Items.Where(t => t.Id == id).FirstOrDefault();

        //        try
        //        {
        //            _item.Description += "WorkCard.vn";
        //            _item.Shared = true;
        //            _item.CanComment = true;
        //            _item.Capabilities.CanDownload = true;

        //            // Send the request to the API.
        //            FilesResource.UpdateRequest request = service.Files.Update(_item, _item.Id);
        //            request.NewRevision = true;
        //            await request.ExecuteAsync();

        //            //service.Files.Update(_item, _item.Id);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }

        //        if (!_item.DownloadUrl.IsNullOrEmptyOrWhiteSpace())
        //        {
        //            using (ApplicationDbContext context = new ApplicationDbContext())
        //            {
        //                var file = context.Documents.Where(t => t.GDriveId == id).FirstOrDefault();
        //                file.DownloadUrl = _item.DownloadUrl;
        //                context.Entry(file).State = EntityState.Modified;
        //                await context.SaveChangesAsync();
        //            }
        //        }
        //        if (Request.IsAjaxRequest())
        //        {
        //            return PartialView("_NotifyMessage", "Downloaded");
        //        }
        //        return View("_NotifyMessage", "Downloaded");
        //    }
        //    else
        //    {
        //        return new RedirectResult(result.RedirectUri);
        //    }
        //}
    }
}