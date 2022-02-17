using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class FacebookProfileTest
    {

        [Fact]
        public void SuccessfullyCreateFacebookUrl()
        {
            const string url = "https://www.facebook.com/id123";
            var result = new FacebookProfile(url);
            Assert.Equal(url,result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateFacebookUrlWithDotsInTheProfileLink()
        {
            const string url = "https://www.facebook.com/id123.fb.tree";
            const string expected="https://www.facebook.com/id123fbtree";
            var result = new FacebookProfile(url);
            Assert.Equal(expected,result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateFacebookUrlWithWhiteSpaceAtTheEnd()
        {
            const string url = "https://www.facebook.com/id123   ", expected="https://www.facebook.com/id123";
            var result = new FacebookProfile(url);
            Assert.Equal(expected,result._ProfileUrl);
        }
        
        [Fact]
        public void SuccessfullyCreateFacebookUrlWithWhiteSpaceAtTheBeginning()
        {
            const string url = "     https://www.facebook.com/id123", expected="https://www.facebook.com/id123";
            var result = new FacebookProfile(url);
            Assert.Equal(expected,result._ProfileUrl);
        }
        
        [Fact]
        public void FailToCreateEmptyFacebookUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile(""));
        }
        
        [Fact]
        public void FailToCreateWhiteSpaceFacebookUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("   "));
        }
        
        [Fact]
        public void FailToCreateNullFacebookUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile(null));
        }
        
        [Fact]
        public void FailToCreateFacebookUrlWithNonFacebookUrl1()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("https://wikipedia.org"));
        }
        
        [Fact]
        public void FailToCreateFacebookUrlWithNonFacebookUrl2()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("https://wikipedia.org/usr1"));
        }
        
        [Fact]
        public void FailToCreateFacebookUrlWithInvalidFacebookUrl1()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("https://facebook.com/usr.1"));
        }
        
        [Fact]
        public void FailToCreateFacebookUrlWithInvalidFacebookUrl2()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("https://facebook.com/$gtret"));
        }
        
        [Fact]
        public void FailToCreateFacebookUrlWithInvalidFacebookUrl3()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new FacebookProfile("https://face.book.com/user234567"));
        }
    }
}