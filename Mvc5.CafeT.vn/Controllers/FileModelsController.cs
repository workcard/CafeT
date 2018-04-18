using CafeT.Text;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Mvc5.CafeT.vn.Controllers
{
    public class FileModelsController : BaseController
    {
        private string UserName { set; get; } = "taipm.vn@gmail.com";

        public FileModelsController(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
        }

        //public ActionResult Index(int? page)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var files = service.GetFilesAsync(100).Result;
        //    files = files.Where(t => t.HasThumbnail.HasValue && t.HasThumbnail.Value)
        //        .Where(t => t.Name.Length > 10)
        //        .OrderByDescending(t => t.CreatedTime);

        //    List<FileModel> list = new List<FileModel>();
        //    if (files != null && files.Count() > 0)
        //    {
        //        foreach (var file in files)
        //        {
        //            FileModel fileModel = Mappers.Mappers.GoogleFileToModel(file);
        //            list.Add(fileModel);
        //        }
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Index", list.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS)));
        //    }
        //    return View("Index", list.ToPagedList(pageNumber: page ?? 1, pageSize: PAGE_ITEMS));
        //}

        

        //public ActionResult GetNewFiles(int? n)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var files = service.GetFilesAsync(n).Result.TakeMax(n);
        //    List<FileModel> list = new List<FileModel>();
        //    if (files != null && files.Count() > 0)
        //    {
        //        foreach (var file in files)
        //        {
        //            FileModel fileModel = Mappers.Mappers.GoogleFileToModel(file);
        //            list.Add(fileModel);
        //        }
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Files/_Files", list));
        //    }
        //    return View("Files/_Files", list);
        //}

        //public ActionResult GetGoogleDocument(int? n)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var files = service.GetFilesAsync(n).Result.TakeMax(n);
        //    var file = files.ToArray()[0];

        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Messages/_GoogleView", file));
        //    }
        //    return View("Messages/_GoogleView", file);
        //}

        //public ActionResult UploadToGoolgle(HttpPostedFileBase file)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    string path = Server.MapPath("~/Temp/Capture.PNG");
        //    var result = service.UploadAsync(path).Result;
        //    return View("Messages/_HtmlString", result.Name + "|" + result.OriginalFilename);
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FileModel fileModel)
        {
            if (ModelState.IsValid)
            {
                fileModel.Id = Guid.NewGuid();
                fileModel.CreatedBy = User.Identity.Name;
                if (_fileManager.Insert(fileModel))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(fileModel);
        }

        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public HttpResponseMessage UploadFileWithSplitsForFile(Guid? fileId)
        {
            string _baseFileName = string.Empty;
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using the original file.part name posted
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath(UploadFolder + User.Identity.Name.Replace("@", "_"));
                    if (!Directory.Exists(UploadPath))
                    {
                        Directory.CreateDirectory(UploadPath);
                    }

                    try
                    {
                        string path = Path.Combine(UploadPath, fileName);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                            if (!path.IsNullOrEmptyOrWhiteSpace())
                            {
                                FileModel _file = new FileModel(UploadFolder + User.Identity.Name.Replace("@", "_") + "/" + fileName);
                                _file.LastUpdatedDate = DateTime.Now;
                                _file.LastUpdatedBy = User.Identity.Name;
                                if (_fileManager.Update(_file))
                                {
                                    return new HttpResponseMessage()
                                    {
                                        StatusCode = System.Net.HttpStatusCode.OK,
                                        Content = new StringContent("File uploaded.")
                                    };
                                }
                            }
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return new HttpResponseMessage()
                        {
                            StatusCode = System.Net.HttpStatusCode.ExpectationFailed,
                            Content = new StringContent("Fail uploaded.")
                        };
                    }
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("File uploaded.")
            };
        }
        


        // generic file post method - use in MVC or WebAPI
        [HttpPost]
        [Authorize]
        public ActionResult UploadFileWithSplits(Guid? articleId, IEnumerable<HttpPostedFileBase> files)
        {
            int i = files.Count();

            for(int n = 0; n < i; n++)
            {
                var FileDataContent = Request.Files[n];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath(UploadFolder + User.Identity.Name.Replace("@","_"));
                    if(!Directory.Exists(UploadPath))
                    {
                        Directory.CreateDirectory(UploadPath);
                    }
                    
                    try
                    {
                        string path = Path.Combine(UploadPath, fileName);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }

                        if (!path.IsNullOrEmptyOrWhiteSpace())
                        {
                            // Once the file part is saved, see if we have enough to merge it
                            Shared.Utils UT = new Shared.Utils(path);
                            UT.TempFolder = UploadPath;
                            UT.SplitFile(path);
                            UT.MergeFile(path);
                            try
                            {
                                FileModel _file = new FileModel(path);
                                _file.CreatedBy = User.Identity.Name;
                                if (articleId.HasValue)
                                {
                                    _file.ArticleId = articleId.Value;
                                    _file.CreatedBy = User.Identity.Name;
                                    if (!_articleManager.AddFile(articleId.Value, _file))
                                    {
                                        //System.IO.File.Delete(path);
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine();
                            }
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return View("Errors");
                    }
                }
            }
            if(Request.IsAjaxRequest())
            {
                return PartialView("_UploadCompleted");
            }
            return View("_UploadCompleted");
        }

        //[HttpPost]
        //public ActionResult DownloadFromGoogle(string id)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var _file = service.GetFileAsync(id).Result;
        //    string saveTo = @"C:\FilesFromGoogle\" + _file.Name;
        //    var result = service.DownloadFileAsync(_file, saveTo);
        //    if(Request.IsAjaxRequest())
        //    {
        //        return PartialView("Messages/_HtmlString", result.Result);
        //    }
        //    return View("Messages/_HtmlString", result.Result);
        //}
        //[HttpPost]
        //public ActionResult GetFileFromGoogle(string id)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var _file = service.GetFileAsync(id).Result;
        //    string saveTo = @"C:\FilesFromGoogle\" + _file.Name;
        //    var result = service.DownloadFileAsync(_file, saveTo);
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("Messages/_HtmlString", result.Result);
        //    }
        //    return View("Messages/_HtmlString", result.Result);
        //}

        //[HttpPost]
        //public async Task<ActionResult> UpdateGoogleFileAsync(string id)
        //{
        //    Uploader service = new Uploader(UserName);
        //    service.ClientSecrectFile = Server.MapPath("~/App_Code/client_secrets.json");
        //    var _file = await service.GetFileAsync(id);
            
        //    var file = await service.UpdateAsync(_file);
        //    var _last = await service.GetFileAsync(id);
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("Messages/_HtmlString", _last.Description);
        //    }
        //    return View("Messages/_HtmlString", _last.Description);
        //}

        //public FileResult Download(Guid id)
        //{
        //    var _object = _fileManager.GetById(id);

        //    if (_object != null)
        //    {
        //        try
        //        {
        //            if (_object.CountDownloads == null) _object.CountDownloads = 0;
        //            _object.CountDownloads = _object.CountDownloads + 1;
        //            if(_fileManager.Update(_object))
        //            {
        //                //string _path = Server.MapPath("~/" + _object.FullPath);
        //                string _path = Path.Combine("~/", _object.FullPath);
        //                if (System.IO.File.Exists(_path))
        //                {
        //                    return File(_path, MimeMapping.GetMimeMapping(_path), _object.FileName);
        //                }
        //            }
        //            return null;
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public FileResult DownloadFile(string fileName)
        //{
        //    string _path = Server.MapPath(UploadFolder + User.Identity.Name.Replace("@", "_"));
        //    var filepath = System.IO.Path.Combine(_path, fileName);
        //    if (System.IO.File.Exists(filepath))
        //    {
        //        return File(filepath, MimeMapping.GetMimeMapping(filepath), fileName);
        //    }
        //    return null;
        //}

        // GET: FileModels
        //public ActionResult Index(int? page, string searchString)
        //{
        //    var _models = _unitOfWorkAsync.Repository<FileModel>().Query().Select().Where(m => !string.IsNullOrWhiteSpace(m.Title))
        //        .OrderByDescending(t => t.CreatedDate).ToList();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        _models = _models.Where(s => (!string.IsNullOrEmpty(s.FileName) && s.FileName.Contains(searchString))
        //                                    || (!string.IsNullOrEmpty(s.Title) && s.Title.Contains(searchString))
        //                                    || (!string.IsNullOrEmpty(s.Description) && s.Description.Contains(searchString))
        //                                    || (!string.IsNullOrEmpty(s.FileName) && s.FileName.Contains(searchString)))
        //                        .ToList();
        //    }
        //    ViewBag.Keyword = searchString;
        //    ViewBag.CountFiles = _models.Count;
        //    ViewBag.TopDownloads = _fileManager.GetTopDownloads(10);
        //    ViewBag.News = _fileManager.GetNews(10);

        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("_Files", _models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
        //    }
        //    return (View(_models.ToPagedList(pageNumber: page ?? 1, pageSize: 10)));
        //}

        //public ActionResult GetNews(int n)
        //{
        //    var _newFiles = _fileManager.GetNews(n);
        //    if (Request.IsAjaxRequest())
        //    {
        //        return (PartialView("Files/_Files", _newFiles));
        //    }
        //    return View("Files/_Files", _newFiles);
        //}

        // GET: FileModels/Details/5
        public ActionResult Details(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            if(_object != null)
            {
                _object.CountViews = _object.CountViews + 1;
                if (_fileManager.Update(_object))
                {
                    ViewBag.TopDownloads = _fileManager.GetTopDownloads(10);
                    
                    return View(_object);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: FileModels/Edit/5
        [Authorize]
        public ActionResult Edit(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<FileModel>().Find(id);
            if(_object != null)
            {
                _object.Title = _object.GetFileName();
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: FileModels/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FileModel model, string imageBase)
        {
            try
            {
                // TODO: Add update logic here
                model.LastUpdatedBy = User.Identity.Name;
                model.LastUpdatedDate = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(imageBase))
                {
                    ////var image = ImageUtility.Base64ToImage(imageBase.Replace("data:image/png;base64,", ""));
                    //string folder = "/Profiles/Uploads/" + User.Identity.Name.Replace("@", "_") + "/Books/";
                    //string urlImage = folder + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".png";
                    //if (!Directory.Exists(Server.MapPath("~" + folder)))
                    //    Directory.CreateDirectory(Server.MapPath("~" + folder));
                    //image.Save(Server.MapPath("~" + urlImage));

                    //model.AvatarPath = urlImage;
                }
                if (_fileManager.Update(model))
                {
                    if(Request.IsAjaxRequest())
                    {
                        return PartialView("_UpdatedMessage");
                    }
                    return RedirectToAction("Details", new { id = model.Id });
                }
                return View("_FailedUpdateMessage","Có lỗi khi sửa tài liệu này. Vui lòng kiểm tra lại");
            }
            catch
            {
                return View();
            }
        }

        // GET: FileModels/Delete/5
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            var _object = _fileManager.GetById(id);
            if (_object != null)
            {
                return View(_object);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: FileModels/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FileModel model)
        {
            try
            {
                // TODO: Add delete logic here
                var _object = _fileManager.GetById(model.Id);
                if(System.IO.File.Exists(_object.FullPath))
                {
                    try
                    {
                        System.IO.File.Delete(_object.FullPath);
                        if(_fileManager.Delete(_object.Id))
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return View();
                    }
                }
                else
                {
                    if (_fileManager.Delete(_object.Id))
                    {
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
