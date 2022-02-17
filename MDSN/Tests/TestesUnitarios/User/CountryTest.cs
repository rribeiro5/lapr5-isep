using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class CountryTest
    {
        [Fact]
        public void SuccessfullyCreateCountry()
        {
            const string country = "United Kingdom";
            var c = new Country(country);
            Assert.Equal(country,c._Country);
        }
        
        [Fact]
        public void SuccessfullyCreateCountryWithSpaceAtTheEnd()
        {
            const string country = "Portugal  ", expected="Portugal";
            var c = new Country(country);
            Assert.Equal(expected,c._Country);
        }
        
        [Fact]
        public void SuccessfullyCreateCountryWithSpaceAtTheBeginning()
        {
            const string country = "   Portugal", expected="Portugal";
            var c = new Country(country);
            Assert.Equal(expected,c._Country);
        }
        
        [Fact]
        public void FailToCreateCountryWithEmptyName1()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country(""));
        }
        
        [Fact]
        public void FailToCreateCountryWithWhiteSpaceName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country("   "));
        }
        
        [Fact]
        public void FailToCreateCountryWithNullName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country(null));
        }
        
        /*[Fact]
        public void FailToCreateCountryWithDigitsInName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country("123"));
        }
        
        [Fact]
        public void FailToCreateCountryWithSpecialCharactersInName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country("FrankG#$"));
        }
        
        [Fact]
        public void FailToCreateCountryWithInvalidNameForCountry()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Country("Country"));
        }*/
    }
}