using FluentResults;

namespace Service.Errors
{
    public class ValidationError(string? message = null) : IError
    {
        public List<IError> Reasons => throw new NotImplementedException();

        public string? Message { get; private set; } = message ?? "Error";

        public Dictionary<string, object> Metadata { get; private set; } = [];

        public ValidationError WithMetadata(string name, object value)
        {
            Metadata.Add(name, value);
            return this;
        }
    }
}
