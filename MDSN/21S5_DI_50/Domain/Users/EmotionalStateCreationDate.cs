

using DDDSample1.Domain.Shared;
using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users
{      
    [Owned]
    public class EmotionalStateCreationDate : IValueObject{

        public string _CreationDate {get; private set;}

        public EmotionalStateCreationDate (){
          this._CreationDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

    }


}