using System;

namespace DDDSample1.Domain.Users
{
    public class UserLeaderboardDTO
    {
        public Guid Id {get; set;}
        public string Email {get;set;}
        public string Name {get;set;}
        public string Avatar {get;set;}
        public int value { get; set; }

        public UserLeaderboardDTO(Guid id, string email, string name, string avatar, int value)
        {
            Id = id;
            Email = email;
            Name = name;
            Avatar = avatar;
            this.value = value;
        }
    }
}