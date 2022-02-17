
#nullable enable
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;
using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class Tag : IValueObject
    {
        public string Description { get;  private set; }

        private Tag(){}

        public Tag(string description)
        {

            if(description == null)
                throw new BusinessRuleValidationException("Introduce a non null Tag description");

            if(description.Contains(" "))
                throw new BusinessRuleValidationException("Tag description shouldn't contain whitespaces");
            
            if(description.Length<1 || description.Length >255)
                throw new BusinessRuleValidationException("Tag Description needs to be between 1 and 255 characters");
            
            this.Description = description;
        }


        public override bool Equals(object? obj)
        {
            if (obj == this)
                return true;
            if (obj==null || obj.GetType() != typeof(Tag))
                return false;
            var oTag = (Tag) obj;
            return this.Description.Equals(oTag.Description);
        }

        public override int GetHashCode()
        {
            return (int) this.Description?.GetHashCode();
        }
    }
}