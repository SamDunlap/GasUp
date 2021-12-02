using GasUp.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasUp.Logic
{
    public class GasStationLogic
    {
        private async Task<string> GetHtml(string url)
        {
            HttpClient client = new HttpClient();
            var data = await client.GetAsync($"https://thingproxy.freeboard.io/fetch/https://www.gasbuddy.com/home?search=48105&fuel=1&maxAge=0&method=all");
            var stream = await data.Content.ReadAsStringAsync();
            //var response_body = await client.GetStringAsync(url);
            return stream;
        }

        public List<StationModel> GetStations(string data)
        {
            // List to hold station objects
            List<StationModel> stations = new List<StationModel>();

            var scrape_url = @"http://www.whateverorigin.org/get?url=https://www.gasbuddy.com/gasprices/michigan/ann-arbor";
            var response_body = data;
            var html_doc = new HtmlDocument();
            html_doc.LoadHtml(response_body);
            var outter_cont = html_doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[3]/div/div/div[1]/div[3]");
            HtmlNodeCollection inner_divs = outter_cont.ChildNodes;

            foreach (var node in inner_divs)
            {
                // Node: /html/body/div/div/div/div[3]/div/div/div[1]/div[3]/div[1]/div
                // Logo: /html/body/div/div/div/div[3]/div/div/div[1]/div[3]/div[1]/div/div[1]
                // Inner Cont: /html/body/div/div/div/div[3]/div/div/div[1]/div[3]/div[1]/div/div[2]
                var inner_cont = node.SelectSingleNode("./div[1]/div[2]");
                if (inner_cont != null)
                {
                    try
                    {
                        // Station Name
                        var station_name = inner_cont.SelectSingleNode("./h3/a").InnerHtml.Replace("&#x27;", "'");
                        // Gas price in USD
                        string price = node.SelectSingleNode("./div[1]/div[4]/div/span").InnerHtml.TrimStart('$');
                        // Price: /html/body/div/div/div/div[3]/div/div/div[1]/div[3]/div[1]/div/div[4]/div
                        var station_price = Convert.ToDouble(price);
                        // Station Address
                        var station_address = inner_cont.SelectSingleNode("./div[2]").InnerHtml.Replace("<br>", "\n");
                        // Logo: /html/body/div/div/div/div[3]/div/div/div[1]/div[3]/div[2]/div/div[1]/div/div/img
                        var station_logo_src = node.SelectSingleNode("./div[1]/div/div/div/noscript/img").Attributes["src"].Value;

                        if (station_name != null && station_price != null && station_address != null)
                        {
                            StationModel station = new StationModel(station_name, station_address, station_price, station_logo_src);
                            if (station.Address.Contains("Plymouth"))
                            {
                                station.distance = 1;
                            }
                            else if(station.Address.Contains("PLYMOUTH"))
                            {
                                station.distance = 1;
                            }
                            else
                            {
                                Random rnd = new Random();
                                station.distance = rnd.Next(3, 6);
                            }
                            
                            stations.Add(station);
                        }
                    }
                    catch(Exception ex)
                    {
                        
                    }
                }
            }
            stations = stations.Where(x => x != null).Select(x => x).ToList();
            return stations;
        }
    }
}
