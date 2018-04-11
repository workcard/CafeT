using System.IO;

namespace CafeT.BusinessObjects
{
    public class ImageObject:BaseObject
    {
        public string FileName { set; get; }
        public string Description { set; get; }
        public string FullPath { set; get; }
        protected FileInfo file;

        public ImageObject()
        {
        }
        public ImageObject(string fullPath)
        {
            FullPath = fullPath;
            file = new FileInfo(fullPath);
            FileName = file.Name;
        }

        public string ExtractText()
        {
            return string.Empty;
        }
    }
}
