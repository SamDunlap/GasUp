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
        NavigationManager Navigation { get; set; }

        [Inject]
        IJSRuntime js { get; set; }
        string stuff { get; set; }

        bool show = true;

        List<StationModel> Stations { get; set; } = new List<StationModel>();

        private static object[] data = { "https://thingproxy.freeboard.io/fetch/https://www.gasbuddy.com/gasprices/michigan/ann-arbor" };


        protected override async Task OnInitializedAsync()
        {
            await CallStationTest();
        }
        public void CallDistanceTest()
        {
            DistanceCalculationAndFilter test = new DistanceCalculationAndFilter();
            test.FindDistanceAndFormat(Stations);
        }

        public async Task CallStationTest()
        {
            
            var input = await js.InvokeAsync<string>("httpGet", "https://www.gasbuddy.com/home?search=48104&fuel=1&maxAge=0&method=all");
            var input2 = await js.InvokeAsync<string>("httpGet", "https://www.gasbuddy.com/home?search=48105&fuel=1&maxAge=0&method=all");
            var x = input.Length;
            //stuff = input;
            GasStationLogic test = new GasStationLogic();
            Stations = test.GetStations(input);
            Stations.AddRange(test.GetStations(input2));
            Stations = Stations.OrderBy<StationModel, Double>(x => x.distance).ThenBy<StationModel,Double>(x => x.price).ToList();
        }

        public async Task CallUserTest()
        {
            var input = await js.InvokeAsync<string>("getLocation");
            stuff = input;
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
