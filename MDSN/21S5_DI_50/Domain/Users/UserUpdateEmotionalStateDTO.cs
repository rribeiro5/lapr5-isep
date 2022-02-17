using System;

namespace DDDSample1.Domain.Users
{
    public class UserUpdateEmotionalStateDTO
    {
        public Guid Id { get; set; }
        
        public string State { get; set; }

        public UserUpdateEmotionalStateDTO(Guid id, string state)
        {
            this.Id = id;
            this.State = state;
        }
    }
}