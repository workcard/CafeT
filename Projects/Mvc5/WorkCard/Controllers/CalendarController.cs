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
                        projects.Add(new Document() { Id = Guid.NewGuid(), Title = item.Title,Path = item.DownloadUrl });
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
    }
}