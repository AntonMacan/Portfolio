const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	
	res.render('notifications', {
		user: req.session.user,
		linkActive: 'notifications'
	});
});

module.exports = router;