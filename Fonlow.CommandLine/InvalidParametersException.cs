using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fonlow.CommandLine
{
    [Serializable]
    public class InvalidParametersException : Exception
    {
        public InvalidParametersException():base()
        {

        }

        public InvalidParametersException(string message):base(message) { }


        protected InvalidParametersException(SerializationInfo info, StreamingContext context) : base(info, context) { }


        public InvalidParametersException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

}
