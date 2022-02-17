using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DDDSample1.Domain.Posts
{
    public class CreatingCommentResponseDTO
    {
        public Guid id{ get; set;}
        public string postId{ get; set;}
        public string userId{ get; set;}
        public string text{ get; set;}
        public ICollection<ReactionDTO> reactions{ get; set;}
        public long creationDateTime{ get; set; }

        
        public CreatingCommentResponseDTO(Guid id,string postId, string userId, string text, ICollection<ReactionDTO> reactions, long creationDateTime)
        {
            this.id = id;
            this.postId = postId;
            this.userId = userId;
            this.text = text;
            this.reactions = reactions;
            this.creationDateTime = creationDateTime;
        }
    }
}