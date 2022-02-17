using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System;
namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class City : IValueObject{

        public string _City {get; private set;}

        private City(){}

        public City (string newCity){
            // information with alpha without any additional validation
            if(String.IsNullOrEmpty(newCity))
            {
                throw new BusinessRuleValidationException($"Empty input for {newCity}",nameof(newCity));
            }
            var city = newCity.Trim();
            if(!Regex.IsMatch(city,@"^[0-9a-zA-Z]+$")){
                throw new BusinessRuleValidationException($"Invalid Name for input {newCity}",nameof(newCity));
            }
            this._City = city;
        }
    }


}