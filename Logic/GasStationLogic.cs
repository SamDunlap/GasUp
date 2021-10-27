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
            var data = await client.GetAsync($"https://thingproxy.freeboard.io/fetch/https://www.gasbuddy.com/gasprices/michigan/ann-arbor");
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
            var outter_cont = html_doc.DocumentNode.SelectSingleNode("/html/body/div/div/div/div[3]/div/div[2]/div[2]/div/div[1]/div");
            HtmlNodeCollection inner_divs = outter_cont.ChildNodes;

            foreach (var node in inner_divs)
            {
                var inner_cont = node.SelectSingleNode("./div[1]/div[2]");
                if (inner_cont != null)
                {
                    // Station Name
                    var station_name = inner_cont.SelectSingleNode("./h3/a").InnerHtml.Replace("&#x27;", "'");
                    // Gas price in USD
                    string price = node.SelectSingleNode("./div[1]/div[4]/div/span").InnerHtml.TrimStart('$');
                    var station_price = Convert.ToDouble(price);
                    // Station Address
                    var station_address = inner_cont.SelectSingleNode("./div[2]").InnerHtml.Replace("<br>", "\n");

                    if (station_name != null && station_price != null && station_address != null)
                    {
                        StationModel station = new StationModel(station_name, station_address, station_price);
                        stations.Add(station);
                    }
                }
            }
            stations = stations.Where(x => x != null).Select(x => x).ToList();
            return stations;
        }
        
        /*public List<StationModel> GetStations()
        {
            StationModel s1 = new StationModel("CITGO", "215 Ecorse Rd, Ypsilanti, MI", 3.01);
            StationModel s2 = new StationModel("Sunoco", "1024 Ecorse Rd, Ypsilanti, MI", 3.01);
            StationModel s3 = new StationModel("RP Fuel", "445 S Huron St, Ypsilanti, MI", 3.09);
            StationModel s4 = new StationModel("BP", "10131 TEXTILE Rd, Ypsilanti, MI", 3.09);
            StationModel s5 = new StationModel("Circle K", "5495 W Michigan Ave, Ypsilanti, MI", 3.09);
            StationModel s6 = new StationModel("Marathon", "2375 S Grove St, Ypsilanti, MI", 3.09);
            StationModel s7 = new StationModel("Speedway", "2190 Rawsonville Rd, Belleville, MI", 3.09);
            StationModel s8 = new StationModel("Costco", "771 Airport Blvd, Ann Arbor, MI", 3.14);
            StationModel s9 = new StationModel("Speedway", "300 W Six Mile Rd., Whitmore Lake, MI", 3.17);
            StationModel s10 = new StationModel("Sunoco", "3891 Platt Rd, Ann Arbor, MI", 3.17);
            List<StationModel> stations = new List<StationModel>
            {
                s1, s2, s3, s4, s5, s6, s7, s8, s9, s10
            };
            return stations;
        }*/
    }
}
