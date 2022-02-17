using System;
using System.Globalization;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;
using Xunit.Abstractions;

namespace Tests.TestesUnitarios.User
{
    public class BirthdayDateTest
    {
        private string format = BirthDayDate.Format;
        
        [Fact]
        public void SuccessfullyCreateBirthdayDateWithDashSeparator()
        {
            const string date = "2000-1-1";
            const string expected = "2000/01/01";
            var result = new BirthDayDate(date);
            Assert.Equal(expected,result._BirthDayDate);
        }
        
        [Fact]
        public void SuccessfullyCreateBirthdayWithSlashSeparator()
        {
            const string date = "2000/1/1";
            const string expected = "2000/01/01";
            var result = new BirthDayDate(date);
            Assert.Equal(expected,result._BirthDayDate);
        }
        
        [Fact]
        public void SuccessfullyCreateBirthdayDateWithTrailingSpace()
        {
            const string date = "2000-1-1    ", expected="2000/01/01";
            var result = new BirthDayDate(date);
            Assert.Equal(expected,result._BirthDayDate);
        }
        
        [Fact]
        public void SuccessfullyCreateBirthdayDateWithLeadingSpace()
        {
            const string date = "   2000-1-1", expected="2000/01/01";
            var result = new BirthDayDate(date);
            Assert.Equal(expected,result._BirthDayDate);
        }
        
        [Fact]
        public void SuccessfullyCreateBirthdayWithExactly16YearsAnd0Days()
        { 
            var date= DateTime.Now.AddYears(-16).ToString(format);
            var result = new BirthDayDate(date);
            Assert.Equal(date,result._BirthDayDate);
            //Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate(date));
        }
        
        [Fact]
        public void FailToCreateBirthdayUnder16YearsOld()
        {
            var date= DateTime.Now.AddYears(-16).AddDays(1).ToString(format);
            Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate(date));
        }
        
        [Fact]
        public void FailToCreateDateWithNonYYYY_MM_DD_Format()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate("1-1-2020"));
        }
        
        [Fact]
        public void FailToCreateDateWithEmptyString()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate(""));
        }
                
        [Fact]
        public void FailToCreateDateWithWhiteSpaceString()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate("   "));
        }
        
        [Fact]
        public void FailToCreateDateWithNullString()
        {
            Assert.Throws<BusinessRuleValidationException>(() => new BirthDayDate(null));
        }
    }
}