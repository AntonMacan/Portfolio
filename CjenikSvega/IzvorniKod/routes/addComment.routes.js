const express = require('express');
const router = express.Router();

const { body, validationResult } = require('express-validator');
const UserDataAccess = require('../data_access/UserDataAccess')
const TrgovinaDataAccess = require('../data_access/TrgovinaDataAccess')

router.post('/',
    body('comment').notEmpty(),
    body('storeid').notEmpty(),
    async (req, res) => {

	    const errors = validationResult(req);
    	if (!errors.isEmpty()) {
      		return res.status(400).json({ errors: errors.array() });
    	}

        if(req.session.user == undefined || req.session.user.access_level !== 2){
            return res.status(403).send("Samo admini mogu dodavati komentare trgovinama")
        }

        if(!(await TrgovinaDataAccess.existsTrgovinaWithId(req.body.storeid))){
            return res.status(400).send("Ne postoji trgovina s id = " + req.body.storeid)
        }

        try {
            await TrgovinaDataAccess.addComment(req.session.user.id, req.body.storeid, req.body.comment)
            console.log("Komentar dodan.")
        } catch (error) {
            console.log(error)
            throw error
        }

        return res.redirect('/profile/' + req.body.storeid)

    })

module.exports = router;