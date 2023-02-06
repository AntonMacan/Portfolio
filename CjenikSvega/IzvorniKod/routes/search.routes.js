const express = require('express');
const db = require('../db');
const router = express.Router();

router.get('/:term', async (req, res) => {
	
	let sql = 'SELECT DISTINCT "Proizvod"."Naziv" AS naziv, "Proizvod"."Barkod" AS barkod ' +
		'FROM "Proizvod" LEFT JOIN "Oznake" USING("Barkod") ' +
		'WHERE LOWER("Proizvod"."Naziv") LIKE LOWER($1::text) OR LOWER("Oznaka") LIKE LOWER($1::text)';

	let sql_parameters = [req.params.term + '%'];

	let products = await db.query(sql, sql_parameters);
	for (let item of products.rows)
		item.naziv = item.naziv.charAt(0).toUpperCase() + item.naziv.slice(1);

	sql = 'SELECT "Naziv" AS naziv, "ID" AS id FROM "Trgovina" WHERE LOWER("Naziv") LIKE LOWER($1::text)';
	let trgovine = await db.query(sql, sql_parameters);


	res.render('search', {
		linkActive: 'search',
		user: req.session.user,
		proizvodi: products.rows,
		trgovine: trgovine.rows
	})

});

router.get('/', async (req, res) => {

	let sql = 'SELECT DISTINCT "Proizvod"."Naziv" AS naziv, "Proizvod"."Barkod" AS barkod ' +
	'FROM "Proizvod" LEFT JOIN "Oznake" USING("Barkod") ' +
	'WHERE LOWER("Proizvod"."Naziv") LIKE LOWER($1::text) OR LOWER("Oznaka") LIKE LOWER($1::text)';

	let sql_parameters = [req.query.item + '%'];

	let products = await db.query(sql, sql_parameters);

	sql = 'SELECT "Naziv" AS naziv, "ID" AS id FROM "Trgovina" WHERE LOWER("Naziv") LIKE LOWER($1::text)';
	let trgovine = await db.query(sql, sql_parameters);


	res.render('search', {
		linkActive: 'search',
		user: req.session.user,
		proizvodi: products.rows,
		trgovine: trgovine.rows
	})
})

module.exports = router;