using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ChuyenTin.vn.Helpers
{

    public class GoogleDrive
    {
        //
        public string CurrentApiKey { set; get; } = "AIzaSyDUEP-mrAJY8qdVXG4EqICDo3BGkwvRscY"; //ChuyenTin.vn
        public string UserName { set; get; }
        public string CurrentApplicationName { set; get; } = string.Empty;
        protected DriveService Service;

        private const int KB = 0x400;
        private const int DownloadChunkSize = 256 * KB;

        // CHANGE THIS with full path to the file you want to upload
        private const string UploadFileName = @"FILE_TO_UPLOAD";

        // CHANGE THIS with a download directory
        private const string DownloadDirectoryName = @"DIRECTORY_FOR_DOWNLOADING";

        // CHANGE THIS if you upload a file type other than a jpg
        private const string ContentType = @"image/jpeg";

        /// <summary>The logger instance.</summary>
        //private static readonly ILogger Logger;

        /// <summary>The Drive API scopes.</summary>
        private static readonly string[] Scopes =
            new[] {
                DriveService.Scope.DriveFile,
                DriveService.Scope.Drive,
                DriveService.Scope.DriveMetadata,
                DriveService.Scope.DriveAppdata,
                DriveService.Scope.DriveMetadataReadonly,
                DriveService.Scope.DrivePhotosReadonly,
                DriveService.Scope.DriveReadonly,
            };

        UserCredential credential;
        protected string ClientSecrectFile { set; get; }

        public GoogleDrive(string appName, string client_secrets, string userName)
        {
            CurrentApplicationName = appName;
            ClientSecrectFile = client_secrets;
            UserName = userName;
        }
        public void AuthenticateByKey()
        {
            string apiKey = "AIzaSyDUEP-mrAJY8qdVXG4EqICDo3BGkwvRscY";
            CurrentApplicationName = "ChuyenTin";
            Service = new DriveService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "ChuyenTin"
            });
        }
        public async Task AuthenticationAsync()
        {
            //using (var stream =
            //        new System.IO.FileStream(ClientSecrectFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        UserName,
            //        CancellationToken.None);
            //}
            Service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = CurrentApplicationName
            });
        }

        public void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = 
                new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

        public async Task<File> GetFileAsync(string id)
        {
            await AuthenticationAsync();
            FilesResource.GetRequest request = Service.Files.Get(id);

            var model = request.Execute();

            return model;
        }

       
        public async Task<File> UpdateAsync(File file)
        {
            string result = string.Empty;
            await AuthenticationAsync();
            string Description = "This is automatic update description for file";
            file.Description = Description;

            File body = new File();
            body.Name = file.Name;
            body.Description = "File updated by Diamto Drive Sample";
            body.MimeType = GetMimeType(file.MimeType);
            body.Parents = file.Parents;

            FilesResource.UpdateRequest request = Service.Files.Update(body, file.Id);
            var model = request.Execute();
            return model;
        }
        public async Task<string> DownloadFileAsync(File file, string saveTo)
        {
            string result = string.Empty;
            await AuthenticationAsync();

            var request = Service.Files.Get(file.Id);
            var stream = new System.IO.MemoryStream();
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            result = "Download complete.";
                            SaveStream(stream, saveTo);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            result = "Download failed.";
                            break;
                        }
                }
            };
            request.Download(stream);
            return result;
        }

        public string LastError { set; get; } = string.Empty;
        public async Task<IEnumerable<File>> GetFilesAsync(int? n)
        {
            AuthenticateByKey();
            FilesResource.ListRequest listRequest = Service.Files.List();
            listRequest.PageSize = 100;
            string type = "mimeType='application/msword' or mimeType='application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
            listRequest.Q = type;
            File file = new File();
            Type myType = file.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            var fields = props.Select(t => t.Name).ToArray();

            string _fields = "files(" + string.Join(", ", fields) + ")";
            listRequest.Fields = @"files(*)";
            try
            {
                var files = listRequest.Execute().Files;
                return files;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            //bool authorized = AuthenticationAsync().Result;
            ////bool authorized = AuthenticateByKey();
            //if (!authorized)
            //{
            //    return null;
            //}
            //else
            //{

            //    FilesResource.ListRequest listRequest = Service.Files.List();
            //    listRequest.PageSize = 100;
            //    string type = "mimeType='application/msword' or mimeType='application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
            //    listRequest.Q = type;
            //    File file = new File();
            //    Type myType = file.GetType();
            //    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            //    var fields = props.Select(t => t.Name).ToArray();

            //    string _fields = "files(" + string.Join(", ", fields) + ")";
            //    listRequest.Fields = @"files(*)";
            //    try
            //    {
            //        var files = listRequest.Execute().Files;
            //        return files;
            //    }
            //    catch(Exception ex)
            //    {
            //        LastError = ex.Message;
            //        Console.WriteLine(ex.Message);
            //        return null;
            //    }
            //}
        }

        public async Task<IEnumerable<File>> GetFilesAsync(string keyword)
        {
            await AuthenticationAsync();
            string pageToken = null;
            List<string> results = new List<string>();
            var request = Service.Files.List();
            //request.Spaces = "Books";
            string type = "mimeType='application/pdf'";
            //string folder = "mimeType = 'application/vnd.google-apps.folder' and title = 'Books'";
            request.Q = type;
            request.PageToken = pageToken;
            request.PageSize = 1000;
            var result = request.Execute().Files;
            return result;
        }

        public async Task<File> UploadAsync(string fileUpload)
        {
            using (var stream = new System.IO.FileStream(ClientSecrectFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "taipm.vn@gmail.com",
                    CancellationToken.None);
            }
            Service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = CurrentApplicationName
            });

            File body = new File();
            body.Name = System.IO.Path.GetFileName(fileUpload);
            body.Description = "File updated by Diamto Drive Sample";
            body.MimeType = GetMimeType(fileUpload);
            //body.Parents = new List() { new ParentReference() { Id = _parent } };

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(fileUpload);
            System.IO.Stream stream2 = new System.IO.MemoryStream(byteArray);
            try
            {
                FilesResource.CreateMediaUpload request =
                    Service.Files.Create(body, stream2, GetMimeType(fileUpload));
                request.Upload();
                return request.ResponseBody;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
        // tries to figure out the mime type of the file.
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }

}