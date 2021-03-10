using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;
using RestSharp.Serialization.Json;

namespace RestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://stockserver20201009223011.azurewebsites.net/");
            client.Authenticator = new HttpBasicAuthenticator("01149637@pw.edu.pl", "lab1scht");

            Console.WriteLine("Wybierz giełdę:");
            var gielda = Console.ReadLine();

            var request = new RestRequest("api/shareslist/" + gielda, DataFormat.Json);

            var responseJson = client.Get(request);

            Console.WriteLine("Lista dostęnych spółek (" + gielda + ")");

            string[] stockExchanges = JsonConvert.DeserializeObject<string[]>(responseJson.Content);

            foreach (string stockExchange in stockExchanges)

                //var deserialize = new JsonDeserializer();
                //var output = deserialize.Deserialize<Dictionary<string, string>>(responseJson);

                //Console.WriteLine(responseJson.Content);
                //Console.WriteLine("\n\n");
                Console.WriteLine(stockExchange);

            Console.ReadKey();
        }
    }
}
