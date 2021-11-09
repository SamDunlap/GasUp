using GasUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace GasUp.Logic

{



    public class DistanceCalculationAndFilter

    {
        public List<StationModel> finalStations;

        private async Task<string> GetHtml(string url)
        {
            HttpClient client = new HttpClient();
            var data = await client.GetAsync($"https://thingproxy.freeboard.io/fetch/https://maps.googleapis.com/maps/api/distancematrix/xml?origins=Vancouver+BC|Seattle&destinations=San+Francisco|Vancouver+BC&mode=bicycling&language=fr-FR&key=AIzaSyBifNS8nqRNqFXzdlv3wDbo8pRjhhGAuqY/DistanceMatrixResponse/row/element/distance/text");
            var stream = await data.Content.ReadAsStringAsync();
            return stream;
        }

        public String getAddressUrl(string userLocation)
        {
            userLocation = userLocation.Replace(" ", ",");
            userLocation = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + userLocation + "%26key=" + "API KEY HERE";
            return userLocation;
        }

        public String createUrl(StationModel station, string userLocation)
        {
            string url = "";
            string destination = station.Address;
            url = "https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + userLocation + "&destinations=" + destination + "%26key=" + "API KEY HERE";
            url = url.Replace(" ", "+");
            url = url.Replace("\n", "+");
            return url;
        }
        public void FindDistanceAndFormat(string data, ref List<StationModel> stations, int counter)
        {
            int index = data.Length - 2;
            double distance = 0;
            if (data[index] == 'k')
            {
                data = data.Substring(0, index - 1);
                data = data.Replace(",", "");
                distance = Convert.ToDouble(data);
                distance = distance * 0.621371;

            }
            else
            {
                data = data.Substring(0, index);
                data = data.Replace(",", "");
                distance = Convert.ToDouble(data);
                distance = distance * 0.621371;
            }
            double result = Math.Round(distance, 1);
            stations[counter].distance = result; 
            return;
        }
    }
}
