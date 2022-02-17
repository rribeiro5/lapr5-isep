using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public class CreatePostResponseDTO
    {
        public string id { get; set; }
        public Guid userId{ get;set; }
        public string text{ get;set; }
        public ICollection<string> tags{ get;set; }
        
        public ICollection<ReactionDTO> reactions{ get;set; }
        
        public ICollection<CommentDTO> comments{ get;set; }
        
        public long creationDateTime{ get; set; }

        public CreatePostResponseDTO(Guid userId, string text, ICollection<string> tags,long creationDateTime,
            ICollection<ReactionDTO> reactions, ICollection<CommentDTO>comments)
        {
            this.userId = userId;
            this.text = text;
            this.tags = tags;
            this.creationDateTime = creationDateTime;
            this.reactions = reactions;
            this.comments = comments;
        }
    }
}