using System.Text.RegularExpressions;
using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using DDDSample1.Utils;
namespace DDDSample1.Domain.Users{
    [Owned]
        public class Password : IValueObject {

        public string _Password {get;private set;}

        private Password(){}

        public Password(string Password){
            if (String.IsNullOrWhiteSpace(Password))
            {
                throw new BusinessRuleValidationException("Need a valid password to register User");
            }
            if(!Regex.IsMatch(Password,@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[A-Za-z\d@$!%*?& ]{8,}$")){
                throw new BusinessRuleValidationException("Password needs to be at least 8 characters long, include at least one uppercase, one lowercase and one number");
            }
            // save password with hash + salt
            this._Password = SecurePasswordHasher.Hash(Password);
        }

    

    }


}