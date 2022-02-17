using DDDSample1.Domain.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using DDDSample1.Utils;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DDDSample1.Domain.Users
{
    public class User : Entity<UserId>,IAggregateRoot
    {
        // todo adicionar facebook and linkedin url
        
        public Avatar _Avatar {get;private set;}
        [Required(ErrorMessage ="Required Name for Register")]
        public Name _Name {get;private set;}

        [Required(ErrorMessage ="Birthday date is required for register")]
        public BirthDayDate _BirthDayDate {get;private set;}
        
        public City _City {get;private set;}

        public Country _Country {get;private set;}

        [Required(ErrorMessage ="An Email is required for register")]
        public Email _Email {get;private set;}
        [Required(ErrorMessage ="An Password is required for register")]
        public Password _Password {get;private set;}

        public ProfileDescription _ProfileDescription {get;private set;}

        public Points _Points {get;private set;}

        public LinkedInProfile _LinkedInLink {get;private set;}

        public FacebookProfile _FacebookLink {get;private set;}

        public TelephoneNumber _TelephoneNumber {get;private set;}

        public EmotionalState _EmotionalState {get;private set;}

        [Required(ErrorMessage ="Need to have at least on interess tag to regist")]
        public IEnumerable<Tag> _InterestTags {get;private set;}

        public bool Active{ get;  private set; }

        public static readonly string DEFAULT_AVATAR_URL =
            "https://cdn.icon-icons.com/icons2/1378/PNG/512/avatardefault_92824.png";

        
        private User (){
            this.Active = true;
        }

        public User (string Name,string BirthDayDate,string City,string Country,string Email ,
            string Password , string Description , string number , string LinkedInLink , 
            string FacebookLink ,List<string> interessTags )
        {   

            this.Id = new UserId(Guid.NewGuid());
            this._Name = new Name(Name);
            this._BirthDayDate = new BirthDayDate(BirthDayDate);
            this._Password = new Password(Password);
            this._Email = new Email(Email);
            this._Points = new Points(0);

            if(!String.IsNullOrEmpty(City))
                this._City = new City(City);
            if(!String.IsNullOrEmpty(Country))
                this._Country = new Country(Country);
            if(!String.IsNullOrEmpty(Description))
                this._ProfileDescription = new ProfileDescription(Description);
            if(!String.IsNullOrEmpty(number))    
                this._TelephoneNumber = new TelephoneNumber(number);
            if(!String.IsNullOrEmpty(LinkedInLink))
                this._LinkedInLink = new LinkedInProfile(LinkedInLink);
            if(!String.IsNullOrEmpty(FacebookLink))
                this._FacebookLink = new FacebookProfile(FacebookLink);
            this.Active = true;

            if(interessTags.IsNullOrEmpty() || interessTags.Count > 5 || interessTags.Count<1)
                throw new BusinessRuleValidationException("User needs to have beetween 1 and 5 interess tags");

            this._InterestTags =  interessTags.ConvertAll<Tag>(e => new Tag(e));

            this._EmotionalState = new EmotionalState();

        }

         public User (string Name,string BirthDayDate,string avatarUrl,string City,string Country,string Email ,
            string Password , string Description , string number , string LinkedInLink , 
            string FacebookLink ,List<string> interessTags )
        {   

            this.Id = new UserId(Guid.NewGuid());
            this._Name = new Name(Name);
            this._BirthDayDate = new BirthDayDate(BirthDayDate);
            this._Password = new Password(Password);
            this._Email = new Email(Email);
            this._Points = new Points(0);

            if(!String.IsNullOrEmpty(avatarUrl)){
                this._Avatar = new Avatar(avatarUrl);
            }else{
                this._Avatar = new Avatar(DEFAULT_AVATAR_URL);
            }
                
            if(!String.IsNullOrEmpty(City))
                this._City = new City(City);
            if(!String.IsNullOrEmpty(Country))
                this._Country = new Country(Country);
            if(!String.IsNullOrEmpty(Description))
                this._ProfileDescription = new ProfileDescription(Description);
            if(!String.IsNullOrEmpty(number))    
                this._TelephoneNumber = new TelephoneNumber(number);
            if(!String.IsNullOrEmpty(LinkedInLink))
                this._LinkedInLink = new LinkedInProfile(LinkedInLink);
            if(!String.IsNullOrEmpty(FacebookLink))
                this._FacebookLink = new FacebookProfile(FacebookLink);
            this.Active = true;

            this._EmotionalState = new EmotionalState();

            if(interessTags.IsNullOrEmpty() || interessTags.Count > 5 || interessTags.Count<1)
                throw new BusinessRuleValidationException("User needs to have beetween 1 and 5 interess tags");

            this._InterestTags =  interessTags.ConvertAll<Tag>(e => new Tag(e));

        }


        protected bool Equals(User other)
        {
            return Equals(_Name, other._Name);
        }
        
        public override int GetHashCode()
        {
            return (_Name != null ? _Name.GetHashCode() : 0);
        }

        public bool HasId(UserId oId)
        {
            return this.Id.Equals(oId);
        }

        public UserSuggestedDto toUserSuggestedDto()
        {
            var l = new List<string>();
            foreach (var interestTag in _InterestTags)
            {
                l.Add(interestTag.Description);
            }
            //todo adicionar avatar
            return new UserSuggestedDto {
                id=Id.AsGuid(),
                name=_Name?._Name,
                email = _Email._Email,
                avatar = _Avatar?._avatarUrl,
                birthdayDate = _BirthDayDate?._BirthDayDate,
                city = _City?._City,
                country=_Country?._Country,
                profileDescription = _ProfileDescription?._Description,
                linkedInURL = _LinkedInLink?._ProfileUrl,
                interestTagsDtoList = l,
            };
        }
        //todo adicionar avatar
        public UserPrivateProfileDto toUserPrivateProfileDto()
        {
            var tags=new List<string>();
            foreach (var interestTag in _InterestTags)
            {
                tags.Add(interestTag.Description);
            }

            return new UserPrivateProfileDto(Id.AsGuid(),_Avatar?._avatarUrl,_Name?._Name,_Email?._Email,_TelephoneNumber?._Number,
                _BirthDayDate?._BirthDayDate,_City?._City,_Country?._Country,_ProfileDescription?._Description,
                _Points._Points,_LinkedInLink?._ProfileUrl,_FacebookLink?._ProfileUrl, tags,_EmotionalState?.ToString());
        }

        public bool UpdateEmotionalState(string state)
        {
            if (state.IsNullOrEmpty()) return false;
            if (this._EmotionalState != null && _EmotionalState.Emotion._Emotion.Equals(state)) return false;
            this._EmotionalState = new EmotionalState(state);
            return true;
        }

        public void UpdateCurrentValueEmotionalState(double newValue)
        {   
            this._EmotionalState = new EmotionalState(this._EmotionalState.Emotion._Emotion,newValue);
        }

        public UserSearchedDTO toUserSearchedDTO()
        {
            var l = new List<string>();
            foreach (var interestTag in _InterestTags)
            {
                l.Add(interestTag.Description);
            }
            return new UserSearchedDTO(
                Id.AsGuid(),
                _Name?._Name,
                _Avatar?._avatarUrl,
                _Country?._Country,
                _Email?._Email,
                l
            );
        }
        
        public UserLeaderboardDTO ToUserLeaderboardDTO(int value)
        {
            return new UserLeaderboardDTO(
                Id.AsGuid(),
                _Email?._Email,
                _Name?._Name,
                _Avatar?._avatarUrl,
                value
            );
        }

        public UserDto ToDto()
        {
            var l = _InterestTags.Select(interestTag => interestTag.Description).ToList();
            return new UserDto(Id.AsGuid(), _BirthDayDate?._BirthDayDate, _Email?._Email,
                _Name?._Name, _Avatar?._avatarUrl,  _EmotionalState?.Emotion._Emotion,l);
        }
        
        public void Update(UserPrivateProfileDto dto)
        {
            if(dto.Avatar!=null)
                _Avatar = new Avatar(dto.Avatar);
            _Name = new Name(dto.Name);
            _BirthDayDate = new BirthDayDate(dto.BirthdayDate);
            if(dto.PhoneNumber!=null)
                _TelephoneNumber = new TelephoneNumber(dto.PhoneNumber);
            if(dto.City!=null)
                _City = new City(dto.City);
            if(dto.Country!=null)
                _Country = new Country(dto.Country);
            if(dto.Description!=null)
                _ProfileDescription = new ProfileDescription(dto.Description);
            if(dto.LinkedInURL!=null)
                _LinkedInLink = new LinkedInProfile(dto.LinkedInURL);
            if(dto.FacebookURL!=null)
                _FacebookLink = new FacebookProfile(dto.FacebookURL);
            if (dto.InterestTags != null)
                _InterestTags=dto.InterestTags.Select(tag => new Tag(tag)).ToList();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            var o = (User) obj;
            return o != null && this.Id.Equals(o.Id);
        }
    }
}