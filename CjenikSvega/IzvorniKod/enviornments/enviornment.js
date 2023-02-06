const enviorment = {
    SessionSecret: "topSecret",
    Database: {
        host: process.env.dbHost,
        port: parseInt(process.env.dbPort),
        dbName: process.env.dbName,
        user: process.env.dbUser,
        password: process.env.dbPassword
    }

}

module.exports = enviorment;
