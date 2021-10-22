using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasUp.Models
{
    public class UserLocationModel
    {
        private double lat;
        private double lng;


        public UserLocationModel (double latitude, double longtude) {
            lat = latitude;
            lng = longtude;
        }

    }
}
