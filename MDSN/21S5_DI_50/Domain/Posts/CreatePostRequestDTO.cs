using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public class CreatePostRequestDTO
    {
        public Guid userId;
        public string text{ get;}
        public ICollection<string> tags{ get;}
        
        public CreatePostRequestDTO(Guid userId, string text, ICollection<string> tags)
        {
            this.userId = userId;
            this.text = text;
            this.tags = tags;
        }
    }
}