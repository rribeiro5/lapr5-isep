using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class GetGroupSuggestionsDTO
    {   
        public string userId { get; set;}
        public List<string>  lTagsObrigatorias { get; set; }

        public int nTagsComum { get; set; }

        public int nMinimoUsers { get; set; }

        public List<string> desired { get; set; }
    
        public List<string> toAvoid { get; set; }
       


        public GetGroupSuggestionsDTO(string userId,List<string>  lTagsObrigatorias,int nTagsComum,int nMinimoUsers,List<string> desired,List<string> toAvoid) {    
            this.userId = userId;
            this.lTagsObrigatorias = lTagsObrigatorias;
            this.nTagsComum = nTagsComum;
            this.nMinimoUsers = nMinimoUsers;
            this.desired = desired;
            this.toAvoid = toAvoid;
        }

      
    }



}