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
        string stuff { get; set; }

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
            var input3 = await js.InvokeAsync<string>("getLocation");
            stuff = input;
            UserLocationLogic test2 = new UserLocationLogic();
            user = test2.GetUserLocation();
            DistanceCalculationAndFilter test3 = new DistanceCalculationAndFilter();
            List<StationModel> tempStations = Stations;
            for (int i = 0; i < Stations.Count; ++i)
            {
                string url = test3.createUrl(Stations[i], user);
                var input4 = await js.InvokeAsync<string>("httpGet2", url);
                Console.WriteLine(input4);
                test3.FindDistanceAndFormat(input4, ref tempStations, user, i);

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
