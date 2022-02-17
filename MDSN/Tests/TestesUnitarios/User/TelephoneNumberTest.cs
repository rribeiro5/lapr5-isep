using System;
using System.Linq;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class TelephoneNumberTest
    {

        [Fact]
        public void SuccessfullyCreateTelephoneNumberWithLength11()
        {
            var length = 11;
            var number = generateRandomString(length);
            TelephoneNumber telephoneNumber = new TelephoneNumber(number);
            Assert.True(telephoneNumber._Number.Equals(number));
        }
        
        [Fact]
        public void SuccessfullyCreateTelephoneNumberWithLength4()
        {
            var length = 4;
            var number = generateRandomString(length);
            TelephoneNumber telephoneNumber = new TelephoneNumber(number);
            Assert.True(telephoneNumber._Number.Equals(number));
        }
        
        [Fact]
        public void FailToCreateNullTelephoneNumber()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new TelephoneNumber(null));
        }
        
        [Fact]
        public void FailToCreateWhiteSpacesTelephoneNumber()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new TelephoneNumber("    "));
        }
        
        [Fact]
        public void FailToCreateEmptyTelephoneNumber()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new TelephoneNumber(""));
        }
        
        [Fact]
        public void FailToCreateTelephoneNumberWithMoreThan11length()
        {
            var length = 12;
            var number = generateRandomString(length);
            Assert.Throws<BusinessRuleValidationException>(() => new TelephoneNumber(number));
        }

        [Fact]
        public void FailToCreateTelephoneNumberWithLessThan4length()
        {
            var length = 3;
            var number = generateRandomString(length);
            Assert.Throws<BusinessRuleValidationException>(() => new TelephoneNumber(number));
        }
        
        private string generateRandomString(int length)
        {
            var countryCodeLength = 3;
            const string chars = "0123456789";
            var random = new Random();
            var number = new string(Enumerable.Repeat(chars, length+countryCodeLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            number = "+" + number;
            return number;
        }
    }
}