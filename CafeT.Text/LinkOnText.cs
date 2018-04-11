using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CafeT.Text
{
    public static class LinkOnText
    {
        public static bool IsUrl(this string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (text.StartsWith("http"))
                {
                    Regex rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");

                    return rx.IsMatch(text);
                }
            }
            return false;
        }

        public static string GetHost(this string url)
        {
            if(url.IsUrl())
            {
                Uri myUri = new Uri(url);
                return myUri.Host;
            }
            return string.Empty;
        }

        public static string GetDomain(this string url)
        {
            var doubleSlashesIndex = url.IndexOf("://");
            var start = doubleSlashesIndex != -1 ? doubleSlashesIndex + "://".Length : 0;
            var end = url.IndexOf("/", start);
            if (end == -1)
                end = url.Length;

            string trimmed = url.Substring(start, end - start);
            if (trimmed.StartsWith("www."))
                trimmed = trimmed.Substring("www.".Length);
            return trimmed;
        }

        public static string ToHtmlLink(this string url)
        {
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.Replace(url, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>")
                .Replace("href=\"www", "href=\"http://www");
        }

        public static string[] GetUrlsWithoutHref(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            //string RegexPattern = @"\b(?:https?://|www\.)\S+\b";
            string RegexPattern = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            MatchCollection matches = Regex.Matches(text, RegexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            //// Report on each match.
            int c = 0;
            foreach (Match match in matches)
            {
                MatchList[c] = match.Value;
                c++;
            }

            return MatchList;
        }
        public static string[] GetUrlsWithHref(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            //match.Groups["name"].Value - URL Name
            // match.Groups["url"].Value - URI
            string RegexPattern = @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>";

            // Find matches.
            MatchCollection matches = Regex.Matches(text, RegexPattern, RegexOptions.IgnoreCase);

            string[] MatchList = new string[matches.Count];

            // Report on each match.
            int c = 0;
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                MatchList[c] = match.Groups["url"].Value;
                c++;
            }

            return MatchList;
        }

        public static string[] GetUrls(this string text)
        {
            if (text == null || text.Length <= 0) return null;
            List<string> _urls = text.GetUrlsWithHref().ToList();
            List<string> _urls2 = text.GetUrlsWithoutHref().ToList();
            foreach (string _url in _urls2)
            {
                _urls.Add(_url);
            }
            return _urls.Distinct().ToArray();
        }


        #region Big company links
        public static bool HasYouTubeLink(this string text)
        {
            if(text.GetYouTubeUrls() != null && text.GetYouTubeUrls().Count() > 0)
            {
                return true;
            }
            return false;
        }

        public static bool HasGoogleDriveUrls(this string text)
        {
            if (text.GetGoogleDriveUrls() != null && text.GetGoogleDriveUrls().Count() > 0)
            {
                return true;
            }
            return false;
        }

        public static bool HastMicrosoftDriveUrls(this string text)
        {
            if (text.GetMicrosoftDriveUrls() != null && text.GetMicrosoftDriveUrls().Count() > 0)
            {
                return true;
            }
            return false;
        }

        public static bool HasCloudEmbedFiles(this string text)
        {
            if (text.HasGoogleDriveUrls() || text.HastMicrosoftDriveUrls())
                return true;
            return false;
        }

        public static bool IsYouTubeUrl(this string url)
        {
            if (url.ToLower().Contains("youtube")) return true;
            return false;
        }
        public static bool IsYouTubeWatchUrl(this string url)
        {
            if (url.IsYouTubeUrl() && url.Contains("watch")) return true;
            return false;
        }
        public static string GetYouTubeId(this string youTubeUrl)
        {
            Match regexMatch = Regex.Match(youTubeUrl, "^[^v]+v=(.{11}).*",
                               RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                return "http://www.youtube.com/v/" + regexMatch.Groups[1].Value + "&hl=en&fs=1";
            }
            return youTubeUrl;
        }

        public static string[] GetYouTubeWatchUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.IsYouTubeWatchUrl())
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.Distinct().ToArray();
        }
        public static string[] GetYouTubeUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls()
                .Where(t=>t.IsYouTubeUrl())
                .ToArray();
            return _urls;
        }

        public static string[] GetGoogleDriveUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://drive.google.com/"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }

        public static string[] GetDropboxUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://www.dropbox.com/"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }
        public static string[] GetMicrosoftDriveUrls(this string str)
        {
            List<string> _youtubeUrls = new List<string>();
            string[] _urls = str.GetUrls();
            foreach (string _url in _urls)
            {
                if (_url.ToLower().Contains("https://onedrive.live.com"))
                {
                    _youtubeUrls.Add(_url);
                }
            }
            return _youtubeUrls.ToArray();
        }

        public static string[] GetEmbedCloudFiles(this string text)
        {
            string[] _microsofts = text.GetMicrosoftDriveUrls();
            string[] _googles = text.GetGoogleDriveUrls();
            string[] _dropboxs = text.GetDropboxUrls();
            return _microsofts.Union(_googles).Union(_dropboxs).ToArray();
        }
        #endregion
    }
}
