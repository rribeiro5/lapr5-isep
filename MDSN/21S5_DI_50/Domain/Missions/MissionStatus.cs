

using DDDSample1.Domain.Shared;
using System;
using Microsoft.EntityFrameworkCore;


namespace DDDSample1.Domain.Mission
{      

    enum Status {
       Active,
       Success,
       Failed
    }

    [Owned]
    public class MissionStatus : IValueObject{

        public string _Status {get; private set;}

        private MissionStatus(){}

        public MissionStatus (string status){

            if (String.IsNullOrWhiteSpace(status))
            {
                throw new BusinessRuleValidationException("Need to introduce an Mission Status");
            }

            Status result;
            if(!Enum.TryParse(status,out result)){
                throw new BusinessRuleValidationException($"Invalid Input {status} for Emotion",nameof(status));
            }
            this._Status = status;
        }
    }
}