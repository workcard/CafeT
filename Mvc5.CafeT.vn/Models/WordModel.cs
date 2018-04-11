using CafeT.BusinessObjects;
using CafeT.Objects;

namespace Mvc5.CafeT.vn.Models
{
    public class WordModel : BaseObject
    {
        public WordObject Model { set; get; }
        public string Translation { set; get; } = string.Empty;
        public bool IsRemembered { set; get; } = false;

        public WordModel() : base()
        {
            Model = new WordObject();
        }

        public WordModel(string model) : base()
        {
            Model = new WordObject(model);
        }
        public WordModel(WordObject model) : base()
        {
            Model = new WordObject(model.Value,model.Index);
        }
    }
}