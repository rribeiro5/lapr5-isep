using System;

namespace DDDSample1.Domain.Users
{
    public class UserUpdateEmotionalStateLikesResponseDTO
    {
        public string userId{ get; set; }
        
        public int differenceLikes { get; set; }
        
        public double newEmotionValue {get;set;}

        public UserUpdateEmotionalStateLikesResponseDTO(string userId, int differenceLikes ,double newEmotionValue)
        {
           this.userId = userId;
           this.differenceLikes = differenceLikes;
           this.newEmotionValue = newEmotionValue;
        }
    }
}