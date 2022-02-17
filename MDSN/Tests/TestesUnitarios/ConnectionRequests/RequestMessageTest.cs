using System;
using System.Linq;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Tests.TestesUnitarios.ConnectionRequests
{
    public class RequestMessageTest
    {
        //todo: review
        [Fact]
        public void SuccessfullyCreateEmptyMessage()
        {
            string message = "";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        //todo: review
        [Fact]
        public void SuccessfullyCreateSpaceMessage()
        {
            string message = " ";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void SuccessfullyCreateSingleWordMessage()
        {
            string message = "Hi";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void SuccessfullyCreateMultipleWordsMessage()
        {
            string message = "Hi X I would like us to be friends";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void SuccessfullyCreateSpecialCharactersMessage()
        {
            string message = "Hi, X! I'm Y_/Y. I would like us to be friends.";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void SuccessfullyCreateAlphanumericMessage()
        {
            string message = "Hi, X! I'm Y123. I would like us to be friends.";
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void SuccessfullyCreateMessageWithLength10000()
        {
            var length = 10000;
            var message = generateRandomString(length);
            RequestMessage m = new RequestMessage(message);
            Assert.Equal(message,m.Message);
        }
        
        [Fact]
        public void FailToCreateNullRequestMessage()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new RequestMessage(null));
        }
        
        [Fact]
        public void FailToCreateRequestMessageWithMoreThan10000length()
        {
            var length = 10001;
            var message = generateRandomString(length);
            Assert.Throws<BusinessRuleValidationException>(() => new RequestMessage(message));
        }


        private string generateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
            var random = new Random();
            var message = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return message;
        }
    }
}