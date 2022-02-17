namespace DDDSample1.Domain.Posts
{
    public class ReactionDTO
    {
        public string userId{ get; set;}
        public string reaction{ get; set;}
        
        public ReactionDTO(string userId ,string reaction)
        {
            this.userId = userId;
            this.reaction = reaction;
        }
    }
}