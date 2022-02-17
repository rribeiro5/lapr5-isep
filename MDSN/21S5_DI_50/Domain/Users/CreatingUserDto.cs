using System.Collections.Generic;
namespace DDDSample1.Domain.Users
{   

    public class CreatingUserDto
    {
        public string Name {get;set;}
        public string BirthDayDate {get;set;}
        public string City {get;set;}
        public string Country {get;set;}
        public string Email {get;set;}

        public string Avatar {get;set;}

        public string Password {get;set;}
        public string ProfileDescription{get;set;}

        public string TelephoneNumber{get;set;}

        public List<string> InterestTags {get;set;} = new List<string>();
        public string LinkFacebook {get; set;}
        public string LinkLinkedin {get; set;}
        
        
        public CreatingUserDto (string name , string birthDayDate,string avatar,string city, string country,string email, string password,string description,string telenumber,List<string> interessTags, string linkFacebook ,  string linkLinkedin ){
            this.Name = name;
            this.BirthDayDate = birthDayDate;
            this.Avatar = avatar;
            this.City = city;
            this.Country = country;
            this.Email = email;
            this.Password = password;
            this.ProfileDescription = description;
            this.TelephoneNumber = telenumber;
            this.InterestTags = interessTags;
            this.LinkFacebook = linkFacebook;
            this.LinkLinkedin = linkLinkedin;
        }

    }
    
}