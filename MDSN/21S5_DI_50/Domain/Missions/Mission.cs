using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DDDSample1.Domain.Mission
{
    public class Mission : Entity<MissionId>,IAggregateRoot
    {   

        // TODO adicionar UserId representativo do objetivo e o user que comecou e o estado da missao
        public UserId Player {get;private set;}

        public UserId Objective {get;private set;}

        public MissionStatus CurrentStatus {get;private set;}
        public Difficulty Difficulty { get; private set; }
        public MissionDateTime DateTime { get; private set; }
        
        protected Mission(){/* Constructor for EF only*/}

        public Mission(int difficultyLevel,UserId player, UserId objective)
        {
            this.Id = new MissionId(Guid.NewGuid());
            this.Difficulty = new Difficulty(difficultyLevel);
            this.DateTime = new MissionDateTime();
            this.Player = player;
            this.Objective = objective;
            this.CurrentStatus = new MissionStatus("Active");
        }
    }
}