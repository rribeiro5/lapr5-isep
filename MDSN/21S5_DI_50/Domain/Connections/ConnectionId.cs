using System;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Connections
{

  public class ConnectionId : EntityId {

      public ConnectionId(Guid value) : base(value)
      {
        
      }
      
      public ConnectionId(String value) : base(value)
      {
        
      }

      
    override
      protected  Object createFromString(String text)
      {
        return new Guid(text);
      }
    
    override
      public String AsString()
      {
        Guid obj = (Guid) ObjValue;
        return obj.ToString();
      }

      public Guid AsGuid(){
            return (Guid) base.ObjValue;
        }
  }

}