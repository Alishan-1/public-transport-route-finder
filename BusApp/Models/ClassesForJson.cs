using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusApp.Models
{
    public partial class RouteDetailForJson
    {
        public RouteDetailForJson()
        {

        }
        public RouteDetailForJson(RouteDetail routeDetail)
        {
            this.ID = routeDetail.ID;
            this.RouteID = routeDetail.RouteID;
            this.StopNo = routeDetail.StopNo;
            this.StopID = routeDetail.StopID;
            this.DistanceFromPrevStop = routeDetail.DistanceFromPrevStop;
            this.Stop = new StopForJson(routeDetail.Stop);
        }
        public decimal ID { get; set; }
        public decimal RouteID { get; set; }
        public int StopNo { get; set; }
        public decimal StopID { get; set; }
        public double DistanceFromPrevStop { get; set; }


        public StopForJson Stop { get; set; }
    }

    public class RouteForJson
    {
        public RouteForJson()
        {

        }
        public RouteForJson(Route r)
        {
            this.ID = r.ID;
            this.CityID = r.CityID;
            this.Name = r.Name;
            this.Status = r.Status;
            this.FareCalcFormula = r.FareCalcFormula;
            this.TypeID = r.TypeID;
            this.AvgSpeed = r.AvgSpeed;
            this.RouteType = new RouteTypeForJson(r.RouteType);
        }
        public decimal ID { get; set; }
        public int CityID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string FareCalcFormula { get; set; }
        public Nullable<short> TypeID { get; set; }
        public Nullable<double> AvgSpeed { get; set; }
        public  RouteTypeForJson RouteType { get; set; }
    }

    public  class StopForJson
    {
        public StopForJson()
        {
            
        }
        public StopForJson(Stop stop)
        {
            this.ID = stop.ID;
            this.CityID = stop.CityID;
            this.Name = stop.Name;
            this.Discription = stop.Discription;
            this.Latitude = stop.Latitude;
            this.Longitude = stop.Longitude;

        }
        public decimal ID { get; set; }
        public int CityID { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
    }
    public  class RouteTypeForJson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RouteTypeForJson()
        {
        }
        public RouteTypeForJson(RouteType Rt)
        {
            this.ID = Rt.ID;
            this.Type = Rt.Type;
            this.Status = Rt.Status;
        }
        public short ID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

    }
}