
using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users{
    [Owned]

    public class ProfileDescription : IValueObject {

        public string _Description {get; private set;}

        private ProfileDescription(){}

        public ProfileDescription(string Description){
            
            if(Description == null)
                throw new BusinessRuleValidationException("Introduce a non null description");

            if(Description.Length >5000)
                throw new BusinessRuleValidationException("Description has a max 5000 characters length|");

            this._Description = Description;
        }



    }


}