using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApiDomain.Dtos
{
    public class ResponseWeather<T>: ResponseBase 
    {
        public string Detail { get; set; }

        public T Data {get;set;}
    }
}
