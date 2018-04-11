namespace Web.Models
{
    public class Document:BaseObject
    {
        public string Title { set; get; }
        public string Description { get; set; }
        public string Path { set; get; }
    }
}