using DDDSample1.Domain.Shared;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System;
namespace DDDSample1.Domain.Users
{   
    [Owned]
    public class EmotionalValue : IValueObject{

        private static readonly double DEFAULT_EMOTION_VALUE = 0.5;
        public double _EmotionalValue {get; private set;}

        public EmotionalValue(){
            this._EmotionalValue = DEFAULT_EMOTION_VALUE;
        }

        public EmotionalValue (double newValue){
            if(newValue>1 || newValue<0)
                 throw new BusinessRuleValidationException("Invalid value for Emotional Value");
           
            this._EmotionalValue = newValue;
        }
    }
}