using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class OCCEmotionsTest
    {
        [Fact]
        public void SuccessfullyCreateOccEmotion()
        {
            const string emotion = "Grateful";
            var result = new OCCEmotion(emotion);
            Assert.Equal(emotion,result._Emotion);
        }
        
        [Fact]
        public void SuccessfullyCreateOccEmotionWithSTrailingSpace()
        {
            const string emotion = "Grateful   ",expected="Grateful";
            var result = new OCCEmotion(emotion);
            Assert.Equal(expected,result._Emotion);
        }
        
        [Fact]
        public void SuccessfullyCreateOccEmotionWithLeadingSpace()
        {
            const string emotion = "     Grateful",expected="Grateful";
            OCCEmotion result = new OCCEmotion(emotion);
            Assert.Equal(expected,result._Emotion);
        }
        
        [Fact]
        public void SuccessfullyCreateOccEmotionAllLowerCaps()
        {
            const string emotion = "grateful",expected="Grateful";
            var result = new OCCEmotion(emotion);
            Assert.Equal(expected,result._Emotion);
        }
        
        [Fact]
        public void SuccessfullyCreateOccEmotionAllUpperCaps()
        {
            const string emotion = "JOYFUL", expected = "Joyful";
            var result = new OCCEmotion(emotion);
            Assert.Equal(expected,result._Emotion);
        }
        
        [Fact]
        public void FailToCreateOccEmotionWithEmptyInput()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new OCCEmotion(""));
        }
        
        [Fact]
        public void FailToCreateOccEmotionWithEmptyWhiteSpaceInput()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new OCCEmotion("    "));
        }
        
        [Fact]
        public void FailToCreateOccEmotionWithEmptyWhiteNullInput()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new OCCEmotion(null));
        }
        
        [Fact]
        public void FailToCreateOccEmotionWithInvalidInput()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new OCCEmotion("Indifferent"));
        }
    }
}