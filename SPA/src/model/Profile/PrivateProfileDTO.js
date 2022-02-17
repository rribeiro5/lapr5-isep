export default class PrivateProfileDTO{
    constructor(id,avatar,name,email,phoneNumber,birthdayDate,city,
        country,description,points,linkedInUrl,facebookUrl, interestTags,emotionalState){
        this.id = id
        this.avatar=avatar  
        this.name = name
        this.email = email
        this.phoneNumber = phoneNumber
        this.birthdayDate = birthdayDate
        this.city = city
        this.country = country
        this.description = description
        this.points = points
        this.linkedInURL = linkedInUrl
        this.facebookURL = facebookUrl
        this.interestTags = interestTags
        this.emotionalState = emotionalState
    }
    
}