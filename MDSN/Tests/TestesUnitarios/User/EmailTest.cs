using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class EmailTest
    {
        [Fact]
        public void SuccessfullyCreateEmail()
        {
            const string email = "1234@gmail.com";
            var result = new Email(email);
            Assert.Equal(email,result._Email);
        }
        
        [Fact]
        public void SuccessfullyCreateEmailWithWhiteSpaceAtTheEnd()
        {
            const string email = "1234@gmail.com  ", expected="1234@gmail.com";
            var result = new Email(email);
            Assert.Equal(expected,result._Email);
        }
        
        [Fact]
        public void SuccessfullyCreateEmailWithWhiteSpaceAtTheBeginning()
        {
            const string email = "    1234@gmail.com", expected="1234@gmail.com";
            var result = new Email(email);
            Assert.Equal(expected,result._Email);
        }
        
        [Fact]
        public void FailCreateEmailWithEmptyString()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Email("    "));
        }
        
        [Fact]
        public void FailCreateEmailWithNullString()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Email(null));
        }
        
        [Fact]
        public void FailCreateEmailWithEmailWithoutAtSymbol()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Email("1234gmail"));
        }
    }
}