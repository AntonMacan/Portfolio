const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	
	res.render('user', {
		linkActive: 'user',
		user: req.session.user
	});
});

module.exports = router;