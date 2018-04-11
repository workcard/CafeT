using CafeT.Text;
using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace Mvc5.CafeT.vn.ModelViews
{
    public class YouTubeView:BaseView
    {
        public const string ytKey = "AIzaSyA5naCjbPLqFdhaY6-f24bwoTwKSTHS9m4";

        public string Name { set; get; }
        public string EmbedUrl { set; get; }
        public string Width { set; get; } = "100%";
        public string Height { set; get; } = "auto";

        public YouTubeView(string watchUrl)
        {
            string _before = @"<iframe width=" + Width + " height=" + Height + " src=";
            string _end = " frameborder=\"0\" allowfullscreen></iframe>";
            string _youtubeLink = "\"" +watchUrl + "\"";
            _youtubeLink = _youtubeLink
                .AddBefore(_before)
                .AddAfter(_end)
                .Replace("watch?v=", "embed/");
            EmbedUrl = _youtubeLink;
            YouTubeImport(watchUrl.GetFromEndTo("=").Substring(1));
        }

        //public int Width { get; set; }
        //public int Height { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; }
        public string ThumbUrl { get; set; }
        public string BigThumbUrl { get; set; }
        public string Description { get; set; }
        public string VideoDuration { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }

        public bool YouTubeImport(string VideoID)
        {
            try
            {
                WebClient WebDownloader = new WebClient();
                WebDownloader.Encoding = System.Text.Encoding.UTF8;

                string _requestMsg = "https://www.googleapis.com/youtube/v3/videos?id=" + VideoID + "&key=" + ytKey + "&part=snippet";
                string jsonResponse = WebDownloader.DownloadString(_requestMsg);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var dynamicObject = Json.Decode(jsonResponse);
                var item = dynamicObject.items[0].snippet;

                Title = item.title;
                ThumbUrl = item.thumbnails.@default.url;
                BigThumbUrl = item.thumbnails.high.url;
                Description = item.description;
                UploadDate = Convert.ToDateTime(item.publishedAt);

                jsonResponse = WebDownloader.DownloadString("https://www.googleapis.com/youtube/v3/videos?id=" + VideoID + "&key=" + ytKey + "&part=contentDetails");
                dynamicObject = Json.Decode(jsonResponse);
                string tmp = dynamicObject.items[0].contentDetails.duration;
                Duration = Convert.ToInt32(System.Xml.XmlConvert.ToTimeSpan(tmp).TotalSeconds);

                Url = "http://www.youtube.com/watch?v=" + VideoID;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool VimeoImport(string VideoID)
        {
            try
            {
                WebClient myDownloader = new WebClient();
                myDownloader.Encoding = System.Text.Encoding.UTF8;

                string jsonResponse = myDownloader.DownloadString("http://vimeo.com/api/v2/video/" + VideoID + ".json");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var dynamicObject = Json.Decode(jsonResponse);
                var item = dynamicObject[0];

                Title = item.title;
                Description = item.description;
                Url = item.url;
                ThumbUrl = item.thumbnail_small;
                BigThumbUrl = item.thumbnail_large;
                UploadDate = Convert.ToDateTime(item.upload_date);
                Width = Convert.ToInt32(item.width);
                Height = Convert.ToInt32(item.height);
                Duration = Convert.ToInt32(item.duration);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}