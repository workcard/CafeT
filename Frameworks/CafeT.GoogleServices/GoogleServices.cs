using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CafeT.GoogleServices
{
    //public class Uploader
    //{
    //    private string CurrentApiKey = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4";
    //    public string UserName { set; get; }
    //    public string CurrentApplicationName = "CafeT.vn";
    //    protected DriveService Service;

    //    private const int KB = 0x400;
    //    private const int DownloadChunkSize = 256 * KB;

    //    // CHANGE THIS with full path to the file you want to upload
    //    private const string UploadFileName = @"FILE_TO_UPLOAD";

    //    // CHANGE THIS with a download directory
    //    private const string DownloadDirectoryName = @"DIRECTORY_FOR_DOWNLOADING";

    //    // CHANGE THIS if you upload a file type other than a jpg
    //    private const string ContentType = @"image/jpeg";

    //    /// <summary>The logger instance.</summary>
    //    //private static readonly ILogger Logger;

    //    /// <summary>The Drive API scopes.</summary>
    //    private static readonly string[] Scopes = 
    //        new[] {
    //            DriveService.Scope.DriveFile,
    //            DriveService.Scope.Drive,
    //            DriveService.Scope.DriveMetadata,
    //            DriveService.Scope.DriveAppdata,
    //            DriveService.Scope.DriveMetadataReadonly,
    //            DriveService.Scope.DrivePhotosReadonly,
    //            DriveService.Scope.DriveReadonly,
    //        };

    //    UserCredential credential;
    //    public string ClientSecrectFile { set; get; }

    //    public Uploader(string userName)
    //    {
    //        UserName = userName;
    //    }

    //    public async Task AuthenticationAsync()
    //    {
    //        using (var stream = new System.IO.FileStream(ClientSecrectFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
    //        {
    //            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
    //                GoogleClientSecrets.Load(stream).Secrets,
    //                Scopes,
    //                UserName,
    //                CancellationToken.None);
    //        }
    //        Service = new DriveService(new BaseClientService.Initializer()
    //        {
    //            HttpClientInitializer = credential,
    //            ApplicationName = CurrentApplicationName
    //        });
    //    }

    //    public void SaveStream(System.IO.MemoryStream stream, string saveTo)
    //    {
    //        using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
    //        {
    //            stream.WriteTo(file);
    //        }
    //    }

        

    //    public async Task<File> GetFileAsync(string id)
    //    {
    //        await AuthenticationAsync();
    //        FilesResource.GetRequest request = Service.Files.Get(id);

    //        var model= request.Execute();

    //        return model;
    //    }

    //    public async Task<File> GetFileMetadataAsync(string id)
    //    {
    //        await AuthenticationAsync();
    //        FilesResource.GetRequest request = Service.Files.Get(id);
    //        File file = new File();

    //        Type myType = file.GetType();
    //        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
    //        var fields = props.Select(t => t.Name).ToArray();

    //        string _fields = "files("+ string.Join(", ", fields) +")";
    //        request.Fields = "nextPageToken, " + _fields;// files(id, name, webContentLink)";

    //        var model = request.Execute();//.Files.Get(id).Execute();
    //        return model;
    //    }
    //    public async Task<File> UpdateAsync(File file)
    //    {
    //        string result = string.Empty;
    //        await AuthenticationAsync();
    //        string Description = "This is automatic update description for file";
    //        file.Description = Description;

    //        File body = new File();
    //        body.Title = file.Title;
    //        body.Description = "File updated by Diamto Drive Sample";
    //        body.MimeType = GetMimeType(file.MimeType);
    //        body.Parents = file.Parents;

    //        FilesResource.UpdateRequest request = Service.Files.Update(body, file.Id);
    //        var model = request.Execute();
    //        return model;
    //    }
    //    public async Task<string> DownloadFileAsync(File file, string saveTo)
    //    {
    //        string result = string.Empty;
    //        await AuthenticationAsync();

    //        var request = Service.Files.Get(file.Id);
    //        var stream = new System.IO.MemoryStream();
    //        request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
    //        {
    //            switch (progress.Status)
    //            {
    //                case Google.Apis.Download.DownloadStatus.Downloading:
    //                    {
    //                        Console.WriteLine(progress.BytesDownloaded);
    //                        break;
    //                    }
    //                case Google.Apis.Download.DownloadStatus.Completed:
    //                    {
    //                        result = "Download complete.";
    //                        SaveStream(stream, saveTo);
    //                        break;
    //                    }
    //                case Google.Apis.Download.DownloadStatus.Failed:
    //                    {
    //                        result = "Download failed.";
    //                        break;
    //                    }
    //            }
    //        };
    //        request.Download(stream);
    //        return result;
    //    }

    //    //public async Task<IEnumerable<File>> GetFilesAsync(int? n)
    //    //{
    //    //    await AuthenticationAsync();
    //    //    FilesResource.ListRequest listRequest = Service.Files.List();
    //    //    listRequest.MaxResults = 100;
    //    //    string type = "mimeType='application/msword' or mimeType='application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
    //    //    listRequest.Q = type;
    //    //    File file = new File();
    //    //    Type myType = file.GetType();
    //    //    IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
    //    //    var fields = props.Select(t => t.Name).ToArray();

    //    //    string _fields = "files(" + string.Join(", ", fields) + ")";
    //    //    listRequest.Fields = @"files(*)";
    //    //    var files = listRequest.Execute().Files;
    //    //    return files;
    //    //}

    //    //public async Task<IEnumerable<File>> GetFilesAsync(string keyword)
    //    //{
    //    //    await AuthenticationAsync();
    //    //    string pageToken = null;
    //    //    List<string> results = new List<string>();
    //    //    var request = Service.Files.List();
    //    //    //request.Spaces = "Books";
    //    //    string type = "mimeType='application/pdf'";
    //    //    //string folder = "mimeType = 'application/vnd.google-apps.folder' and title = 'Books'";
    //    //    request.Q = type;
    //    //    request.PageToken = pageToken;
    //    //    request.PageSize = 1000;
    //    //    var result = request.Execute().Files;
    //    //    return result;
    //    //}

    //    public async Task<File> UploadAsync(string fileUpload)
    //    {
    //        using (var stream = new System.IO.FileStream(ClientSecrectFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
    //        {
    //            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
    //                GoogleClientSecrets.Load(stream).Secrets,
    //                Scopes,
    //                "taipm.vn@gmail.com",
    //                CancellationToken.None);
    //        }
    //        Service = new DriveService(new BaseClientService.Initializer()
    //        {
    //            HttpClientInitializer = credential,
    //            ApplicationName = CurrentApplicationName
    //        });

    //        File body = new File();
    //        body.Name = System.IO.Path.GetFileName(fileUpload);
    //        body.Description = "File updated by Diamto Drive Sample";
    //        body.MimeType = GetMimeType(fileUpload);
    //        //body.Parents = new List() { new ParentReference() { Id = _parent } };

    //        // File's content.
    //        byte[] byteArray = System.IO.File.ReadAllBytes(fileUpload);
    //        System.IO.Stream stream2 = new System.IO.MemoryStream(byteArray);
    //        try
    //        {
    //            FilesResource.CreateMediaUpload request =
    //                Service.Files.Create(body, stream2, GetMimeType(fileUpload));
    //            request.Upload();
    //            return request.ResponseBody;
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine("An error occurred: " + e.Message);
    //            return null;
    //        }
    //    }
    //    // tries to figure out the mime type of the file.
    //    private static string GetMimeType(string fileName)
    //    {
    //        string mimeType = "application/unknown";
    //        string ext = System.IO.Path.GetExtension(fileName).ToLower();
    //        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
    //        if (regKey != null && regKey.GetValue("Content Type") != null)
    //            mimeType = regKey.GetValue("Content Type").ToString();
    //        return mimeType;
    //    }
    //}
    
    public class GoogleServices
    {
        public GoogleServices()
        {
        }

        public string GetGoogleApiKey()
        {
            return "AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc";
        }
        public string GetGoogleSearchEngine()
        {
            return "004317969426278842680:_f5wbsgg7xc";
        }
       
        public Google.Apis.Customsearch.v1.Data.Search Search(string keywords)
        {
            List<string> _results = new List<string>();

            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = GetGoogleApiKey()
            });
            
            var listRequest = svc.Cse.List(keywords);
            listRequest.Cx = GetGoogleSearchEngine();
            //listRequest.ExactTerms = "stackoverflow";
            listRequest.Gl = "en"; //Default search by Vietnamese
            listRequest.Hl = "vi";

            var search = listRequest.Execute();
            return search;
        }

        public Google.Apis.Customsearch.v1.Data.Search SearchImage(string keywords)
        {
            List<string> _results = new List<string>();
            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer
            {
                ApiKey = GetGoogleApiKey()
            });
            //var listRequest = svc.Cse.List(keywords.Split(new string[] { "," },StringSplitOptions.None)[0]);
            var listRequest = svc.Cse.List(keywords);
            listRequest.Cx = GetGoogleSearchEngine();
            listRequest.SearchType = Google.Apis.Customsearch.v1.CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Gl = "en"; //Default search by Vietnamese
            listRequest.Hl = "vi";
           
            //string startDate = DateTime.UtcNow.AddDays(-3).ToString("yyyyMMdd");
            //string endDate = DateTime.UtcNow.ToString("yyyyMMdd");
            //listRequest.DateRestrict = startDate + ":" + endDate;
            //listRequest.Start = 1;
            var search = listRequest.Execute();//.Fetch();
            return search;
        }
    }
}
