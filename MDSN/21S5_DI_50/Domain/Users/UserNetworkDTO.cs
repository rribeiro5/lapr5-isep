using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class UserNetworkDTO
    {
        public int UserLevel { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<string> InterestTags { get; set; }
        public string Avatar { get; set; }
        public string EmotionalState{ get; set; }
        public List<UserNetworkConnDTO> Connections { get; set; }

        public UserNetworkDTO(int level, Guid id, string email, List<string> tags, string emotionalState, List<UserNetworkConnDTO> connections) {
            this.UserLevel = level;
            this.Id = id;
            this.Email = email;
            this.InterestTags = tags;
            this.EmotionalState = emotionalState;
            this.Connections = connections;
        }

        public UserNetworkDTO(int level, Guid id, string email, string name, List<string> tags, string avatar, string emotionalState, List<UserNetworkConnDTO> connections) {
            this.UserLevel = level;
            this.Id = id;
            this.Email = email;
            this.Name = name;
            this.InterestTags = tags;
            this.Avatar = avatar;
            this.EmotionalState = emotionalState;
            this.Connections = connections;
        }

        public override bool Equals(object obj)
        {
            return obj is UserNetworkDTO dTO &&
                   Id.Equals(dTO.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }

    public class UserNetworkConnDTO {
        public int ConnectionStrength { get; set; }
        public int RelationshipStrength { get; set; }
        public List<string> ConnTags { get; set; }
        public UserNetworkDTO User { get; set; }

        public UserNetworkConnDTO(int connectionStrength, int relationshipStrength, List<string> tags, UserNetworkDTO user)
        {
            this.ConnectionStrength = connectionStrength;
            this.RelationshipStrength = relationshipStrength;
            this.ConnTags = tags;
            this.User = user;
        }

        public override bool Equals(object obj)
        {
            return obj is UserNetworkConnDTO dTO &&
                   ConnectionStrength == dTO.ConnectionStrength &&
                   RelationshipStrength == dTO.RelationshipStrength &&
                   //EqualityComparer<List<string>>.Default.Equals(ConnTags, dTO.ConnTags) && este metodo nao esta a funcionar verificar mais tarde
                   EqualityComparer<UserNetworkDTO>.Default.Equals(User, dTO.User);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ConnectionStrength, RelationshipStrength, ConnTags, User);
        }
    }


}