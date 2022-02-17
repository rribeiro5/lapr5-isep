
using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
namespace DDDSample1.Domain.Users{
    [Owned]
    public class Avatar : IValueObject {

        public string _avatarUrl {get;private set;}

        private Avatar(){}

        public Avatar(string url){
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new BusinessRuleValidationException("Need to introduce a Url for Avatar");
            }
            this._avatarUrl = url.Trim();

        }


    }


}