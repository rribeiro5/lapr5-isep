using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Mission
{
    public class MissionId : EntityId
    {
        public MissionId(Guid value) : base(value) {}
    
        override
        protected  object createFromString(string text)
        {
            return new Guid(text);
        }
        
        override
        public  string AsString()
        {
            Guid obj = (Guid) ObjValue;
            return obj.ToString();
        }
    }
}