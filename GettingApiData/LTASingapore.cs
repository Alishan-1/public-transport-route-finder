using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GettingApiData
{
    public class LTASingapore
    {
        static HttpClient client = new HttpClient();
        public static RootObject busStopsres;
        public static RootObject test()
        {
            RunAsync().Wait();
            return busStopsres;
        }
        static async Task RunAsync()
        {
            /////For stops///////
            /*
            client.BaseAddress = new Uri("http://datamall2.mytransport.sg/ltaodataservice/BusStops");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccountKey", "sJKkdal2TbilqmsbNvheQA==");

            RootObject busStops = await GetStopsAndInsertIntoDB_DelOldStopsAsync(client.BaseAddress.PathAndQuery).ConfigureAwait(false);
            busStopsres = busStops;
            */

            //////For Bus Services/////
            /*
            client.BaseAddress = new Uri("http://datamall2.mytransport.sg/ltaodataservice/BusServices");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccountKey", "sJKkdal2TbilqmsbNvheQA==");
            
            bool result = await GetBusServicesAndInsertIntoDB_DelOldBusServicesAsync(client.BaseAddress.PathAndQuery).ConfigureAwait(false);
            */
            //////For Bus route detail/////
            client.BaseAddress = new Uri("http://datamall2.mytransport.sg/ltaodataservice/BusRoutes");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AccountKey", "sJKkdal2TbilqmsbNvheQA==");

            bool result = await GetBusRouteDetailAndInsertIntoDBAsync(client.BaseAddress.PathAndQuery).ConfigureAwait(false);


        }
        static async Task<RootObject> GetStopsAndInsertIntoDB_DelOldStopsAsync(string path)
        {
            RootObject busStops = null;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                busStops = JsonConvert.DeserializeObject<RootObject>(jsonString);
            }
            //return busStops;
            string connectionString = @"data source=ALISHAN\SQLEXPRESS14;initial catalog=PublicTransportRouteFinder;integrated security=True;";
            SqlConnection conn = new SqlConnection(connectionString);
            for (int i = 0; i < busStops.value.Count; i++)
            {
                string stopQuery = @"SELECT ID " +
                " FROM Stops WHERE BusStopCode = @stopID ";
                SqlCommand cmd = new SqlCommand(stopQuery, conn);
                cmd.Parameters.Add("stopID", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode;
                SqlDataAdapter presDa = new SqlDataAdapter(cmd);
                DataSet presds = new DataSet();
                presDa.Fill(presds);
                if (presds.Tables[0].Rows.Count > 0)
                {
                    string deleteQry = "delete from Stops where BusStopCode = @stopID";
                    cmd = new SqlCommand(deleteQry, conn);
                    cmd.Parameters.Add("stopID", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                string insertQry = @"INSERT INTO [dbo].[Stops]([CityID],[Name],[Discription],[Latitude],[Longitude],[RoadName],[seqNo],[BusStopCode])" +
                                        @"VALUES      (65, @Description, @longDesc, " +
                                        busStops.value[i].Latitude + ", " + busStops.value[i].Longitude + ", @RoadName, " + (i + 1) +
                                        ", '" + busStops.value[i].BusStopCode + "')";
                cmd = new SqlCommand(insertQry, conn);
                cmd.Parameters.Add("Description", SqlDbType.VarChar).Value = busStops.value[i].Description;
                cmd.Parameters.Add("longDesc", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode + " - " + busStops.value[i].Description + " - " + busStops.value[i].RoadName;
                cmd.Parameters.Add("RoadName", SqlDbType.VarChar).Value = busStops.value[i].RoadName;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }



            int skip = 50;
            while(busStops.value.Count > 0)
            {
                busStops = null;
                response = null;
                response = await client.GetAsync(path + @"?$skip="+skip).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    busStops = JsonConvert.DeserializeObject<RootObject>(jsonString);
                }
                

                for (int i = 0; i < busStops.value.Count; i++)
                {
                    string stopQuery = @"SELECT ID " +
                    " FROM Stops WHERE BusStopCode = @stopID ";
                    SqlCommand cmd = new SqlCommand(stopQuery, conn);
                    cmd.Parameters.Add("stopID", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode;
                    SqlDataAdapter presDa = new SqlDataAdapter(cmd);
                    DataSet presds = new DataSet();
                    presDa.Fill(presds);
                    if (presds.Tables[0].Rows.Count > 0)
                    {
                        string deleteQry = "delete from Stops where BusStopCode = @stopID";
                        cmd = new SqlCommand(deleteQry, conn);
                        cmd.Parameters.Add("stopID", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    string insertQry = @"INSERT INTO [dbo].[Stops]([CityID],[Name],[Discription],[Latitude],[Longitude],[RoadName],[seqNo],[BusStopCode])" +
                                        @"VALUES      (65, @Description, @longDesc, " +
                                        busStops.value[i].Latitude + ", " + busStops.value[i].Longitude + ", @RoadName, " + (i+ skip + 1) +
                                        ", '" + busStops.value[i].BusStopCode + "')";
                    cmd = new SqlCommand(insertQry, conn);
                    cmd.Parameters.Add("Description", SqlDbType.VarChar).Value = busStops.value[i].Description;
                    cmd.Parameters.Add("longDesc", SqlDbType.VarChar).Value = busStops.value[i].BusStopCode + " - " + busStops.value[i].Description + " - " + busStops.value[i].RoadName;
                    cmd.Parameters.Add("RoadName", SqlDbType.VarChar).Value = busStops.value[i].RoadName;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                skip += 50;
                System.Diagnostics.Debug.WriteLine(skip +" "+ DateTime.Now.TimeOfDay.ToString());
            }

            return busStops;
        }

        static async Task<bool> GetBusServicesAndInsertIntoDB_DelOldBusServicesAsync(string path)
        {
            BusServicesRootObject BusServices = null;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                BusServices = JsonConvert.DeserializeObject<BusServicesRootObject>(jsonString);
            }
            else
            {
                throw new Exception("Error status code " + response.StatusCode.ToString());
            }
            //return busStops;
            string connectionString = @"data source=ALISHAN\SQLEXPRESS14;initial catalog=PublicTransportRouteFinder;integrated security=True;";
            SqlConnection conn = new SqlConnection(connectionString);
            for (int i = 0; i < BusServices.value.Count; i++)
            {
                string BusServicesQuery = @"SELECT ID " +
                " FROM Routes WHERE Name = @Name and CityID = 65 and Direction = @Direction  ";
                SqlCommand cmd = new SqlCommand(BusServicesQuery, conn);
                cmd.Parameters.Add("Name", SqlDbType.VarChar).Value = BusServices.value[i].ServiceNo;
                cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = BusServices.value[i].Direction;
                SqlDataAdapter presDa = new SqlDataAdapter(cmd);
                DataSet presds = new DataSet();
                presDa.Fill(presds);
                if (presds.Tables[0].Rows.Count > 0)
                {
                    string deleteQry = "delete from Routes WHERE Name = @Name and CityID = 65 and Direction = @Direction  ";
                    cmd = new SqlCommand(deleteQry, conn);
                    cmd.Parameters.Add("Name", SqlDbType.VarChar).Value = BusServices.value[i].ServiceNo;
                    cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = BusServices.value[i].Direction;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    System.Diagnostics.Debug.WriteLine( "deleted " + BusServices.value[i].ServiceNo+"  "+ BusServices.value[i].Direction);
                    cmd.Connection.Close();
                }
                string insertQry = @"INSERT INTO [dbo].[Routes]
           ([CityID],[Name],[Status],[FareCalcFormula],[TypeID],[AvgSpeed],[Operator],[Direction],[Category],[OriginCode],[DestinationCode],[AM_Peak_Freq],[AM_Offpeak_Freq],[PM_Peak_Freq],[PM_Offpeak_Freq],[LoopDesc]) VALUES"+
           "(65,'"+ BusServices.value[i].ServiceNo + @"','A','',1,30,'" + BusServices.value[i].Operator + @"','" + BusServices.value[i].Direction + @"','" + BusServices.value[i].Category + @"','" + BusServices.value[i].OriginCode + @"','" + BusServices.value[i].DestinationCode + @"','" + BusServices.value[i].AM_Peak_Freq + @"' ,'" +
           BusServices.value[i].AM_Offpeak_Freq + @"','" + BusServices.value[i].PM_Peak_Freq + @"','" + BusServices.value[i].PM_Offpeak_Freq + @"',@LoopDesc )";
                cmd = new SqlCommand(insertQry, conn);
                if(BusServices.value[i].LoopDesc == null)
                {
                    cmd.Parameters.Add("LoopDesc", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("LoopDesc", SqlDbType.VarChar).Value = BusServices.value[i].LoopDesc;
                }
                
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }



            int skip = 50;
            while (BusServices.value.Count > 0)
            {
                BusServices = null;
                response = null;
                response = await client.GetAsync(path + @"?$skip=" + skip).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    BusServices = JsonConvert.DeserializeObject<BusServicesRootObject>(jsonString);
                }


                for (int i = 0; i < BusServices.value.Count; i++)
                {
                    string BusServicesQuery = @"SELECT ID " +
                " FROM Routes WHERE Name = @Name and CityID = 65 and Direction = @Direction  ";
                    SqlCommand cmd = new SqlCommand(BusServicesQuery, conn);
                    cmd.Parameters.Add("Name", SqlDbType.VarChar).Value = BusServices.value[i].ServiceNo;
                    cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = BusServices.value[i].Direction;
                    SqlDataAdapter presDa = new SqlDataAdapter(cmd);
                    DataSet presds = new DataSet();
                    presDa.Fill(presds);
                    if (presds.Tables[0].Rows.Count > 0)
                    {
                        string deleteQry = "delete from Routes WHERE Name = @Name and CityID = 65 and Direction = @Direction  ";
                        cmd = new SqlCommand(deleteQry, conn);
                        cmd.Parameters.Add("Name", SqlDbType.VarChar).Value = BusServices.value[i].ServiceNo;
                        cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = BusServices.value[i].Direction;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("deleted " + BusServices.value[i].ServiceNo + "  " + BusServices.value[i].Direction);
                        cmd.Connection.Close();
                    }
                    string insertQry = @"INSERT INTO [dbo].[Routes]
           ([CityID],[Name],[Status],[FareCalcFormula],[TypeID],[AvgSpeed],[Operator],[Direction],[Category],[OriginCode],[DestinationCode],[AM_Peak_Freq],[AM_Offpeak_Freq],[PM_Peak_Freq],[PM_Offpeak_Freq],[LoopDesc]) VALUES" +
           "(65,'" + BusServices.value[i].ServiceNo + @"','A','',1,30,'" + BusServices.value[i].Operator + @"','" + BusServices.value[i].Direction + @"','" + BusServices.value[i].Category + @"','" + BusServices.value[i].OriginCode + @"','" + BusServices.value[i].DestinationCode + @"','" + BusServices.value[i].AM_Peak_Freq + @"' ,'" +
           BusServices.value[i].AM_Offpeak_Freq + @"','" + BusServices.value[i].PM_Peak_Freq + @"','" + BusServices.value[i].PM_Offpeak_Freq + @"',@LoopDesc )";
                    cmd = new SqlCommand(insertQry, conn);
                    if (BusServices.value[i].LoopDesc == null)
                    {
                        cmd.Parameters.Add("LoopDesc", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("LoopDesc", SqlDbType.VarChar).Value = BusServices.value[i].LoopDesc;
                    }
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                skip += 50;
                System.Diagnostics.Debug.WriteLine(skip + " " + DateTime.Now.TimeOfDay.ToString());
            }

            return true;
        }

        static async Task<bool> GetBusRouteDetailAndInsertIntoDBAsync(string path)
        {
            RouteDtlRootObj BusRoutesDetails = null;
            HttpResponseMessage response = await client.GetAsync(path).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                BusRoutesDetails = JsonConvert.DeserializeObject<RouteDtlRootObj>(jsonString);
            }
            else
            {
                throw new Exception("Error status code " + response.StatusCode.ToString());
            }
            string connectionString = @"data source=ALISHAN\SQLEXPRESS14;initial catalog=PublicTransportRouteFinder;integrated security=True;";
            SqlConnection conn = new SqlConnection(connectionString);

            foreach(var item in BusRoutesDetails.value)
            {
                
                string insertQry = @"INSERT INTO [dbo].[RouteDetail]([StopNo],[ServiceNo]
                   ,[Operator],[Direction],[BusStopCode],[Distance],[WD_FirstBus],[WD_LastBus],[SAT_FirstBus],[SAT_LastBus],[SUN_FirstBus],[SUN_LastBus]) VALUES"+
                   "("+ item.StopSequence + ",'" + item.ServiceNo + "','" + item.Operator+ "'," + item.Direction+ ",'" + item.BusStopCode+ "'," + item.Distance+ ",'" + item.WD_FirstBus+
                   "','" + item.WD_LastBus+ "','" + item.SAT_FirstBus+ "','" + item.SAT_LastBus+ "','" + item.SUN_FirstBus+ "','" + item.SUN_LastBus+"')";
                SqlCommand cmd = new SqlCommand(insertQry, conn);
                

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();

            }



            int skip = 50;
            while (BusRoutesDetails.value.Count > 0)
            {
                BusRoutesDetails = null;
                response = null;
                response = await client.GetAsync(path + @"?$skip=" + skip).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    BusRoutesDetails = JsonConvert.DeserializeObject<RouteDtlRootObj>(jsonString);
                }


                foreach (var item in BusRoutesDetails.value)
                {

                    string insertQry = @"INSERT INTO [dbo].[RouteDetail]([StopNo],[ServiceNo]"+
                        ",[Operator],[Direction],[BusStopCode],[Distance],[WD_FirstBus],[WD_LastBus],[SAT_FirstBus],[SAT_LastBus],[SUN_FirstBus],[SUN_LastBus]) VALUES" +
                        "(@StopSequence ,'" + item.ServiceNo + "','" + item.Operator + "',@Direction ,'" + item.BusStopCode + "',@Distance ,'" + item.WD_FirstBus +
                        "','" + item.WD_LastBus + "','" + item.SAT_FirstBus + "','" + item.SAT_LastBus + "','" + item.SUN_FirstBus + "','" + item.SUN_LastBus + "')";
                    SqlCommand cmd = new SqlCommand(insertQry, conn);
                    if (item.StopSequence == null)
                    {
                        cmd.Parameters.Add("StopSequence", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("StopSequence", SqlDbType.VarChar).Value = item.StopSequence;
                    }
                    if (item.Direction == null)
                    {
                        cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("Direction", SqlDbType.VarChar).Value = item.Direction;
                    }
                    if (item.Distance == null)
                    {
                        cmd.Parameters.Add("Distance", SqlDbType.VarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("Distance", SqlDbType.VarChar).Value = item.Distance;
                    }
                    
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                skip += 50;
                System.Diagnostics.Debug.WriteLine(skip + " " + DateTime.Now.TimeOfDay.ToString());
            }

            return true;
        }

    }




    public class BusStop
    {
        public string BusStopCode { get; set; }
        public string RoadName { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RootObject
    {
        public string odata_metadata { get; set; }
        public List<BusStop> value { get; set; }
    }

    public class BusServices
    {
        public string ServiceNo { get; set; }
        public string Operator { get; set; }
        public int Direction { get; set; }
        public string Category { get; set; }
        public string OriginCode { get; set; }
        public string DestinationCode { get; set; }
        public string AM_Peak_Freq { get; set; }
        public string AM_Offpeak_Freq { get; set; }
        public string PM_Peak_Freq { get; set; }
        public string PM_Offpeak_Freq { get; set; }
        public string LoopDesc { get; set; }
    }

    public class BusServicesRootObject
    {
        public string odata_metadata { get; set; }
        public List<BusServices> value { get; set; }
    }


    public class RouteDetail
    {
        public string ServiceNo { get; set; }
        public string Operator { get; set; }
        public int? Direction { get; set; }
        public int? StopSequence { get; set; }
        public string BusStopCode { get; set; }
        public double? Distance { get; set; }
        public string WD_FirstBus { get; set; }
        public string WD_LastBus { get; set; }
        public string SAT_FirstBus { get; set; }
        public string SAT_LastBus { get; set; }
        public string SUN_FirstBus { get; set; }
        public string SUN_LastBus { get; set; }
    }

    public class RouteDtlRootObj
    {
        public string odata_metadata { get; set; }
        public List<RouteDetail> value { get; set; }
    }
}
