using System;
using System.Linq;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class ProfileDescriptionTest
    {
        [Fact]
        public void SuccessfullyCreateEmptyProfileDescription()
        {
            string description = "";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateWhiteSpaceProfileDescription()
        {
            string description = " ";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateSingleWordProfileDescription()
        {
            string description = "Hi";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateMultipleWordsProfileDescription()
        {
            string description = "Hi I am from Y";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateSpecialCharactersProfileDescription()
        {
            string description = "Hi all :)! I'm Y_/Y, my email is y@email.com, please contact me! #yyyy";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateAlphanumericProfileDescription()
        {
            string description = "Hi all :)! I'm Y_/Y and I'm 23 years old.\n My email is 123@email.com, please contact me! #yyyy";
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void SuccessfullyCreateProfileDescriptionWithLength5000()
        {
            var length = 5000;
            var description = GenerateRandomString(length);
            ProfileDescription profileDescription = new ProfileDescription(description);
            Assert.True(profileDescription._Description.Equals(description));
        }
        
        [Fact]
        public void FailToCreateNullProfileDescription()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new ProfileDescription(null));
        }
        
        [Fact]
        public void FailToCreateProfileDescriptionWithMoreThan5000Length()
        {
            var length = 5001;
            var description = GenerateRandomString(length);
            Assert.Throws<BusinessRuleValidationException>(() => new ProfileDescription(description));
        }


        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
            var random = new Random();
            var message = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return message;
        }
    }
}