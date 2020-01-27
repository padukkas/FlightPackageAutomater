using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodingExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            OutputUserStoryOne();
            OutputUserStoryTwo();
            Console.ReadLine();
        }

        public static void OutputUserStoryOne()
        {
            int days = 2;
            int planes = 3;
            int flightCounter = 1;
            for (int day = 1; day <= days; day++)
            {
                for (int plane = 1; plane <= planes; plane++)
                {
                    string arrival = "YYZ";
                    if (plane == 2) arrival = "YYC";
                    if (plane == 3) arrival = "YVR";
                    Console.WriteLine($"Flight: {flightCounter}, departure: YUL, arrival: {arrival}, day: {day}");
                    flightCounter++;
                }
            }
        }

        public static void OutputUserStoryTwo()
        {
            List<Item> items = ParseJson();

            foreach (var item in items)
            {
                if (item.Scheduled)
                {
                    Console.WriteLine($"order: {item.OrderNumber}, flightNumber: {item.FlightNumer}, depature: {item.DepartuerCity}, arrival: {item.ArrivalCity}, day: {item.ArrivalDay}");
                }
                else
                {
                    Console.WriteLine($"order: {item.OrderNumber}, flightNumber: not scheduled");
                }
            }

        }

        public static List<Item> ParseJson()
        {
            List<Item> items = new List<Item>();

            int yyzOrders = 0;
            int yycOrders = 0;
            int yvrOrders = 0;

            bool scheduled;

            int flightNumber = 0;
            int yyzFlightNumber = 1;
            int yycFlightNumber = 2;
            int yvrFlightNumber = 3;
            foreach (var each in LoadJson())
            {
                scheduled = true;
                switch (each.Value.First().Value)
                {
                    case "YYZ":
                        yyzOrders += 1;
                        if (yyzOrders > 20)
                        {
                            yyzFlightNumber += 3;
                            yyzOrders = 0;
                        }
                        flightNumber = yyzFlightNumber;
                        break;
                    case "YYC":
                        yycOrders += 1;
                        if (yycOrders > 20)
                        {
                            yycFlightNumber += 3;
                            yycOrders = 0;
                        }
                        flightNumber = yycFlightNumber;
                        break;
                    case "YVR":
                        yvrOrders += 1;
                        if (yvrOrders > 20)
                        {
                            yvrFlightNumber += 3;
                            yvrOrders = 0;
                        }
                        flightNumber = yvrFlightNumber;
                        break;
                    default:
                        scheduled = false;
                        break;
                }
                items.Add(new Item { FlightNumer = flightNumber, ArrivalCity = each.Value.First().Value, OrderNumber = each.Key, Scheduled = scheduled, ArrivalDay = flightNumber > 3 ? 2 : 1 });
            }
            return items;
        }

        public static Dictionary<string, Dictionary<string, string>> LoadJson()
        {
            using (StreamReader r = new StreamReader("codingoutput.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            }
        }

        public class Item
        {
            public int FlightNumer { get; set; }
            public bool Scheduled { get; set; }
            public string DepartuerCity { get; set; } = "YUL";
            public string ArrivalCity { get; set; }
            public int ArrivalDay { get; set; }
            public string OrderNumber { get; set; }
        }
    }
}
