using System.Collections.Generic;

namespace DDDSample1.Domain.Connections
{
    public class ChangeConnInfoDTO
    {
        public int ConnectionStrength { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public ChangeConnInfoDTO(int connStrength, List<string> tags) 
        {
            this.ConnectionStrength = connStrength;
            this.Tags = tags;
        }
    }
}