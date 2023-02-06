const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	
	res.render('home', {
		linkActive: 'home',
		user: req.session.user
	});
});

router.post('/', (req, res) => {
	let url = '/search/' + req.body.item.toLowerCase();
	res.redirect(url);
})

module.exports = router;