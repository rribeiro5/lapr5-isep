using System;
using System.Collections.Generic;

namespace DDDSample1.Domain.Users
{
    public class GroupSuggestionsDTO
    {   
        
        public List<string>  grupo { get; set; }


        public GroupSuggestionsDTO(List<string>  grupo) {    
            this.grupo = grupo;
        }

      
    }



}