using System;
using System.Collections;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;

namespace DDDSample1.Domain.Posts
{
    public class Post :Entity<PostId>,IAggregateRoot
    {
        public UserId UserId;
        public PostText Text{ get;private set; }
        public ICollection<Tag> Tags{ get;private set; }
        public ICollection<Comment> Comments { get; private set; }
        public ICollection<Reaction> Reactions{ get;private set; }

        public Post(Guid userId, PostText text, ICollection<Tag> tags)
        {
            UserId = new UserId(userId);
            Text = text;
            Tags = tags;
        }
    }
}