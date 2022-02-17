using System;
using System.Collections.Generic;
namespace DDDSample1.Domain.Users
{
    public class UserDto{
        public Guid Id {get; set;}
        public string BirthDayDate {get;set;}
        public string Email {get;set;}

        public string Name {get;set;}

        public string Avatar {get;set;}
 
        public IEnumerable<string> InterestTags {get;set;}

        public string EmotionalState{get;set;}

        public double EmotionalValue {get;set;}

        public UserDto(Guid id , string birthdayDate , string email ,IEnumerable<string> interestTagsDtoList ){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.InterestTags = interestTagsDtoList;
        }

        public UserDto(Guid id , string birthdayDate , string email , string emotionalState ,IEnumerable<string> interestTagsDtoList ){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.InterestTags = interestTagsDtoList;
            this.EmotionalState = emotionalState;
        }

         public UserDto(Guid id , string birthdayDate , string email ,string name,string emotionalState, IEnumerable<string> interestTagsDtoList){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.Name = name;
            this.InterestTags = interestTagsDtoList;
            this.EmotionalState = emotionalState;
        }

        public UserDto(Guid id , string birthdayDate , string email ,string name,string emotionalState, IEnumerable<string> interestTagsDtoList,double emotionalValue){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.Name = name;
            this.InterestTags = interestTagsDtoList;
            this.EmotionalState = emotionalState;
            this.EmotionalValue = emotionalValue;
        }

              public UserDto(Guid id , string birthdayDate , string email ,string name,string avatar,string emotionalState, IEnumerable<string> interestTagsDtoList,double emotionalValue){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.Name = name;
            this.InterestTags = interestTagsDtoList;
            this.EmotionalState = emotionalState;
            this.Avatar= avatar;
            this.EmotionalValue = emotionalValue;
        }

        
         public UserDto(Guid id , string birthdayDate , string email ,string name,string avatar,string emotionalState, IEnumerable<string> interestTagsDtoList){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.Name = name;
            this.InterestTags = interestTagsDtoList;
            this.EmotionalState = emotionalState;
            this.Avatar= avatar;
        }

        public UserDto(Guid id , string birthdayDate , string email ,string name ){
            this.Id = id ;
            this.BirthDayDate = birthdayDate;
            this.Email = email;
            this.Name = name;
        }
        
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            UserDto o = (UserDto)obj;
            return this.BirthDayDate == o.BirthDayDate && this.Email == o.Email;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.Email.GetHashCode();
        }


    }
}