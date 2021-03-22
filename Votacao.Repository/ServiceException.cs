using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votacao.Repository
{
    public sealed class ServiceException : Exception
    {
        public string Title { get; }

        public ServiceException(string message) : base(message) 
        {
            Title = string.Empty;
        }

        public ServiceException(string title, string message) : base(message)
        {
            Title = title;
        }

        public ServiceException(string title, string message, Exception innerException) : base(message, innerException)
        {
            Title = title;
        }
    }
}
