using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;
using Xunit.Abstractions;

namespace Tests.TestesUnitarios.User
{
    public class CityTest
    {

        [Fact]
        public void SuccessfullyCreateCityWithNormalName()
        {
            const string city = "cityTest";
            var c = new City(city);
            Assert.Equal(city,c._City);
        }
        
        [Fact]
        public void SuccessfullyCreateCityWithNormalNameWithDigits()
        {
            const string city = "cityTest2";
            var c = new City(city);
            Assert.Equal(city,c._City);
        }
        
        [Fact]
        public void SuccessfullyCreateCityWithSpaceAtTheEnd()
        {
            const string city = "cityTest  ", expected="cityTest";
            var c = new City(city);
            Assert.Equal(expected,c._City);
        }
        
        [Fact]
        public void SuccessfullyCreateCityWithSpaceAtTheBeginning()
        {
            const string city = "  cityTest", expected="cityTest";
            var c = new City(city);
            Assert.Equal(expected,c._City);
        }
        
        [Fact]
        public void FailToCreateCityWithEmptyName1()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new City(""));
        }
        
        [Fact]
        public void FailToCreateCityWithWhiteSpaceName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new City("   "));
        }
        
        [Fact]
        public void FailToCreateCityWithNullName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new City(null));
        }
        
        
        [Fact]
        public void FailToCreateCityWithSpecialCharactersInName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new City("FrankG#$"));
        }
    }
}