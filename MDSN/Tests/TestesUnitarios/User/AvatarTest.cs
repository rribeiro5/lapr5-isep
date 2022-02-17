using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class AvatarTest
    {
        [Fact]
        public void successfullyCreateAvatar()
        {
            var url = "some url";
            var result = new Avatar(url);
            Assert.Equal(url,result._avatarUrl);
        }

        [Fact]
        public void FailToCreateAvatarWithNullUrl()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new Avatar(null));
        }
        
        [Fact]
        public void FailToCreateAvatarWithEmptyUrl1()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new Avatar(""));
        }
        
        [Fact]
        public void FailToCreateAvatarWithEmptyUrl2()
        {
            Assert.Throws<BusinessRuleValidationException>(()=>new Avatar("        "));
        }
    }
}