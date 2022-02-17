using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DDDSample1.Domain.Shared;
using Xunit;
using Xunit.Abstractions;


namespace Tests.TestesUnitarios.User
{
    public class UserTest 
    {
        private readonly ITestOutputHelper _testOutputHelper;

        //Mandatory fields
        private string _name;
        private string  _birthDayDate;
        private string  _password;
        private string  _email;
        private List<string> _interestTags;
        //Optional Fields
        private string _city;
        private string _country;
        private string _description;
        private string _number;
        private string _linkedInProfile;
        private string _facebookProfile;

        public UserTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _name = "name1";
            _birthDayDate = "2000-01-01";
            _password ="Password1?";
            _email = "1234@gmail.com";
            _interestTags = new List<string> {"tag1"};
        }

        [Fact]
        public void SuccessfullyCreateUserWithMinimumRequired()
        {
            var user = CreateUser();
            Assert.True(user!=null);
        }

        [Fact]
        public void SuccessfullyCreateUserWithAllFieldsFilled()
        {
            _city = "city1";
            _country = "Portugal";
            _description = "Test Description";
            _number = "+351123456789";
            _linkedInProfile = "https://linkedin.com/in/profile-1";
            _facebookProfile = "https://www.facebook.com/testProfile";
            var user = CreateUser();
            Assert.True(user!=null);
        }

        [Fact]
        public void FailToCreateUserWithoutName()
        {
            _name = null;
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }
        
        [Fact]
        public void FailToCreateUserWithoutBirthdayDate()
        {
            _birthDayDate = null;
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }
        
        [Fact]
        public void FailToCreateUserWithoutPassword()
        {
            _password = null;
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }
        
        [Fact]
        public void FailToCreateUserWithoutEmail()
        {
            _email = null;
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }
        
        [Fact]
        public void FailToCreateUserWitInterestTagsAsNull()
        {
            _interestTags = null;
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }
        [Fact]
        public void FailToCreateUserWitInterestTagsAsEmptyList()
        {
            _interestTags = new List<string>();
            Assert.Throws<BusinessRuleValidationException>(CreateUser);
        }

        private DDDSample1.Domain.Users.User CreateUser()
        {
            return new DDDSample1.Domain.Users.User(_name, _birthDayDate, _city, _country, _email, _password, 
                _description, _number, _linkedInProfile, _facebookProfile, _interestTags);
        }
    }
}