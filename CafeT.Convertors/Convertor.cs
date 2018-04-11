using CafeT.Html;
using CafeT.Text;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CafeT.Convertors
{
    public class Convertor
    {
        public static string RateUrl = "https://ex-rate.com/amp/convert/";
        #region Finance services
        public static string CurrencyConvert(decimal amount, string fromCurrency, string toCurrency)
        {
            var url = RateUrl + fromCurrency.ToLower() + "/" + amount.ToString() + "-to-" + toCurrency + ".html";
            //usd /1000-to-vnd.html";
            var page = new WebPage(url);
            string result = page.GetNodesByClass("result-right-out")
                .FirstOrDefault()
                .InnerText.Replace("Vietnamese dong", " VND");
            return result;
            //var tables = page.HtmlTables;
            //string rate = string.Empty;
            //if(tables.Any())
            //{
            //    var table = tables.Where(t => t.HtmlContent.Contains("Ngoại tệ"))
            //        .FirstOrDefault();
            //    if (table != null)
            //    {
            //        var rows = table.Rows;
            //        if (rows.Any())
            //        {
            //            var items = rows.Where(t => t.Contains("USD"));
            //            if(items.Any())
            //            {
            //                foreach (var item in items)
            //                {
            //                    if (item.IsContainsNumber())
            //                    {
            //                        rate = item.GetNumbers()[1];
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //try
            //{
            //    return (decimal.Parse(rate) * amount).ToReadable();
            //}
            //catch
            //{
            //    return "Can't convert";
            //}
        }

        public static async Task<string> GetStock(string StockSymbol)
        {
            double? dblStockValue = await GetStockRateAsync(StockSymbol);
            if (dblStockValue == null)
            {
                return string.Format("This \"{0}\" is not an valid stock symbol", StockSymbol);
            }
            else
            {
                return string.Format("Stock : {0}\n Price : {1}", StockSymbol, dblStockValue);
            }

        }
        public static async Task<double?> GetStockRateAsync(string StockSymbol)
        {
            try
            {
                string ServiceURL = $"http://finance.yahoo.com/d/quotes.csv?s={StockSymbol}&f=sl1d1nd";
                string ResultInCSV;
                using (WebClient client = new WebClient())
                {
                    ResultInCSV = await client.DownloadStringTaskAsync(ServiceURL).ConfigureAwait(false);
                }
                var FirstLine = ResultInCSV.Split('\n')[0];
                var Price = FirstLine.Split(',')[1];
                if (Price != null && Price.Length >= 0)
                {
                    double result;
                    if (double.TryParse(Price, out result))
                    {
                        return result;
                    }
                }
                return null;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
