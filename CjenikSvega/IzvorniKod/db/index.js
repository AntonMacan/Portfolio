const {Pool} = require('pg');

//finding enviornment variables
const enviornment = require('../enviornments/enviornment');

//connection to database
const pool = new Pool({
    user: enviornment.Database.user,
    host: enviornment.Database.host,
    database: enviornment.Database.dbName,
    password: enviornment.Database.password,
    port: enviornment.Database.port,
	ssl: enviornment.Database.ssl
});

module.exports = {
    query: (text, params) => {
        const start = Date.now();
        return pool.query(text, params)
            .then(res => {
                const duration = Date.now() - start;
                console.log('executed query', {text, params, duration, rows: res.rows});
                return res;
            });
    },
    pool: pool
}