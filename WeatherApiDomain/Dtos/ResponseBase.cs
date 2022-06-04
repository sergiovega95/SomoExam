using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApiDomain.Dtos
{
    public class ResponseBase
    {
        public bool IsSucessFull { get; set; }       

        public string Error { get; set; }
    }
}
