const express = require('express');
const router = express.Router();
const TrgovinaDataAccess = require('../data_access/TrgovinaDataAccess')
const UserDataAccess = require('../data_access/UserDataAccess')
const upload = require('express-fileupload')
var fs = require('fs');
const { stringify } = require('querystring');
const Trgovina = require('../models/TrgovinaModel');
router.get('/',async (req, res) => {
    
    //ako nije registriran redirectaj na login
	if(req.session.user == undefined){
        res.redirect('/login')
    }

    //ako je korisnik ili admin
    if(req.session.user.access_level == 0 || req.session.user.access_level == 2){
        res.render('userprofil', {
            linkActive: 'user',
            user: req.session.user
        });
    }

    //ako je trgovina
    if(req.session.user.access_level == 1){
        let trgovina = await TrgovinaDataAccess.getTrgovina(req.session.user.id)
        res.render('trgovinaprofil', {
            linkActive: 'user',
            user: req.session.user,
            trgovina: trgovina,
            error: ''
        });
    }
	
});

router.post('/', async(req,res) => {
    let trgovina = await TrgovinaDataAccess.getTrgovina(req.session.user.id)
    if(req.files == null) {
        res.render('trgovinaprofil', {
            linkActive: 'user',
            user: req.session.user,
            trgovina: trgovina,
            error: 'please upload a document before submitting!'
        });
        return;
    } 

    var fileName = req.files.fileName;
    console.log(fileName.name.substring(fileName.name.length - 4, fileName.name.length));

    if(fileName.name.toString().substring(fileName.name.length - 4, fileName.name.length) != ".txt") {
        res.render('trgovinaprofil', {
            linkActive: 'user',
            user: req.session.user,
            trgovina: trgovina,
            error: 'please upload a valid text document!'
        });
        return;
    }


    var str = fileName.data.toString('utf8')
    var count = 0;
    const map = new Map();
    let key, value;
    let prethodni = 0;
	
    for(let i = 0; i < str.length;i++){
        if(str.charAt(i) == ' '|| str.charAt(i) == '\n'){
            if(count == 0){
                key = str.substring(prethodni,i);
                count++;
                prethodni = i+1;
            }else{
                value = str.substring(prethodni,i);
                count = 0;
                prethodni = i+1;
                map.set(key,value);
            }
        }
    }

    map.forEach((v,k) => {
        if(TrgovinaDataAccess.checkIfItemExists(k)){
            if(TrgovinaDataAccess.checkIfItemExistsInShop(k)){
                TrgovinaDataAccess.changePriceInShop(v,k,req.session.user.id);
            }
        }
    })
    res.redirect('/');
});


router.get('/:id', async (req, res) => {
	
	if (req.params.id.charAt(0) == 'b')
	{
		return res.redirect('/item/' + req.params.id);
	}
	
    if(req.session.user == undefined){
        //ako nije registriran redirectaj na login
        res.redirect('/login')
        return
    }

    console.log(JSON.stringify(req.session.user))

    if(req.session.user.access_level !== 2 && req.session.user.id != req.params.id){
        //ako nije admin nema pristup tudem profilu
        return res.status(403).send('403 forbidden');
    }

    let user = undefined;
    try {
        user = await UserDataAccess.getById(req.params.id);
        
        if(user.id == req.session.user.id){
            res.redirect('/profile')
            return;
        }
    } catch (error) {
        return res.status(404).send('Not found');
    }
    
    if(user !== undefined){
        let trgovina = undefined;

        if(user.access_level == 1){
            trgovina = await TrgovinaDataAccess.getTrgovina(user.id)
        }

        /*atributi za koje je korisnik odredio da su privatni
        nece se poslati*/
        if(user.privacy !== undefined && !user.privacy.is_nickname_public){
            user.nickname = undefined
        }
        if(user.privacy !== undefined && !user.privacy.is_first_name_public){
            user.first_name = undefined
        }
        if(user.privacy !== undefined && !user.privacy.is_last_name_public){
            user.last_name = undefined
        }
        if(user.privacy !== undefined && !user.privacy.is_email_public){
            user.email = undefined
        }
        
        res.render('displayProfileForAdmin', {
            linkActive: 'user',
            user: req.session.user,
            displayUser: user,
            displayTrgovina: trgovina
        });

    } else {
        return res.status(404).send('Not found');
    }
    

});

module.exports = router;