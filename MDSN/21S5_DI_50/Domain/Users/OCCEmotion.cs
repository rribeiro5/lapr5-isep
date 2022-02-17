

using DDDSample1.Domain.Shared;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DDDSample1.Domain.Users
{      

    enum OCC {
        Joyful,
        Distressed,
        Hopeful,
        Fearful,
        Relieve,
        Disappointed,
        Proud,
        Remorseful,
        Grateful,
        Angry
    }

    [Owned]
    public class OCCEmotion : IValueObject{
        private static readonly string DEFAULT_EMOTION = "Joyful";
        public string _Emotion {get; private set;}

        public OCCEmotion(){
            this._Emotion = DEFAULT_EMOTION;
        }

        public OCCEmotion (string emotion){

            if (String.IsNullOrWhiteSpace(emotion))
            {
                throw new BusinessRuleValidationException("Need to introduce an Emotion");
            }

            emotion = emotion.Trim().ToLower();
            emotion=char.ToUpper(emotion[0])+emotion.Substring(1);
            OCC result;
            if(!Enum.TryParse(emotion,out result)){
                throw new BusinessRuleValidationException($"Invalid Input {emotion} for Emotion",nameof(emotion));
            }
            this._Emotion = emotion;
        }
    }
}