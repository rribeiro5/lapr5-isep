using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Domain.ConnectionRequests
{
    [Owned]
    public class RequestMessage : IValueObject
    {

        public string Message { get; private set; }

        private const int MAX_LENGTH = 10000;

        private RequestMessage() {}

        public RequestMessage(string message)
        {
            if (message == null)
                throw new BusinessRuleValidationException("Message can't be null");
            if (message.Length > MAX_LENGTH)
                throw new BusinessRuleValidationException("Maximum of 10000 characters");
            
            this.Message = message;
        }
    }
}