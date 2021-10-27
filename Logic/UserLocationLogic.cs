using GasUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Device.Location;

namespace GasUp.Logic
{
    public class UserLocationLogic
    {
        public UserLocationModel GetUserLocation()
        {

            /*GeoCoordinateWatcher watcher= new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start(); //started watcher
            GeoCoordinate coord = watcher.Position.Location;
            double lat = 0;
            double lng = 0;
            if (!watcher.Position.Location.IsUnknown) {
                lat = coord.Latitude; //latitude
                lng = coord.Longitude;  //logitude
            }
            Console.WriteLine("(" + lat + "," + lng + ")");
            return new UserLocationModel(lat,lng);*/
            return new UserLocationModel(1, 2);
        }
    }
}
