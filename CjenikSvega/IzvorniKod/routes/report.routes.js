const express = require('express');
const ItemAccess = require('../data_access/ItemDataAccess.js');
const TrgovinaAccess = require('../data_access/TrgovinaDataAccess.js');
const router = express.Router();
const db = require('../db');
const moment = require('moment');
const path = require('path');

router.get('/:trgId', async (req, res) => {
	
	if (req.session.user == undefined)
	{
		res.redirect('/login');
        return;
	}
	
	let trgId = req.params.trgId;
	let barcode = req.query.item;
	let itemName = await ItemAccess.getItemName(barcode);
	
	if (!await TrgovinaAccess.existsTrgovinaWithId(trgId) || itemName == undefined)
		return res.status(404).send('404 not found');
	
	itemName = itemName.charAt(0).toUpperCase() + itemName.slice(1);
	
	let trg = await TrgovinaAccess.getTrgovina(trgId);
	
	res.render('report', {
		linkActive: 'user',
		user: req.session.user,
		barcode: barcode,
		storeId: trgId,
		itemName: itemName,
		storeName: trg.naziv
	});
});

router.post('/:trgId', async (req, res) => {
	
	if (req.session.user == undefined)
	{
		res.redirect('/login');
        return;
	}
	
	if (!req.files)
	{
		return res.status(400).send('Slika je obavezna');
	}
	
	let newPrice = parseFloat(req.body.newprice);
	if (isNaN(newPrice))
		return res.status(400).send('Nova cijena nije ispravna');
	
	let trgId = req.params.trgId;
	let barcode = req.query.item;
	
	if (!await TrgovinaAccess.existsTrgovinaWithId(trgId) || await ItemAccess.getItemName(barcode) == undefined)
		return res.status(400).send('400 bad request');
	
	const file = req.files.filename;
	const extensionName = path.extname(file.name);
	const allowedExtension = ['.png','.jpg','.jpeg'];
	const fileName = Date.now() + extensionName;
	const targetPath = './public/userImages/' + fileName;
	
	if(!allowedExtension.includes(extensionName)){
		return res.status(422).send('Format slike nije podržan');
	}
	
	let err = await file.mv(targetPath);
	
	if (err != undefined)
	{
		console.log(err);
		return res.status(500).send('Greška prilikom spremanja slike');
	}
	
	const sql = `
   INSERT INTO "PromjenaCijeneKorisnik" ("KorisnikID", "Barkod", "TrgovinaID", 
		"DatumVrijeme", "NovaCijena", "Status", "SlikaPath") VALUES
   ($1::int, $2::text, $3::int, $4::timestamp, $5::float, $6::text, $7::text);
`;

	const sql_parameters = [req.session.user.id, barcode, trgId, moment().format("YYYY-MM-DD HH:mm:ss"), newPrice, 'in progress', 'userImages/' + fileName];
	
	try {
      const result = await db.query(sql, sql_parameters);
	} catch (err) {
      console.log(err);
      return res.status(500).send('Greška pri dodavanju u bazu');
   }
	
	res.redirect('/');
});

module.exports = router;