using DDDSample1.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;

namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class LinkedInProfile : IValueObject{

        public string _ProfileUrl {get; private set;}

        private LinkedInProfile(){}

        public LinkedInProfile (string url){
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new BusinessRuleValidationException($"Input for url of LinkedIn {url} is empty",nameof(url));
            }
            var urlTrimmed = url.Trim();
            if (!Regex.IsMatch(urlTrimmed, "^https://(w{3}.)?linkedin.com/in/[0-9a-zA-Z-]{3,100}(/)?$"))
            {
                 throw new BusinessRuleValidationException($"Input for url of LinkedIn {url} is not valid",nameof(url));
            }
            this._ProfileUrl = urlTrimmed;
        }
    }
}