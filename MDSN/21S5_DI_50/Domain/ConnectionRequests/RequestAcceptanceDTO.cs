using System.Collections.Generic;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class RequestAcceptanceDTO
    {
        public bool Answer { get; set; }
        public int ConnectionStrength { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public RequestAcceptanceDTO(bool answer, int connectionStrength, List<string> tags)
        {
            this.Answer = answer;
            this.ConnectionStrength = connectionStrength;
            this.Tags = tags;
        }
    }
}