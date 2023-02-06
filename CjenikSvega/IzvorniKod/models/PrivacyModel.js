module.exports = class Privacy{
    constructor(is_nickname_public, is_first_name_public, is_last_name_public, is_email_public){
        this.is_nickname_public = is_nickname_public
        this.is_first_name_public = is_first_name_public
        this.is_last_name_public = is_last_name_public
        this.is_email_public = is_email_public
    }
}