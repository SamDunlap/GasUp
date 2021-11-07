using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasUp.Models
{
    public class UserLocationModel
    {
        public double lat;
        public double lng;


        public UserLocationModel (double latitude, double longtude) {
            lat = latitude;
            lng = longtude;
        }

    }
}
