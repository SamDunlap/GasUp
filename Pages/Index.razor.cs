using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasUp.Logic;
using GasUp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.IO;
using System.Text;
using System.Data;

namespace GasUp.Pages
{
    public partial class Index
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        IJSRuntime js { get; set; }
        string userLocation { get; set; }

        bool show = true;
        StationModel TEST = new StationModel("", "", 0.0);

        List<StationModel> Stations { get; set; } = new List<StationModel>();
        UserLocationModel user;

        private static object[] data = { "https://thingproxy.freeboard.io/fetch/https://www.gasbuddy.com/gasprices/michigan/ann-arbor" };


        protected override async Task OnInitializedAsync()
        {
            await CallStationTest();
        }

        public async Task CallStationTest()
        {
            var input = await js.InvokeAsync<string>("httpGet", "https://www.gasbuddy.com/home?search=48104&fuel=1&maxAge=0&method=all");
            var input2 = await js.InvokeAsync<string>("httpGet", "https://www.gasbuddy.com/home?search=48105&fuel=1&maxAge=0&method=all");
            var x = input.Length;
            GasStationLogic test = new GasStationLogic();
            Stations = test.GetStations(input);
            Stations.AddRange(test.GetStations(input2));
            Stations = Stations.OrderBy<StationModel, Double>(x => x.distance).ThenBy<StationModel, Double>(x => x.price).ToList();
            var input3 = await js.InvokeAsync<string>("getLocation");
            userLocation = input3;
            DistanceCalculationAndFilter test3 = new DistanceCalculationAndFilter();
            List<StationModel> tempStations = Stations;
            string addressUrl = test3.getAddressUrl(userLocation);
            //var input4 = await js.InvokeAsync<string>("httpGetAddress", addressUrl);
            for (int i = 0; i < Stations.Count; ++i)
            {
                string url = test3.createUrl(Stations[i], addressUrl);
                var input5 = await js.InvokeAsync<string>("httpGet2", Stations[i].Address, addressUrl);
                test3.FindDistanceAndFormat(input5, ref tempStations, i);

            }
     
            Stations = tempStations;
        }

        public void ToggleHide()
        {
            show = !show;
        }

        public void Reload()
        {
            Navigation.NavigateTo("/GasUp/", true);
        }
    }
}
