using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusApp.Models
{
    //when we TRAVEL by bus and go from stop no 1 to 5 then our direction is up
    //and if we go from stop 5 to 1 then our direction is down.
    public enum JourneyDirection { UP, DOWN}
    public class RouteSearchResult
    {
        public List<ResultComponent> busesToTake = new List<ResultComponent>();
        public double TotalLength
        {
            get
            {
                return busesToTake.Select(x => x.Length).Sum();
            }
        }
        public double TotalFare
        {
            get
            {
                return busesToTake.Select(x => x.Fare).Sum();
            }
        }
        public double TotalApproxTimeInMints
        {
            get
            {
                return busesToTake.Select(x => x.ApproxTimeInMints).Sum();
            }
        }
    }

    public class ResultComponent
    {
        public ResultComponent()
        {
            routeNo = new RouteForJson();
            stops = new List<RouteDetailForJson>();
            sequenceNo = 0;
            journeyDirection = JourneyDirection.UP;
            length = 0;
        }
        public RouteForJson routeNo;
        public List<RouteDetailForJson> stops;
        public int sequenceNo;
        public JourneyDirection journeyDirection;
        private double length;
        public double Length
        {
            get
            {
                if (length > 0)
                {
                    return length;
                }
                else
                {
                    calculateLength();
                    return length;
                }
            }
        }
        private double fare;
        public double Fare
        {
            get
            {
                if (fare > 0)
                {
                    return fare;
                }
                else
                {
                    calculatefare();
                    return fare;
                }
            }
        }

        private void calculatefare()
        {
            string[] fTriples = this.routeNo.FareCalcFormula.Split(',');
            string[] values;
            for (int i = 0; i < fTriples.Length - 1; i++)
            {
                values = fTriples[i].Split('-');
                if(Length >= Double.Parse(values[0]) && Length < Double.Parse(values[1]))
                {
                    this.fare = Double.Parse(values[2]);
                    return;
                }
            }
            values = fTriples[fTriples.Length - 1].Split('-');
            if (Length >= Double.Parse(values[0]) )
            {
                this.fare = Double.Parse(values[2]);
                return;
            }
        }

        private double approxTimeInMints;
        public double ApproxTimeInMints
        {
            get
            {
                if (approxTimeInMints > 0)
                {
                    return approxTimeInMints;
                }
                else
                {
                    calculateApproxTimeInMints();
                    return approxTimeInMints;
                }
            }

            
        }

        private void calculateApproxTimeInMints()
        {
            approxTimeInMints = Length / Double.Parse( routeNo.AvgSpeed.ToString()) * 60;// d /v = t
        }

        private void calculateLength()
        {
            length = stops.Select(x => x.DistanceFromPrevStop).Sum();

            length -= stops.Where(x => x.StopNo == stops.Select(y => y.StopNo).Min()).Select(x => x.DistanceFromPrevStop).Single();

        }
    }
}
