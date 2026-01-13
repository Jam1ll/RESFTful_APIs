using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace core.application.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException() : base() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, params object[] args) : 
            base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            
        }
    }
}
