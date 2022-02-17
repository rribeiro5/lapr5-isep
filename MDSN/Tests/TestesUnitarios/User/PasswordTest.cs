using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Utils;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class PasswordTest
    {
        [Fact]
        public void SuccessfullyCreatePassword()
        {
            var result = new Password("Password1?");
            Assert.True(!result._Password.IsNullOrEmpty());
        }
        
        [Fact]
        public void SuccessfullyCreatePasswordWithWhiteSpaceAsSpecialCharacter()
        {
            var result = new Password("Pass word 1");
            Assert.True(!result._Password.IsNullOrEmpty());
        }
        
        [Fact]
        public void FailToCreatePasswordWithoutMinimumNumberOfChars()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password("!2345Ss"));
        }
        
        [Fact]
        public void FailToCreateEmptyPassword()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password(""));
        }
        
        [Fact]
        public void FailToCreateWhiteSpaceOnlyPassword()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password("     "));
        }
        
        [Fact]
        public void FailToCreateNullPassword()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password(null));
        }
        
        [Fact]
        public void FailToCreatePasswordWithoutAnUpperCaseLetter()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password("password1?"));
        }
        
        [Fact]
        public void FailToCreatePasswordWithoutALowerCaseLetter()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password("PASSWORD1?"));
        }
        
        /*
        [Fact]
        public void FailToCreatePasswordWithoutSpecialCharacters()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Password("Password1"));
        }*/
    }
}