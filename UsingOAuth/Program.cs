using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System;
using System.Threading.Tasks;

namespace UsingOAuth
{
    class Program
    {
        // O365 User Name and Password
        private const string userName = "satya@bizappsmar19.onmicrosoft.com";
        private const string password = "";
        // D365 Application Url 
        private const string serviceUrl = "https://bizappsmar19.api.crm8.dynamics.com";
        // Azure APP Application Id
        private const string applicationId = "xxxxxxx-6051-415e-8729-c690a151e7dd";
        // Redirct Uri specified during registration of application
        private const string RedirectUri = "https://localhost";
        // OAuth 2.0 Authorization Endpoint copied from Azure APP
        private const string authorityUri = "https://login.microsoftonline.com/xxxx-a189-4ee4-908e-fd6b9a467e7d/oauth2/authorize";
        private static AuthenticationResult authResult = null;
        static void Main(string[] args)
        {
            // Code to connect to D365
            var credentials = new UserPasswordCredential(userName, password);
            var context = new AuthenticationContext(authorityUri);
            authResult = context.AcquireTokenAsync(serviceUrl, applicationId, credentials).Result;
             Task.WaitAll(Task.Run(async () => await ExecuteWhoAmI()));
            
        }

        private static async Task ExecuteWhoAmI()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(serviceUrl),
                Timeout = new TimeSpan(0, 2, 0)
            };
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            // Add this line for TLS complaience
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Call WhoAmI
            var retrieveResponse = await httpClient.GetAsync("api/data/v9.1/WhoAmI");
            if (retrieveResponse.IsSuccessStatusCode)
            {
                var jRetrieveResponse = JObject.Parse(retrieveResponse.Content.ReadAsStringAsync().Result);
                var currUserId = (Guid)jRetrieveResponse["UserId"];
                var businessId = (Guid)jRetrieveResponse["BusinessUnitId"];
                Console.WriteLine("My User Id – " + currUserId);
                Console.WriteLine("My User Id – " + businessId);
                Console.ReadLine();
            }
        }
    }
}
