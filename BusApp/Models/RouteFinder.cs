using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusApp.Models
{
    public class RouteFinder
    {
        Hashtable passageList = new Hashtable();
        private PublicTransportRouteFinderEntities db = new PublicTransportRouteFinderEntities();
        
        public RouteFinder()
        {
            //Construct the passage list
            foreach (Stop  stop in db.Stops)
            {
                //select the routes that pass phrough this stop
                List<Decimal> routesList = db.RouteDetails.Where(x => x.StopID == stop.ID).Select(x => x.RouteID).ToList();
                passageList.Add(stop.ID, routesList);
            }
        }
        private ResultComponent createResultComponent(decimal route, Stop firstStop, Stop lastStop)
        {
            int graterStopNo = db.RouteDetails.Where(x => x.RouteID == route && x.StopID == firstStop.ID ).Select(x => x.StopNo).SingleOrDefault(),
                smallerStopNo = db.RouteDetails.Where(x => x.RouteID == route && x.StopID == lastStop.ID).Select(x => x.StopNo).SingleOrDefault();
            if(graterStopNo < smallerStopNo)
            {
                //Swaping the values
                int tmp = graterStopNo;
                graterStopNo = smallerStopNo;
                smallerStopNo = tmp;
            }
            
            ResultComponent createdResultComponent = new ResultComponent();
             Route route1 = db.Routes.Where(x => x.ID == route).Select(x => x).SingleOrDefault();
            createdResultComponent.routeNo = new RouteForJson(route1);
            List<RouteDetail> routeDetailList = db.RouteDetails.Where
                (x => x.RouteID == route && x.StopNo >= smallerStopNo && x.StopNo <= graterStopNo).OrderBy(x => x.StopNo).ToList();
            List<RouteDetailForJson> routeDtlForJsonLst = new List<RouteDetailForJson>();
            foreach (var routeDetail in routeDetailList)
            {
                routeDtlForJsonLst.Add(new RouteDetailForJson(routeDetail));
            }
            createdResultComponent.stops = routeDtlForJsonLst;
            int totalStopsToTravel = createdResultComponent.stops.Count;


            if (createdResultComponent.stops[0].StopID == firstStop.ID && createdResultComponent.stops[totalStopsToTravel - 1].StopID == lastStop.ID)
            {
                createdResultComponent.journeyDirection = JourneyDirection.UP;
            }
            else if(createdResultComponent.stops[totalStopsToTravel - 1].StopID == firstStop.ID && createdResultComponent.stops[0].StopID == lastStop.ID)
            {
                createdResultComponent.journeyDirection = JourneyDirection.DOWN;
            }


            return createdResultComponent;
        }
        public List<RouteSearchResult> find(FindRouteInput input)
        {
            List<RouteSearchResult> resultRoutesList = new List<RouteSearchResult>();
            RouteSearchResult currentResultRoute = new RouteSearchResult();
            List<Decimal> destRoutes = (List<Decimal>)passageList[input.destinationPoint.ID];

            //case one --when single bus goes from start to destination.
            foreach (decimal route in (List<Decimal>)passageList[input.startPoint.ID])
            {
                 if(destRoutes.Contains(route))
                {
                    ResultComponent resComp = createResultComponent(route, input.startPoint, input.destinationPoint);
                    resComp.sequenceNo = currentResultRoute.busesToTake.Count;
                    currentResultRoute.busesToTake.Add(resComp);
                    resultRoutesList.Add(currentResultRoute);
                    currentResultRoute = new RouteSearchResult();
                }
            }
            //case Two --when passenger can go from start to destination by switching a bus in the safar.
            foreach (decimal startPointRoute in (List<Decimal>)passageList[input.startPoint.ID])
            {
                foreach (var stopOfStartPointRoute in db.RouteDetails.Where(x=>x.RouteID == startPointRoute))
                {
                    if (stopOfStartPointRoute.StopID == input.startPoint.ID || stopOfStartPointRoute.StopID == input.destinationPoint.ID)
                    {
                        ///int n = 5;
                        continue;
                    }
                    foreach (decimal routeOfPossibleSwitchPoint in (List<Decimal>)passageList[stopOfStartPointRoute.StopID])
                    {
                        if (startPointRoute == routeOfPossibleSwitchPoint)
                            continue;
                        if (destRoutes.Contains(routeOfPossibleSwitchPoint))
                        {
                            ResultComponent resComp1 = createResultComponent(startPointRoute, input.startPoint, stopOfStartPointRoute.Stop);
                            resComp1.sequenceNo = currentResultRoute.busesToTake.Count;
                            currentResultRoute.busesToTake.Add(resComp1);
                            ResultComponent resComp2 = createResultComponent(routeOfPossibleSwitchPoint, stopOfStartPointRoute.Stop, input.destinationPoint);
                            resComp2.sequenceNo = currentResultRoute.busesToTake.Count;
                            currentResultRoute.busesToTake.Add(resComp2);
                            resultRoutesList.Add(currentResultRoute);
                            currentResultRoute = new RouteSearchResult();
                        }
                    }

                }
            }

            return resultRoutesList;
        }

    }
}