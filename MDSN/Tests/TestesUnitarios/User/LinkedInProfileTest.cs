using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class LinkedInProfileTest
    {
        [Fact]
        public void SuccessfullyCreateLinkedInUrl()
        {
            const string url = "https://www.linkedin.com/in/id123";
            var result = new LinkedInProfile(url);
            Assert.Equal(url,result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateLinkedInUrlWithWhiteSpaceAtTheEnd()
        {
            const string url = "https://www.linkedin.com/in/id123   ", expected="https://www.linkedin.com/in/id123";
            var result = new LinkedInProfile(url);
            Assert.Equal(expected, result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateLinkedInUrlWithWhiteSpaceAtTheBeginning()
        {
            const string url = "     https://www.linkedin.com/in/id123", expected="https://www.linkedin.com/in/id123";
            var result = new LinkedInProfile(url);
            Assert.Equal(expected, result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateLinkedInUrlWithMaxNumberOfChars()
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("https://www.linkedin.com/in/");
            for (int i = 1; i <= 100; i++) { sBuilder.Append('a'); }
            var url = sBuilder.ToString();
            var result = new LinkedInProfile(url);
            Assert.Equal(url, result._ProfileUrl);
        }
        
        [Fact]
        public void FailToCreateEmptyLinkedInUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile(""));
        }
        
        [Fact]
        public void FailToCreateWhiteSpaceLinkedInUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile("   "));
        }
        
        [Fact]
        public void FailToCreateNullLinkedInUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile(null));
        }
        
        [Fact]
        public void FailToCreateLinkedInUrlWithNonLinkedInUrl1()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile("https://wikipedia.org"));
        }
        
        [Fact]
        public void FailToCreateLinkedInUrlWithNonLinkedInUrl2()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile("https://wikipedia.org/usr1"));
        }
        
        [Fact]
        public void FailToCreateLinkedInUrlWithInvalidLinkedInUrlNotEnoughChars()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile("https://linkedin.com/in/12"));
        }
        
        [Fact]
        public void FailToCreateLinkedInUrlWithInvalidLinkedInUrlNoIn()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile("https://LinkedIn.com/user234234"));
        }
        
        [Fact]
        public void FailToCreateLinkedInUrlWithInvalidLinkedInUrlTooManyChars()
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("https://www.linkedin.com/in/");
            for (int i = 1; i <= 101; i++)
            {
                sBuilder.Append('a');
            }
            Assert.Throws<BusinessRuleValidationException>(() => new LinkedInProfile(sBuilder.ToString()));
        }
    }
}