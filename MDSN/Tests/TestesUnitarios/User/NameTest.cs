using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class NameTest
    {
        [Fact]
        public void SuccessfullyCreateName()
        {
            const string name = "name1";
            var result = new Name(name);
            Assert.Equal(name,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithTrailingSpace()
        {
            const string name = "name1     ", expected="name1";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithLeadingSpace()
        {
            const string name = "    name1", expected="name1";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithAccents1()
        {
            const string name = "João", expected="João";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithAccents2()
        {
            const string name = "José", expected="José";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithAccents3()
        {
            const string name = "Mário", expected="Mário";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void SuccessfullyCreateNameWithAccents4()
        {
            const string name = "Ánanas", expected="Ánanas";
            var result = new Name(name);
            Assert.Equal(expected,result._Name);
        }
        
        [Fact]
        public void FailToCreateEmptyName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Name(""));
        }
        
        [Fact]
        public void FailToCreateWhiteSpaceOnlyName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Name("    "));
        }
        
        [Fact]
        public void FailToCreateNullName()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Name(null));
        }
        
        [Fact]
        public void FailToCreateNameWithSpecialChars()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Name("name1!"));
        }
        
        /*
        [Fact]
        public void FailToCreateNameWithCharsAndWhiteSpace()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Name("name 1"));
        }*/
    }
}