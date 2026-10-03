using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Domain.Exceptions
{
    public class BadRequestCustomException : Exception
    {
        public readonly IEnumerable<string>? Errors;

        public BadRequestCustomException(string msg,IEnumerable<string>? errors=null):base(msg)
        {
            Errors = errors;   
        }
    }
}
