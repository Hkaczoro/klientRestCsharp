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
using System.Globalization;

namespace Rest
{
    public class SharePrice
    {
        public int time { get; set; }
        public string price { get; set; }
        public string buySell { get; set; }
        public string amount { get; set; }

    }

    public class User
    {
        public string username { get; set; }
        public float funds { get; set; }
        public Dictionary<string, int> shares { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Połączenie z serwerem i wysłanie pytania o akcje ALIORa
            var client = new RestClient("https://zsutstockserver.azurewebsites.net");
            client.Authenticator = new HttpBasicAuthenticator("01149637@pw.edu.pl", "lab1scht");
            var request = new RestRequest("/api/shareprice/Warszawa?share=11BIT", DataFormat.Json);
            var responseJson = client.Get(request);
            SharePrice[] sharePrices = JsonConvert.DeserializeObject<SharePrice[]>(responseJson.Content);



                //Wysłanie oferty sprzedaży akcji
                var postRequest = new RestRequest("/api/buyoffer", Method.POST);
                string cena = sharePrices[1].price;
                Console.WriteLine("Cena zakupu akcji: " + cena);
                double fCena = Convert.ToDouble(cena, CultureInfo.InvariantCulture);
                postRequest.AddJsonBody(new
                {
                    stockExchange = "Warszawa",
                    share = "11BIT",
                    amount = 1,
                    price = fCena
                });
                IRestResponse response = client.Execute(postRequest);
                var content = response.Content;

                //Wysłanie zapytania o dane klienta
                var clientRequest = new RestRequest("/api/client", DataFormat.Json);
                var clientResponse = client.Get(clientRequest);
                User user = JsonConvert.DeserializeObject<User>(clientResponse.Content);
                Dictionary<string, int> akcje = new Dictionary<string, int>();
                akcje = user.shares;

                //Sprzedaż wszystkich akcji użytkownika
                int akcjeBita = akcje["11BIT"];
                var sellRequest = new RestRequest("/api/selloffer", Method.POST);
                string cenaSprzedazy = sharePrices[0].price;
                double dCenaSprzedazy = Convert.ToDouble(cenaSprzedazy, CultureInfo.InvariantCulture);
                Console.WriteLine("Cena sprzedaży akcji: " + dCenaSprzedazy);
                sellRequest.AddJsonBody(new
                {
                    stockExchange = "Warszawa",
                    share = "11BIT",
                    amount = akcjeBita,
                    price = dCenaSprzedazy
                });
                IRestResponse sellResponse = client.Execute(sellRequest);
            

            
        }

    }

}

