using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class UserPrivateProfileDto
    {
        public Guid Id {get;}
        
        public string Avatar { get; }
        public string Name { get; set; }
        public string Email {get;}
        public string PhoneNumber { get; set; }
        public string BirthdayDate {get;set;}
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public int Points {get;}
        public string LinkedInURL { get; set; }
        public string FacebookURL { get; set; }
        public IEnumerable<string> InterestTags {get; set; }
        public string EmotionalState{get;}
        
        public UserPrivateProfileDto(Guid id, string avatar,string name, string email, string phoneNumber, string birthdayDate, 
            string city, string country, string description, int points, string linkedInUrl, 
            string facebookUrl, IEnumerable<string> interestTags, string emotionalState)
        {
            Id = id;
            Avatar = avatar;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            BirthdayDate = birthdayDate;
            City = city;
            Country = country;
            Description = description;
            Points = points;
            LinkedInURL = linkedInUrl;
            FacebookURL = facebookUrl;
            InterestTags = interestTags;
            EmotionalState = emotionalState;
        }
    }
}