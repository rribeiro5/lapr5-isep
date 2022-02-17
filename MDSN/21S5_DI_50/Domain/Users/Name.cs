using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System;
namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class Name : IValueObject{

        public string _Name {get; private set;}

        private Name(){}

        public Name (string name){
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleValidationException("Need to introduce a Name for Register");
            }

            name = name.Trim();
            if(!Regex.IsMatch(name,@"^[0-9a-zA-ZáàâãéèêíïóôõöúçñÁÀÂÃÉÈÊÍÏÓÔÕÖÚÇÑ ]+$")){
                throw new BusinessRuleValidationException($"Invalid Format for input {name}",nameof(name));
            }
            this._Name = name;
        }
    }
}