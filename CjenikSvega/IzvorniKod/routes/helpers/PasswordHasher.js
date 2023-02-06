const bcrypt = require('bcrypt')

module.exports = {
    hash: (password) => {
        const salt = 10
    
        return bcrypt.hashSync(password, salt)
    },
    
    compare: (text, hashedPassword) => {
        return bcrypt.compareSync(text, hashedPassword)
    }

}