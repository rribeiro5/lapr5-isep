using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class UserSearchedDTO
    {
        public Guid Id { get; set; }
        
        public string Name {get; set;}

        public string Avatar {get;set;}
        
        public string Country {get; set;}

        
        public string Email {get; set;}
        
        public IEnumerable<string> InterestTagsDto {get; set;}

        public UserSearchedDTO(Guid id, string name, string country, string email, IEnumerable<string> interestTagsDto)
        {
            Id = id;
            Name = name;
            Country = country;
            Email = email;
            InterestTagsDto = interestTagsDto;
        }

        public UserSearchedDTO(Guid id, string name, string avatar, string country, string email, IEnumerable<string> interestTagsDto)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            Country = country;
            Email = email;
            InterestTagsDto = interestTagsDto;
        }

        public UserSearchedDTO() { }
    }
}