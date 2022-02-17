using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class ResponseUpdateEmotionalStateGroupSugestions
    {
        public string userId {get; set;}

        public List<string> suggestedUsers{ get; set; }

        public List<string> hopeUsers{ get; set; }

        public List<string> scareUsers{ get; set; }

        public double newEmotionValue {get; set; }

        public ResponseUpdateEmotionalStateGroupSugestions(string userId, List<string> suggestedUsers,List<string> hopeUsers,List<string> scareUsers,double newEmotionValue)
        {
            this.userId = userId;
            this.hopeUsers = hopeUsers;
            this.suggestedUsers = suggestedUsers;
            this.scareUsers = scareUsers;
            this.newEmotionValue = newEmotionValue;
        }

    
    }
}