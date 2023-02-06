const db = require('../db');

async function getItemName(barcode)
{
	const sql = `
    SELECT "Naziv"
    FROM "Proizvod"
	WHERE "Barkod" = $1::text
    `;
    const sql_parameters = [barcode];
	
	try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length <= 0){
            return undefined
        } else {
            return result.rows[0].Naziv;
        }

    } catch (err) {
        console.log(err);
        return "";
    }
}

async function getStores(barcode)
{
	const sql = `
    SELECT "Naziv" name, "TrgovinaID", "Cijena"
    FROM "ProizvodTrgovina" JOIN "Trgovina" ON "ProizvodTrgovina"."TrgovinaID" = "Trgovina"."ID"
	WHERE "Barkod" = $1::text
    `;
    const sql_parameters = [barcode];
	
	let stores;
	try {
        stores = await db.query(sql, sql_parameters);
        
        if(stores.rows.length <= 0){
            return [];
        } else {
            stores = stores.rows;
        }
    } catch (err) {
        console.log(err);
        return undefined;
    }
	
	for (let store of stores)
	{
		store.prices = [];
	}
	
	const sql2 = `
    SELECT "TrgovinaID", "DatumVrijeme", "NovaCijena"
    FROM "PromjenaCijeneTrgovina"
	WHERE "Barkod" = $1::text
    ORDER BY "DatumVrijeme" DESC
	LIMIT 7`;
	
	try {
        let priceChange = await db.query(sql2, sql_parameters);
		
        if(priceChange.rows.length <= 0){
            return stores;
        } else {
            for (let change of priceChange.rows)
			{
				let store = stores.find(store => store.TrgovinaID == change.TrgovinaID);
				
				if (store != undefined)
				{
					store.prices.push({
						datetime: change.DatumVrijeme,
						price: change.NovaCijena
					});
				}
			}
			return stores;
        }
    } catch (err) {
        console.log(err);
        return stores;
    }
}

async function getTags(barcode)
{
	const sql = `
    SELECT "Oznaka"
    FROM "Oznake"
	WHERE "Barkod" = $1::text
	GROUP BY "Oznaka"
	ORDER BY COUNT("KorisnikID") DESC
	LIMIT 5
    `;
    const sql_parameters = [barcode];
	
	try {
        let result = await db.query(sql, sql_parameters);
        
        if(result.rows.length < 0){
            return undefined
        } else {
			let tags = [];
			for (let tag of result.rows)
				tags.push(tag.Oznaka);
            return tags;
        }

    } catch (err) {
        console.log(err);
        return undefined;
    }
}

module.exports = {
    getItemName,
	getStores,
	getTags
}