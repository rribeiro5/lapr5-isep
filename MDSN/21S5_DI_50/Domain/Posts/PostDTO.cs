using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public class PostDTO
    {
        public string id { get; set; }
        public Guid userId{ get; set; }
        public string text{ get; set; }
        public ICollection<string> tags{ get; set; }
        public ICollection<CommentDTO> comments { get; set; }
        public ICollection<ReactionDTO> reactions { get; set; }
        public long creationDateTime{ get; set; }

        public PostDTO(Guid userId, string text, ICollection<string> tags, ICollection<CommentDTO> comments, ICollection<ReactionDTO> reactions,long creationDateTime)
        {
            this.userId = userId;
            this.text = text;
            this.tags = tags;
            this.comments = comments;
            this.reactions = reactions;
            this.creationDateTime = creationDateTime;
        }
    }
}