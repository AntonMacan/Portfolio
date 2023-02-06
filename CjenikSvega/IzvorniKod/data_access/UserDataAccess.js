const db = require('../db')
const User = require('../models/UserModel')
const Privacy = require('../models/PrivacyModel')

addNewUser = async (user) => {
    const sql = `
        INSERT INTO "Korisnik" ("Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa") VALUES
        ($1::text, $2::text , $3::text , $4::text , $5::text, $6::smallint) RETURNING "ID";
    `;

        const sql_parameters = [user.first_name, user.last_name, user.email,user.nickname,  user.password_hash, user.access_level];
    try {
        const result = await db.query(sql, sql_parameters);
        await initPrivacy(result.rows[0].ID)
        return result.rows[0].ID;
    } catch (err) {
        console.log(err);
        throw err
    }
}

existsWithId = async (id) =>{
    const sql = `
    SELECT "ID", "Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa"
    FROM "Korisnik"
    WHERE "ID" = $1::int
    `
    const sql_parameters = [id]

    try {
        let result = await db.query(sql, sql_parameters);
        return result.rows.length > 0;
    } catch (err) {
        console.log(err);
        throw err
    }
}

//returns true if there is not user in database that is already registered with that nickname or email
wouldBeUnique = async (user) => {
    const sql = `
    SELECT "ID", "Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa"
    FROM "Korisnik"
    WHERE ("Nadimak" = $1::text OR "Email" = $2::text)
    `
    const sql_parameters = [user.nickname, user.email]
    try {
        let result = await db.query(sql, sql_parameters);
        return result.rows.length == 0;
    } catch (err) {
        console.log(err);
        throw err
    }
} 

getById = async (id) =>{
    const sql = `
    SELECT "ID", "Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa", "ZabranjenPristup"
    FROM "Korisnik"
    WHERE "ID" = $1::int
    `
    const sql_parameters = [id]

    try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length <= 0){
            return undefined
        } else {
            
            let results = result.rows

            let newUser = {}
            newUser.nickname = results[0].Nadimak
            newUser.first_name = results[0].Ime
            newUser.last_name = results[0].Prezime
            newUser.email = results[0].Email
            newUser.password_hash = results[0].Lozinka,
            newUser.access_level = results[0].RazinaPristupa
            newUser.id = results[0].ID
            newUser.access_forbidden = results[0].ZabranjenPristup
            newUser.privacy = await getPrivacyForUser(newUser.id)

            return newUser
        }

    } catch (err) {
        console.log(err);
        throw err
    }
}

getByNickname = async (nickname) =>{
    const sql = `
    SELECT "ID", "Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa", "ZabranjenPristup"
    FROM "Korisnik"
    WHERE "Nadimak" = $1::text
    `
    const sql_parameters = [nickname]

    try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length <= 0){
            return undefined
        } else {
            
            let results = result.rows

            let newUser = {}
            newUser.nickname = results[0].Nadimak
            newUser.first_name = results[0].Ime
            newUser.last_name = results[0].Prezime
            newUser.email = results[0].Email
            newUser.password_hash = results[0].Lozinka,
            newUser.access_level = results[0].RazinaPristupa
            newUser.id = results[0].ID
            newUser.access_forbidden = results[0].ZabranjenPristup
            newUser.privacy = await getPrivacyForUser(newUser.id)

            return newUser
        }

    } catch (err) {
        console.log(err);
        throw err
    }
}

getByEmail = async (email) =>{
    const sql = `
    SELECT "ID", "Ime", "Prezime" , "Email" , "Nadimak" , "Lozinka", "RazinaPristupa"
    FROM "Korisnik"
    WHERE "Email" = $1::text
    `
    const sql_parameters = [email]

    try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length <= 0){
            return undefined
        } else {
            
            let results = result.rows

            let newUser = {}
            newUser.nickname = results[0].Nadimak
            newUser.first_name = results[0].Ime
            newUser.last_name = results[0].Prezime
            newUser.email = results[0].Email
            newUser.password_hash = results[0].Lozinka,
            newUser.access_level = results[0].RazinaPristupa
            newUser.id = results[0].ID
            newUser.access_forbidden = results[0].ZabranjenPristup
            newUser.privacy = await getPrivacyForUser(newUser.id)

            return newUser
        }

    } catch (err) {
        console.log(err);
        throw err
    }
}

getPrivacyForUser = async (user_id) => {
    const sql = `
    SELECT "KorisnikID", "Ime", "Prezime", "Email", "Nadimak"
    FROM "Privatnost"
	WHERE "KorisnikID" = $1::int
    `    
    const sql_parameters = [user_id]

    let newPrivacy = new Privacy(true, true, true, true)

    try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length <= 0){
            return undefined
        } else {
            
            let results = result.rows

            newPrivacy.is_nickname_public = results[0].Nadimak
            newPrivacy.is_first_name_public = results[0].Ime
            newPrivacy.is_last_name_public = results[0].Prezime
            newPrivacy.is_email_public = results[0].Email

            return newPrivacy
        }

    } catch (err) {
        console.log(err);
        throw err
    }
}

initPrivacy = async (user_id) =>{
    const sql = `
    INSERT INTO "Privatnost" ("KorisnikID", "Ime", "Prezime", "Email", "Nadimak")
    VALUES ($1::int, true, true, true, true)
    `    
    const sql_parameters = [user_id]

    try {
        let result = await db.query(sql, sql_parameters);

    } catch (err) {
        console.log(err);
        throw err
    }
}

privacyExists = async (user_id) => {

    sql = `
    SELECT * FROM "Privatnost"
    WHERE "KorisnikID" = $1::int`

    const sql_parameters = [user_id]
    try {
        let result = await db.query(sql, sql_parameters);
        return result.rows.length > 0;
    } catch (err) {
        console.log(err);
        throw err
    }
}

updatePrivacy = async (user_id, attribute, isPublic) => {
    attributeInSql = ""
    if(attribute == 'nickname'){
        attributeInSql = `"Nadimak"`
    } else if(attribute == 'first_name'){
        attributeInSql = `"Ime"`
    } else if(attribute == 'last_name'){
        attributeInSql = `"Prezime"`
    } else if(attribute == 'email'){
        attributeInSql = `"Email"`
    } else {
        throw "Krivi atribut"
    }

    if(!(await privacyExists(user_id))){
        await initPrivacy(user_id)
    }

    sql = `UPDATE "Privatnost" SET ` + attributeInSql + ` = $2 where "KorisnikID" = $1::int`
    const sql_parameters = [user_id, isPublic]

    try {
        let result = await db.query(sql, sql_parameters);
    } catch (err) {
        console.log(err);
        throw err
    }

}

forbidAccess = async (user_id) => {
    sql = `
    UPDATE "Korisnik" SET "ZabranjenPristup" = TRUE WHERE "ID" = $1::int`

    const sql_parameters = [user_id]
    try {
        let result = await db.query(sql, sql_parameters);

    } catch (err) {
        console.log(err);
        throw err
    }
}

isAccessForbidden = async (user_id) => {
    sql = `
    SELECT "ZabranjenPristup" FROM "Korisnik" WHERE "ID" = $1::int`

    const sql_parameters = [user_id]
    try {
        let result = await db.query(sql, sql_parameters);
        if(result.rows.length == 0){
            return false
        }

        return result.rows[0].ZabranjenPristup;
    } catch (err) {
        console.log(err);
        throw err
    }
}

module.exports = {
    addNewUser,
    existsWithId,
    getById,
    getByNickname,
    getByEmail,
    wouldBeUnique,
    updatePrivacy,
    getPrivacyForUser,
    forbidAccess,
    isAccessForbidden
}
