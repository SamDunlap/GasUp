using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasUp.Logic;

namespace GasUp.Pages
{
    public partial class Index
    {
        public void CallDistanceTest()
        {
            DistanceCalculationAndFilter test = new DistanceCalculationAndFilter();
            test.FindDistanceAndFormat();
        }

        public void CallStationTest()
        {
            GasStationLogic test = new GasStationLogic();
            test.GetStations();
        }

        public void CallUserTest()
        {
            UserLocationLogic test = new UserLocationLogic();
            test.GetUserLocation();
        }
    }
}
