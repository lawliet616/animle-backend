using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace Animle.services
{
    public class MyanimeListClientHttpService
    {
        async public Task<string> ReturnAny(string subUrl, string apiUrl = "https://api.myanimelist.net/v2/")
        {
            HttpClient client = new HttpClient();

            apiUrl += subUrl;

            try
            {
                var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json")
                    .Build();

                client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", "5ab79100e2772855f94a8372f5863c36");
            
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    return responseBody;

                }
                else
                {
                    Console.WriteLine("Mi a tő"); ;

                    return null;

                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
