using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System;
namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class Points : IValueObject{

         public int _Points {get; private set;}

        private Points(){} 

        public Points (int points){
            // information with alphanumeric without any additional validation
            if(points < 0 ){
                throw new BusinessRuleValidationException("Invalid value for points");
            }

            this._Points= points;
        }

    }


}