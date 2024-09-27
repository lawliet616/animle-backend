using Animle.classes;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace Animle.Helpers
{
    public class MyanimeListClientHttpService
    {
        private readonly ConfigSettings _appSettings;

        public MyanimeListClientHttpService(IOptions<ConfigSettings> options)
        {
            _appSettings = options.Value;
        }
        async public Task<string> ReturnAny(string subUrl, string apiUrl = "https://api.myanimelist.net/v2/")
        {
            HttpClient client = new HttpClient();


            apiUrl += subUrl;

            try
            {

                string malId = _appSettings.MalId;

                client.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", malId);

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
