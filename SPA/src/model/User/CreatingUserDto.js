export default class CreatingUserDto{
    constructor(email,telephoneNumber,name,avatar,city,country,linkLinkedin,linkFacebook,password,birthdaydate,description,interestTags) {
        this.email = email
        this.telephoneNumber= telephoneNumber
        this.name = name 
        this.avatar = avatar
        this.city = city
        this.country = country 
        this.linkLinkedin = linkLinkedin
        this.linkFacebook = linkFacebook
        this.password = password
        this.birthdaydate = birthdaydate
        this.description = description
        this.interestTags = interestTags
    }
}