using DDDSample1.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class FacebookProfile : IValueObject{

        public string _ProfileUrl {get; private set;}

        private FacebookProfile(){}

        public FacebookProfile (string url){
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new BusinessRuleValidationException($"Input for url of Facebook {url} is empty",nameof(url));
            }
            var urlTrimmed = url.Trim();
            var profilePagePathIndex=urlTrimmed.LastIndexOf('/')+1;
            if (profilePagePathIndex == -1 || profilePagePathIndex>urlTrimmed.Length)
            {
                throw new BusinessRuleValidationException($"Input for url of Facebook {url} is invalid",nameof(url));
            }
            if (!Regex.IsMatch(urlTrimmed, "^https://(w{3}.)?facebook.com/[0-9a-zA-Z.]{5,}(/)?$"))
            {
                throw new BusinessRuleValidationException($"Input for url of Facebook {url} is invalid",nameof(url));
            }
            //remove dots from profile link after "facebook.com/" since facebook doesn't count the dot as a character in the profile name
            //example: "facebook.com/user.12.3" counts as "facebook.com/user123"
            if (urlTrimmed[^1] == '/')
            {
                urlTrimmed = urlTrimmed[..(urlTrimmed.Length-1)];
            }
            var profilePath = urlTrimmed[(urlTrimmed.LastIndexOf('/') + 1)..];
            bool rewriteUrl = false;
            if (profilePath.Contains("."))
            {
                rewriteUrl = true;
                profilePath = profilePath.Replace(".",string.Empty);
            }
            if (profilePath.Length < 5)
            {
                throw new BusinessRuleValidationException($"Input for url of Facebook {url} is invalid",nameof(url));
            }
            if (rewriteUrl)
            {
                this._ProfileUrl = "https://www.facebook.com/" + profilePath;
            }
            else
            {
                this._ProfileUrl = urlTrimmed;
            }
        }
    }
}