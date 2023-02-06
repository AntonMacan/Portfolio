const express = require('express');
const router = express.Router();

const User = require('../models/UserModel')
const UserDataAccess = require('../data_access/UserDataAccess')
const PasswordHasher = require('./helpers/PasswordHasher')

router.get('/', (req, res) => {
	
	if(req.session.user === undefined) {
		res.render('login', {
			linkActive: 'login',
			user: req.session.user,
			error: ""
		});
	} else {
		res.redirect('/');
	}

});


router.post('/', async (req, res) => {
	
	const username = req.body.username
	const password = req.body.password

	let user = await UserDataAccess.getByNickname(username)
	
	if(user === undefined){
		res.render('login', {
			linkActive: 'login',
			user: req.session.user,
			error: "Incorrect username or password."
		});
		return;
	}

	console.log('kurcina')

	let passwordValid = PasswordHasher.compare(password, user.password_hash);
	if(passwordValid){

		let expiryDate = new Date(Number(new Date()) + 604800000);  
        res.cookie('appuserID', req.body.email, { expires: expiryDate, httpOnly: true });

        req.session.user = user;
        res.redirect('/');

	} else {
		res.render('login', {
			linkActive: 'login',
			user: req.session.user,
			error: "Incorrect username or password."
		})
	}
})

module.exports = router;