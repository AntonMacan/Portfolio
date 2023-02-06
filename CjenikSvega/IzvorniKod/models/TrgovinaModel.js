const UserDataAccess = require('../data_access/TrgovinaDataAccess.js')

module.exports = class Trgovina {
    constructor(id,naziv) {
        this.id = id;
        this.naziv = naziv;
    }

    async addToDatabase(){
      let isUniqueID = UserDataAccess.isUniqueID(this);
        if(isUniqueID){
            await UserDataAccess.addNewTrgovina(this)
            return true
        } else {
            return false
        }
    }
}