using System;
using System.Collections.Generic;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Users;

namespace Tests.TestesUnitarios.UseCasesTests.UC10_Pedido_Conexão
{
    public class Utils
    {
        public Guid ConnectionGuid = new ("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
        public DDDSample1.Domain.Users.User oUser;
        public DDDSample1.Domain.Users.User dUser;
        public string message = "test";
        public int strength=3;
        public List<string> tagList = new(){"tag1"};
        
        public Utils()
        {
            List<string> tags = new (){"A"};
            oUser = new DDDSample1.Domain.Users.User("Abc", "2000-10-10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
            "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
            dUser = new DDDSample1.Domain.Users.User("Def", "2000-10-10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
        }
        
        public CreatingRequestDTO createRequestDto()
        {
            return new CreatingRequestDTO(oUser.Id.AsGuid(), dUser.Id.AsGuid(),
                message, strength, tagList);
        }
        
        public ConnectionRequestDTO createResponseDto()
        {
            return new ConnectionRequestDTO{Id = ConnectionGuid, OrigUser = oUser.Id,
                DestUser = dUser.Id, MessageOrigToDest = message};
        }
    }
}