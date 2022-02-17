using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public class CreatingCommentDTO
    {
        public string postId{ get; set;}
        public string userId{ get; set;}
        public string text{ get; set;}
        public ICollection<ReactionDTO> reactions{ get; set;}

        public CreatingCommentDTO(string postId, string userId, string text)
        {
            this.postId = postId;
            this.userId = userId;
            this.text = text;
            this.reactions = new List<ReactionDTO>();
        }
    }
}