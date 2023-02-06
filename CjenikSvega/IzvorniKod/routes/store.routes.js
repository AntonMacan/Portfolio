const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	
	res.render('store', {
		linkActive: 'store'
	});
});

module.exports = router;