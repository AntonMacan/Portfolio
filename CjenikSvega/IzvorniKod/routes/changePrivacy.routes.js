const express = require('express');
const router = express.Router();
const { body, validationResult } = require('express-validator');
const UserDataAccess = require('../data_access/UserDataAccess')


router.post('/',
    body('attribute').notEmpty(),
    async (req, res) => {

        const errors = validationResult(req);
    	if (!errors.isEmpty()) {
      		return res.status(400).json({ errors: errors.array() });
    	}

        if(req.session.user == undefined){
            return res.status(400).json('You have to login first');
        }
        
        if(req.body.attribute == 'nickname'){

            let public = req.body.nickname !== undefined
            await UserDataAccess.updatePrivacy(req.session.user.id, 'nickname', public)

        } else if(req.body.attribute == 'first_name'){

            let public = req.body.first_name !== undefined
            await UserDataAccess.updatePrivacy(req.session.user.id, 'first_name', public)

        } else if(req.body.attribute == 'last_name'){

            let public = req.body.last_name !== undefined
            await UserDataAccess.updatePrivacy(req.session.user.id, 'last_name', public)
            
        } else if(req.body.attribute == 'email'){
            let public = req.body.email !== undefined
            await UserDataAccess.updatePrivacy(req.session.user.id, 'email', public)
        }

        req.session.user.privacy = await UserDataAccess.getPrivacyForUser(req.session.user.id)

        return res.redirect('/profile')
    })

module.exports = router;