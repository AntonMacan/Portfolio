const express = require('express');
const router = express.Router();

router.get('/', (req, res) => {
	
	req.session.user = undefined

	req.session.destroy((err) => {
    
		if(err) {
		  console.log(err)
		}
		else {
		  res.redirect('/')
		}
	})      
});

module.exports = router;