using System;
using System.Collections;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class UserSuggestedDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        
        public string email { get; set; }
        public string avatar { get; set; }
        public string birthdayDate { get; set; }
        public string city { get; set; }
        public string country { get;set; }
        public string profileDescription { get; set; }
        public string linkedInURL { get;set; }
        public string facebookURL { get; set; }
        public IEnumerable<string> interestTagsDtoList {get; set; }
/*
        public UserSuggestedDto(Guid id, string avatar, string birthdayDate, string city, string country, 
            string profileDescription, string linkedInUrl, string facebookUrl, IEnumerable<string> interestTagsDtoList)
        {
            this.id = id;
            //this.avatar = avatar;
            this.birthdayDate = birthdayDate;
            this.city = city;
            this.country = country;
            this.profileDescription = profileDescription;
            this.linkedInURL = linkedInUrl;
            this.facebookURL = facebookUrl;
            this.interestTagsDtoList = interestTagsDtoList;
        }
*/
        public UserSuggestedDto() {}
    }
}