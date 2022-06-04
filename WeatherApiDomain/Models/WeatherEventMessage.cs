using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApiDomain.Models
{
    public class WeatherEventMessage
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public WeatherAPI.Standard.Models.CurrentJsonResponse CurrentWeather { get; set; }
    }
}
