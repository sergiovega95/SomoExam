using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApiDomain.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public int statusCode;

        public  ExternalServiceException(string message):base(message)
        {
          
        }

        public ExternalServiceException(string message, System.Net.HttpStatusCode code) : base(message)
        {
            statusCode = (int)code;
        }

        public ExternalServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
