using System.Collections;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Posts
{
    public class Comment : Entity<CommentId>
    {
        public UserId UserId;
        public CommentText comment { get; private set; }
        public ICollection<Reaction> reactions { get; private set;}

        public Comment(UserId userId, CommentText comment, ICollection<Reaction> reactions)
        {
            UserId = userId;
            this.comment = comment;
            this.reactions = reactions;
        }
    }
}