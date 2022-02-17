using System.Collections.Generic;

namespace DDDSample1.Domain.Connections
{
    public class UserConnectionsDTO
    {
        public List<ConnectionDTO> Connections { get; set; }

        public UserConnectionsDTO(List<ConnectionDTO> conns) 
        {
            this.Connections = new List<ConnectionDTO>(conns);
        }
    }
}