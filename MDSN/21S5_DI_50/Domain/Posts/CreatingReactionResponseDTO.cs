using System;
namespace DDDSample1.Domain.Posts
{
    public class CreatingReactionResponseDTO
    {
        public string publicationId { get; set; }

        public string publicationUserId { get; set; }
        public string userId{ get; set;}

        public bool incrementRelation { get; set; }
        public string reaction{ get; set;}
        
        public CreatingReactionResponseDTO(string publicationId, string publicationUserId, string userId, bool incrementRelation ,string reaction)
        {   
            this.publicationId = publicationId;
            this.publicationUserId = publicationUserId;
            this.userId = userId;
            this.incrementRelation = incrementRelation;
            this.reaction = reaction;
        }

         public override  bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else {
                CreatingReactionResponseDTO p = (CreatingReactionResponseDTO) obj;
                return  (publicationUserId == p.publicationUserId && reaction == p.reaction && publicationId == p.publicationId && userId == p.userId && incrementRelation==p.incrementRelation);
            }
        }
        
    }
}