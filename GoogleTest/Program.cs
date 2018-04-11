using CafeT.GoogleManager;
using CafeT.Text;
using CafeT.Translators;
using System;

namespace GoogleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello ?vi".GetFromBeginTo("?"));
            Console.WriteLine("Hello ?vi".GetFromEndTo("?"));
            Manager manager = new Manager("AIzaSyDz0FsFxxf7xsskqxlhaNWMvxM05b4HQBc", "004317969426278842680:_f5wbsgg7xc");
            string response = new Translator().Translate("Bạn khỏe không", "en", "vi");
            Console.WriteLine(response);

            //var search = manager.Search("Hello");
            //Console.WriteLine(search.Items.Count);

            //var image = manager.SearchImage("C#");
            //Console.WriteLine(image.Items.Count);

            Console.ReadKey();
        }
    }
}
