using System;

namespace DDDSample1.Domain.Users
{
    public class UpdateEmotionalStateLikesDTO
    {
        public string userId {get; set;}

        public int differenceLikes { get; set; }

        public UpdateEmotionalStateLikesDTO(string _id,int _differenceLikes)
        {
            this.userId = _id;
            this.differenceLikes = _differenceLikes;
        }
    }
}