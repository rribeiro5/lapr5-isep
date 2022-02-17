using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;
namespace DDDSample1.Domain.Users{
    [Owned]
    public class EmotionalState: IValueObject{


        public EmotionalStateCreationDate _creationDate {get;private set;}

        public OCCEmotion Emotion{get;private set;}

        public EmotionalValue Value {get;private set;}
        
        public EmotionalState(){
            this._creationDate = new EmotionalStateCreationDate();
            this.Emotion = new OCCEmotion();
            this.Value = new EmotionalValue();
        }
        public EmotionalState(string emotion){
            this._creationDate = new EmotionalStateCreationDate();
            this.Emotion = new OCCEmotion(emotion);
            this.Value = new EmotionalValue();
        }

         public EmotionalState(string emotion,double value){
            this._creationDate = new EmotionalStateCreationDate();
            this.Emotion = new OCCEmotion(emotion);
            this.Value = new EmotionalValue(value);
        }


    }



}