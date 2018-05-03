using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
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
using System.Web;

namespace Web.Controllers
{
    [Authorize]
    public class GoogleDrivesController : Controller
    {
        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            List<Document> documents = new List<Document>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                documents = await context.Documents.ToListAsync();
            }
            ViewBag.Message = "FILE COUNT IS: " + documents.Count();
            return View("Files", documents.OrderByDescending(t => t.CreatedDate));
            
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


        #region Download from GoogleDrive
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
        #endregion

        #region Upload to GoogleDrive

        #endregion

        
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        public async Task<ActionResult> UploadGoogleFileAsync(HttpPostedFileBase file, Document document, CancellationToken cancellationToken)
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

                if(file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = System.IO.Path.GetFileName(file.FileName);
                        var path = System.IO.Path.Combine(Server.MapPath("~/App_Data/Uploads/GoogleDrives"), fileName);
                        file.SaveAs(path);

                        File _file = new File();
                        _file.Title = file.FileName;
                        _file.Description = "From WorkCard.vn; ";
                        _file.MimeType = GetMimeType(file.FileName);
                        _file.CanComment = true;
                        _file.Shared = true;
                        _file.Shareable = true;
                        

                        using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                        {
                            try
                            {
                                FilesResource.InsertMediaUpload request = service.Files.Insert(_file, stream, _file.MimeType);
                                await request.UploadAsync();
                                if (request.ResponseBody != null)
                                {
                                    using (ApplicationDbContext context = new ApplicationDbContext())
                                    {
                                        document.Id = Guid.NewGuid();
                                        document.Load(request.ResponseBody);
                                        document.CreatedBy = User.Identity.Name;
                                        document.CreatedDate = DateTime.Now;

                                        context.Documents.Add(document);
                                        await context.SaveChangesAsync();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("An error occurred: " + e.Message);
                            }
                        }
                    }
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView();
                    }
                }
                
                return RedirectToAction("Details","Articles", new { id = document.ArticleId });
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

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
                    if(!_item.Description.Contains("WorkCard.vn"))
                    {
                        _item.Description += "WorkCard.vn";
                    }
                    _item.Shared = true;
                    _item.CanComment = true;
                    _item.Capabilities.CanDownload = true;
                    _item.Capabilities.CanShare = true;
                    FilesResource.UpdateRequest request = service.Files.Update(_item,_item.Id);
                    request.NewRevision = true;
                    await request.ExecuteAsync();
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