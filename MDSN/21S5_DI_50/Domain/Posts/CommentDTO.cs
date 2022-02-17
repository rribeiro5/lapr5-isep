using System.Collections.Generic;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Posts
{
    public class CommentDTO
    {
        public string id { get; set; }
        public string userId{ get; set;}
        public UserDto user { get; set; }
        public string text{ get; set;}
        public ICollection<ReactionDTO> reactions{ get; set;}

        public CommentDTO(string id, string userId, string text, ICollection<ReactionDTO> reactions)
        {
            this.id = id;
            this.userId = userId;
            this.text = text;
            this.reactions = reactions;
        }
    }
}