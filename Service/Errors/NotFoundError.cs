using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Errors
{
    public sealed class NotFoundError:IError
    {
        public NotFoundError(string? message = null)
        {
            Message = message ?? "Not Found";
        }

        public string? Message { get; private set; }

        public List<IError> Reasons => throw new NotImplementedException();

        public Dictionary<string, object> Metadata => throw new NotImplementedException();
    }
}
