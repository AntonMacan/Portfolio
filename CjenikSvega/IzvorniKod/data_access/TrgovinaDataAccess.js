const db = require('../db')
const Trgovina = require('../models/TrgovinaModel')
const moment = require("moment")

isUniqueID = async(id) => {
   const sql = 'SELECT COUNT("ID") FROM "Trgovina" WHERE "ID" = $1::int'
   const sql_parameters = [id];
   try{
       let result = await db.query(sql, sql_parameters);
       let count = result.rows[0];
       if(count.count < 1){
           return true;
       }else{
           return false;
       }
       
   }catch(err){
       console.log(err);
       throw err
   }
}

addNewTrgovina = async(trgovina) =>{
   const sql = `
   INSERT INTO "Trgovina" ("ID", "Naziv") VALUES
   ($1::int, $2::text) RETURNING "ID";
`;

   const sql_parameters = [trgovina.id, trgovina.naziv];
   try {
      const result = await db.query(sql, sql_parameters);
      return result.rows[0].id;
   } catch (err) {
      console.log(err);
      throw err
   }
}

getTrgovina = async(user_id) =>{
   const sql = `
   select "Naziv" from "Trgovina" WHERE "ID" = $1::int;
`;

   const sql_parameters = [user_id];
   try {
      const result = await db.query(sql, sql_parameters);
      if(result.rows.length <= 0){
         return undefined
     } else {
      let trgovina = {};

      trgovina.id = user_id
      trgovina.naziv = result.rows[0].Naziv
      trgovina.wrong_prices = await getWrongPricesForStore(trgovina.id);
      trgovina.comment = await getCommentForStore(trgovina.id);

      console.log(JSON.stringify(trgovina))

      return trgovina;
     }
      
   } catch (err) {
      console.log(err);
      throw err
   }
}

getWrongPricesForStore = async (store_id) => {
   const sql = `
   SELECT COUNT(*) AS "KriveCijene" FROM "PromjenaCijeneKorisnik"
   WHERE ("TrgovinaID" = $1::int AND "Status" = 'accepted')
   `;

   const sql_parameters = [store_id];

   try {
      const result = await db.query(sql, sql_parameters);

      return result.rows[0].KriveCijene
      
   } catch (err) {
      console.log(err);
      throw err
   }
}

getCommentForStore = async (store_id) => {
   const sql = `
   SELECT "OpisKomentara" FROM "Komentar"
   WHERE "TrgovinaID" = $1::int
   `;

   const sql_parameters = [store_id];

   try {
      const result = await db.query(sql, sql_parameters);
      if(result.rows.length <= 0){
         return ""
      } else {
         return result.rows[0].OpisKomentara
      }      
      
   } catch (err) {
      console.log(err);
      throw err
   }
}

putItemsInStore = async (trgovinaID, barkod, proizvod, cijena) => {
   if (parseFloat(cijena) == NaN)
   {
	   console.log('Cijena nije dobro upisana');
	   return;
   }
   
   let sql = 'INSERT INTO "Proizvod" VALUES ($1::text, $2::text)';       
   let sql_parameters = [barkod, proizvod];

   let sqlControl = 'SELECT * FROM "Proizvod" WHERE "Barkod" = $1::text';
   let sqlControl_params = [barkod];

   let resultControl = await db.query(sqlControl, sqlControl_params);
   if(resultControl.rows.length == 0){
      try {
         let result = await db.query(sql, sql_parameters);
         console.log(result);  
      } catch (err) {
         console.log(err);
      }
   }

   sql = 'INSERT INTO "ProizvodTrgovina" VALUES ($1::text, $2::integer, $3::float)';
   sql_parameters = [barkod, trgovinaID, cijena];
   // Dodano u promjenacijenetrgovina tablicu radi pracenja povijesti 
   var now  = moment().format("YYYY-MM-DD HH:mm:ss");
   console.log(now);
   sqlPromjena = 'INSERT INTO "PromjenaCijeneTrgovina" VALUES($1::text,$2::int,$3::timestamp,$4::float)'
   sql_params = [barkod,trgovinaID,now,cijena];


   try {
      result = await db.query(sql, sql_parameters);
      result += await db.query(sqlPromjena,sql_params);
      console.log(result);      
   } catch (err) {
      console.log(err);

   }

}

commentExists = async (admin_id, store_id) =>{
   const sql = 'SELECT * FROM "Komentar" WHERE ("KorisnikID" = $1::int AND "TrgovinaID"= $2::int)'
   const sql_parameters = [admin_id, store_id];
   try{
       let result = await db.query(sql, sql_parameters);
       
       if(result.rows.length > 0){
         return true
       } else {
         return false
       }
       
   }catch(err){
       console.log(err);
       throw err
   }
}

existsTrgovinaWithId = async (store_id) =>{
   const sql = 'SELECT * FROM "Trgovina" WHERE "ID" = $1::int'
   const sql_parameters = [store_id];
   try{
       let result = await db.query(sql, sql_parameters);
       
       if(result.rows.length > 0){
         return true
       } else {
         return false
       }
       
   }catch(err){
       console.log(err);
       throw err
   }
}

addComment = async (admin_id, store_id, comment) => {
   if(await commentExists(admin_id, store_id)){
      //update query

      const sql = `
      UPDATE "Komentar"
      SET "OpisKomentara" = $3::text
      WHERE ("KorisnikID" = $1::int AND "TrgovinaID"= $2::int);
      `
      const sql_parameters = [admin_id, store_id, comment]

      try {
         let result = await db.query(sql, sql_parameters);
      } catch (err) {
         console.log(err)
         throw err
      }
   } else {
      //insert query

      const sql = 'INSERT INTO "Komentar" ("KorisnikID", "TrgovinaID", "OpisKomentara") VALUES ($1::int, $2::int, $3::text)'
      const sql_parameters = [admin_id, store_id, comment]

      try {
         let result = await db.query(sql, sql_parameters);
      } catch (err) {
         console.log(err)
         throw err
      }
   }
}

checkIfItemExists = async(barkod) => {
   let sql = 'SELECT * FROM "Proizvod" WHERE "Barkod" = $1::text';
   let sql_parameters = [barkod];
   let resultControl;
   try{
   resultControl = await db.query(sql,sql_parameters);
   }catch(err){
      console.log(err);
      throw err;
   }
   if(resultControl.rows.length == 0){
      return false;
   }
   return true;
}


checkIfItemExistsInShop = async(barkod,trgovinaId) => {
   let sql2 = 'SELECT * FROM "ProizvodTrgovina" WHERE "TrgovinaID" = $1::int AND "Barkod" = $2::text';
   let sql_parameters2 = [trgovinaId,barkod];
   let result2
   try{
   result2= await db.query(sql2,sql_parameters2);
   }catch(err){
      console.log(err)
      throw err;
   }
   
   if(result2 != undefined && result2.rows.length == 0){
      return false;
   }
   return true;
}

changePriceInShop = async(cijena,barkod,trgovinaId) =>{
   let sql2 = 'SELECT "Cijena" FROM "ProizvodTrgovina" WHERE "TrgovinaID" = $1::int AND "Barkod" = $2::text';
   let sql_parameters2 = [trgovinaId,barkod];
   let result2
   try{
      result2 = await db.query(sql2,sql_parameters2);
   }catch(err){
      console.log(err)
      throw err;
   }
   if(result2 != undefined && result2.rows.length > 0){
      if(cijena != result2.rows[0].Cijena){
         console.log("Mijenja se: iz "+result2.rows[0].Cijena+" u "+cijena);
         let sqlPromjena = 'UPDATE "ProizvodTrgovina" SET "Cijena" = $1::float WHERE "TrgovinaID" = $2::int AND "Barkod" = $3::text';
         let sql_params = [cijena, trgovinaId, barkod];
         try{
         await db.query(sqlPromjena,sql_params);
         }catch(err){
            throw err;
         }
         var now  = moment().format("YYYY-MM-DD HH:mm:ss");
         console.log(now);
         sqlPromjena = 'INSERT INTO "PromjenaCijeneTrgovina" VALUES($1::text,$2::int,$3::timestamp,$4::float)'
         sql_params = [barkod,trgovinaId,now,cijena];
         try{
            await db.query(sqlPromjena,sql_params);
            }catch(err){
               throw err;
            }
      }
   }
}



module.exports = {
   isUniqueID,
   addNewTrgovina,
   getTrgovina,
   putItemsInStore,
   changePriceInShop,
   checkIfItemExistsInShop,
   checkIfItemExists,
   addComment,
   existsTrgovinaWithId
}