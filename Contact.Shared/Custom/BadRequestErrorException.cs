using System.Runtime.Serialization;

namespace Contact.Shared.Custom
{
    public class BadRequestErrorException : Exception
    {
        public BadRequestErrorException()
        {
        }

        public BadRequestErrorException(string? message) : base(message)
        {
        }

        public BadRequestErrorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BadRequestErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
