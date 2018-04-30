namespace Web.Models
{
    public class Document:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Path { set; get; }
        public string GDriveId { set; get; }
        public string DownloadUrl { set; get; }
        public int CountOfDownload { set; get; } = 0;
        public double Size { set; get; } = 0;
        //public string Storage { set; get; }
    }
}