using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusApp.Models
{
    public class FindRouteInput
    {
        public City city;
        public Stop startPoint;
        public Stop destinationPoint;
    }
}