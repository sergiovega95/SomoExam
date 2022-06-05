using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApiDomain.Exceptions;
using WeatherApiDomain.Interfaces.ExternalServices;

namespace WeatherApiInfraestructure.Implementations
{
    public class ConsumerServices:IConsumerServices
    {
        private readonly IHttpClientFactory _clientFactory;  

        public ConsumerServices(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        //TODO: With more time you can implement a named client for httpclient factory

        public async Task<T> Get<T>(string url, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,url);
            
            foreach (var value in headers)
            {
                request.Headers.Add(value.Key, value.Value);
            }
          
            var client = _clientFactory.CreateClient();            
            var response = await client.SendAsync(request);

            if (response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var payload = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(payload);
            }
            else
            {               
                throw new ExternalServiceException(await response.Content.ReadAsStringAsync(),response.StatusCode);
            }
        }

        public async Task Post<T>(string url , T body)  where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content= new StringContent(JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json");            
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ExternalServiceException(await response.Content.ReadAsStringAsync());
            }            
        }
       
    }
}
