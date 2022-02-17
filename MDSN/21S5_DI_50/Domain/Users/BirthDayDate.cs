using DDDSample1.Domain.Shared;
using System;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users
{      
    [Owned]
    public class BirthDayDate : IValueObject{

        public string _BirthDayDate {get; private set;}
        public static readonly string Format = "yyyy/MM/dd";
        
        public BirthDayDate (string birthDayDate){
           if(string.IsNullOrEmpty(birthDayDate)) 
               throw new BusinessRuleValidationException("Introduce a valid Birth Day Date.");
           var today = DateTime.Today;
           try
           {
               var birthDate = DateTime.Parse(birthDayDate.Trim());
               var yearDiff = today.Year - birthDate.Year;
               // verify if the User is at least 16 years old
               if ((yearDiff < 16)
                   || (yearDiff==16 && birthDate.Month>today.Month)
                   ||(yearDiff==16 && birthDate.Month==today.Month && birthDate.Day>today.Day))
               {
                   throw new BusinessRuleValidationException($"User with input {birthDayDate} needs to be at least 16 years of age.",nameof(birthDayDate));
               }

               this._BirthDayDate = birthDate.ToString(Format);
           }catch (FormatException)
           {
               throw new BusinessRuleValidationException($"Incorrect format for input {birthDayDate} Date.", nameof(birthDayDate));
           }
        }

 


    }


}