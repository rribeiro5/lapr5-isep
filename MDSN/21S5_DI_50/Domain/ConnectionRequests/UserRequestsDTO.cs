using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class UserRequestsDTO
    {
        public List<ConnectionRequestDTO> Requests { get; set; }

        public UserRequestsDTO(List<ConnectionRequestDTO> requests)
        {
            this.Requests = new List<ConnectionRequestDTO>(requests);
        }

        public override bool Equals(object obj)
        {
            return obj is UserRequestsDTO dTO &&
                   EqualityComparer<List<ConnectionRequestDTO>>.Default.Equals(Requests, dTO.Requests);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Requests);
        }
    }
}