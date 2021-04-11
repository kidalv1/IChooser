using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using Oef1.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Oef1
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = GetLines();
            Console.WriteLine("Voer je filter in");
            string filter = Console.ReadLine();
            var filterList = list.Where(x => x.Camera.Contains(filter));
            foreach (var item in filterList)
            {
                Console.WriteLine($"{item.Id} | {item.Camera} | {item.Latitude} | {item.Longitude}");
            }
            PostToApi(filterList.ToList());
            Console.ReadLine();

            
        }
        public static void PostToApi(List<Adres> data)
        {
            var client = new RestClient("https://localhost:44307/api/");
            var request = new RestRequest("addresses", Method.POST);
            string json = JsonConvert.SerializeObject(data);
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            request.RequestFormat = DataFormat.Json;
            request.AddBody(json);
            var response = client.Execute(request);
        }
        public static List<Adres> GetLines()
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var listOfAddresses = File.ReadLines($"{path}/data.txt").Skip(1).Select(line => ConvertToAders(line)).ToList();
            listOfAddresses.RemoveAll(x => x == null);
            return listOfAddresses;
        }

        public static Adres ConvertToAders(string record)
        {
            Adres address = null;
            var split = record.Split(';');
            if (split.Length > 2)
            {
                address = new Adres();
                address.Camera = split[0];
                address.Id = SplitCamera(address.Camera);
                address.Latitude = split[1];
                address.Longitude = split[2];
            }
            return address;

        }

        private static string SplitCamera(string camera)
        {
            string pattern = @"\d";
            string result = "";
            foreach (Match m in Regex.Matches(camera, pattern))
            {
                result = result + m;
            }
            return result;
        }
    }
}

