const UserDataAccess = require('../data_access/UserDataAccess.js')

module.exports = class User {
    constructor(nickname, first_name, last_name, email, password_hash, access_level, privacy, access_forbidden) {
        this.id = undefined
        this.nickname = nickname
        this.first_name = first_name
        this.last_name = last_name
        this.email = email
        this.password_hash = password_hash
        this.access_level = access_level
        this.privacy = privacy
        this.access_forbidden = access_forbidden
    }

    async addToDatabase(){
        let unique = await UserDataAccess.wouldBeUnique(this);
        if((this.id == undefined) && unique){
            await UserDataAccess.addNewUser(this)
            return true
        } else {
            return false
        }
    }
}