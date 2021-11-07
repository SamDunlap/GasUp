using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasUp.Models
{
    public class StationModel
    {
        public string CompanyName;
        public string Address;
        public double distance;
        public double price;

        public StationModel(string name, string addr, double p)
        {
            CompanyName = name;
            Address = addr;
            distance = '0';
            price = p;
        }
    }
}
