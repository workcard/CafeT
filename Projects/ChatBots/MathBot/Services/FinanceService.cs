using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class FinanceService
{
    #region LUIS Services
    public static async Task<string> GetEntityFromLUIS(string Query)
    {
        Query = Uri.EscapeDataString(Query);
        //Rootobject Data = new Rootobject();
        using (HttpClient client = new HttpClient())
        {
            string RequestURI =
                "https://api.projectoxford.ai/luis/v1/application?id=f30e2ba8-5d6e-471f-b934-6aec62dc604e&subscription-key=6438e43093424af2b60b84ac2e15f702&q=" + Query;
            HttpResponseMessage msg = await client.GetAsync(RequestURI);

            if (msg.IsSuccessStatusCode)
            {
                var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                return JsonDataResponse;
                //Data = JsonConvert.DeserializeObject<rootobject>(JsonDataResponse);
            }
        }
        return string.Empty;
    }
    #endregion
    #region Finance services
    //public static string CurrencyConvert(decimal amount, string fromCurrency, string toCurrency)
    //{

    //    //Grab your values and build your Web Request to the API
    //    string apiURL = String.Format("https://www.google.com/finance/converter?a={0}&from={1}&to={2}&meta={3}", amount, fromCurrency, toCurrency, Guid.NewGuid().ToString());

    //    //Make your Web Request and grab the results
    //    var request = WebRequest.Create(apiURL);

    //    //Get the Response
    //    var streamReader = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.ASCII);

    //    //Grab your converted value (ie 2.45 USD)
    //    var result = Regex.Matches(streamReader.ReadToEnd(), "<span class=\"?bld\"?>([^<]+)</span>")[0].Groups[1].Value;

    //    //Get the Result
    //    return result;
    //}

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
            //handle your exception here  
            throw ex;
        }
    }
    #endregion
}