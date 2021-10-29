using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasUp.Logic;
using GasUp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GasUp.Pages
{
    public partial class Index
    {
        [Inject]
        IJSRuntime js { get; set; }
        string stuff { get; set; }

        List<StationModel> Stations { get; set; } = new List<StationModel>();

        private static object[] data = { "https://thingproxy.freeboard.io/fetch/https://www.gasbuddy.com/gasprices/michigan/ann-arbor" };

        public void CallDistanceTest()
        {
            DistanceCalculationAndFilter test = new DistanceCalculationAndFilter();
            test.FindDistanceAndFormat();
        }

        public async Task CallStationTest()
        {
            
            var input = await js.InvokeAsync<string>("httpGet", "info");
            var x = input.Length;
            //stuff = input;
            GasStationLogic test = new GasStationLogic();
            Stations = test.GetStations(input);
        }

        public async Task CallUserTest()
        {
            var input = await js.InvokeAsync<string>("getLocation");
            stuff = input;
        }
    }
}
