using System;
using System.Linq;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.Generics
{
    public class TagTest
    {
        
        [Fact]
        public void SuccessfullyCreateSingleWordTag()
        {
            string description = "Test";
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void SuccessfullyCreateMultipleWordsTag()
        {
            string description = "TestMe";
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void SuccessfullyCreateSpecialCharactersTag()
        {
            string description = "Tag,.-!+/,{}[]%&~\\#@$^*()";
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void SuccessfullyCreateAlphanumericTag()
        {
            string description = "01234tAg56789";
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void SuccessfullyCreateMessageWithLength1()
        {
            var length = 1;
            var description = generateRandomString(length);
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void SuccessfullyCreateMessageWithLength255()
        {
            var length = 255;
            var description = generateRandomString(length);
            Tag m = new Tag(description);
            Assert.Equal(description,m.Description);
        }
        
        [Fact]
        public void FailToCreateNullTag()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new Tag(null));
        }
        
        [Fact]
        public void FailToCreateEmptyTag()
        {
            string description = "";
            Assert.Throws<BusinessRuleValidationException>(() => new Tag(description));
        }
        
        
        [Fact]
        public void FailToCreateTagWithMoreThan255length()
        {
            var length = 256;
            var description = generateRandomString(length);
            Assert.Throws<BusinessRuleValidationException>(() => new Tag(description));
        }
        
        [Fact]
        public void FailToCreateOnlyWhiteSpaceTag()
        {
            string description = " ";
            Assert.Throws<BusinessRuleValidationException>(() => new Tag(description));
        }
        
        [Fact]
        public void FailToCreateWordsAndWhiteSpaceTag()
        {
            string description = "Hi there";
            Assert.Throws<BusinessRuleValidationException>(() => new Tag(description));
        }


        private string generateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            var random = new Random();
            var message = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return message;
        }
    }
}