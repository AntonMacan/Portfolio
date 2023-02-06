const express = require('express');
const router = express.Router();

const { body, validationResult } = require('express-validator');
const UserDataAccess = require('../data_access/UserDataAccess')

router.post('/',
    body('userid').notEmpty(),
    async (req, res) => {

	    const errors = validationResult(req);
    	if (!errors.isEmpty()) {
      		return res.status(400).json({ errors: errors.array() });
    	}

        if(req.session.user == undefined || req.session.user.access_level !== 2){
            return res.status(403).send("Samo admini mogu zabraniti pristup korisnicima")
        }

        if(!(await UserDataAccess.existsWithId(req.body.userid))){
            return res.status(400).send("Ne postoji korisnik s id = " + req.body.storeid)
        }

        try {
            await UserDataAccess.forbidAccess(req.body.userid)
            return res.status(200).send("Zabranili ste pristup korisniku")
        } catch (error) {
            console.log(error)
            throw error
        }


    })

module.exports = router;