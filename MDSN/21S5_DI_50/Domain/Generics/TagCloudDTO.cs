using System;
using System.Collections.Generic;

namespace DDDNetCore.Domain.Generics
{
    public class TagCloudDTO
    {
        public string value { get; set; }
        public int count{get; set;}

        public TagCloudDTO(string value, int count)
        {
            this.value = value;
            this.count = count;
        }
    }
}