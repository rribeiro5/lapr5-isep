using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class UpdateEmotionalStateGroupSugestions
    {
        public string userId {get; set;}

        public List<string> suggestedUsers{ get; set; }

        public List<string> hopeUsers{ get; set; }

        public List<string> scareUsers{ get; set; }

        public UpdateEmotionalStateGroupSugestions(string _id, List<string> _suggestedUsers,List<string> _hopeUsers,List<string> _scareUsers)
        {
            this.userId = _id;
            this.hopeUsers = _hopeUsers;
            this.suggestedUsers = _suggestedUsers;
            this.scareUsers = _scareUsers;
        }
    }
}