using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public class CreatingReactionDTO
    {
        public string publicationId { get; set; }
        public Guid userId{ get; set;}
        public string reaction{ get; set;}
        
        public CreatingReactionDTO(string _publicationId, Guid _userId, string _reaction)
        {
            this.userId = _userId;
            this.publicationId = _publicationId;
            this.reaction = _reaction;
        }
    }
}