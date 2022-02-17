using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System;
using DDDSample1.Utils;

namespace DDDSample1.Domain.Users
{
    [Owned]
    public class TelephoneNumber : IValueObject{

         public string _Number {get; private set;}

        private TelephoneNumber(){} 

        public TelephoneNumber (string Number){
            if (Number.IsNullOrEmpty())
            {
                throw new BusinessRuleValidationException("Number cannot be null or empty !");
            }
            Number = Number.Trim();
            // Regex to verify if number has the code of country and is between 4 to 11 digits
            if(!Regex.IsMatch(Number,@"^\+[0-9]{3}[0-9]{4,11}$")){
                throw new BusinessRuleValidationException("Invalid Number Introduced, needs to include country code !");
            }   
            this._Number = Number;
        }



    }


}