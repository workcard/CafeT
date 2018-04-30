using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Controllers
{
    //internal class GoogleDrive
    //{

    //}
    public class GoogleDrive
    {
        DriveService service = null;
        public GoogleDrive()
        {
            Run().Wait();
        }


        private File GetFileByID(string fileID, DriveService service)
        {
            File file = service.Files.Get(fileID).Execute();
            if (file.ExplicitlyTrashed == null)
                return file;
            return null;
        }

        public string GetFolderID(string FolderName)
        {
            FilesResource.ListRequest request = service.Files.List();
            request.Q = "title = '" + FolderName + "'";
            FileList files = request.Execute();
            return files.Items[0].Id;
        }
        public List<File> ListFolderContent(string FolderID)
        {
            ChildrenResource.ListRequest request = service.Children.List(FolderID);
            List<File> files = new List<File>();
            //request.Q = "mimeType = 'image/jpeg'";

            request.Q = "'" + FolderID + "' in parents";

            do
            {
                try
                {
                    ChildList children = request.Execute();

                    foreach (ChildReference child in children.Items)
                    {
                        files.Add(GetFileByID(child.Id, service));
                    }

                    request.PageToken = children.NextPageToken;

                }
                catch (Exception e)
                {
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return files;
        }

        public List<File> ListRootFileFolders()
        {
            List<File> result = new List<File>();
            FilesResource.ListRequest request = service.Files.List();
            do
            {
                try
                {
                    FileList files = request.Execute();

                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return result;
        }

        public byte[] DownloadFile(string fileID)
        {
            File file = service.Files.Get(fileID).Execute();
            var bytes = service.HttpClient.GetByteArrayAsync(file.DownloadUrl);
            return bytes.Result;
        }


        private async Task Run()
        {
            try
            {
                GoogleWebAuthorizationBroker.Folder = "Files";
                UserCredential credential;

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = MyClientSecrets.ClientId,
                        ClientSecret = MyClientSecrets.ClientSecret
                    },
                    MyRequestedScopes.Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("Files"));
                
                var drvService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "WorkCard.vn",

                });

                if (drvService != null)
                {
                    service = drvService;
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}