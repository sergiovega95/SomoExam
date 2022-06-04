using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApiDomain.Interfaces.ExternalServices
{
    public interface IConsumerServices
    {
        Task<T> Get<T>(string url, Dictionary<string, string> headers);

        Task Post<T>(string url, T body) where T : class;
    }
}
