const express = require('express');
const app = express();
const fileUpload = require('express-fileupload');
const path = require('path');

//middleware for sessions and connecting to database
const pg = require('pg');
const db = require('./db');
const session = require('express-session');
const pgSession = require('connect-pg-simple')(session);

//finding enviornment variables
const enviornment = require('./enviornments/enviornment');

//routes
const homeRouter = require('./routes/home.routes.js');
const itemRouter = require('./routes/item.routes.js');
const loginRouter = require('./routes/login.routes.js');
const logoutRouter = require('./routes/logout.routes.js');
const notificationsRouter = require('./routes/notifications.routes.js');
const searchRouter = require('./routes/search.routes.js');
const signupRouter = require('./routes/signup.routes.js');
const profileRouter = require('./routes/profile.routes');
const addCommentRouter = require('./routes/addComment.routes');
const changePrivacyRouter = require('./routes/changePrivacy.routes');
const forbidAccessRouter = require('./routes/forbidAccess.routes');
const reportRouter = require('./routes/report.routes.js')

//ejs middleware
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');

//static files would be stored in 'public' folder
app.use(express.static(path.join(__dirname, 'public')));

//middleware for decoding parameters
app.use(express.urlencoded({ extended: true }));
app.use(fileUpload({
	limits: {
        fileSize: 1024 * 1024 * 20 // 20 MB
    },
    abortOnLimit: true
}));

//session middleware
app.use(session({
    store: new pgSession({
        pool: db.pool,
        tableName: 'session'
    }),
    secret: enviornment.SessionSecret,
    resave: false,
    saveUninitialized: true,
}));

const UserDataAccess = require('./data_access/UserDataAccess')
//middleware za zabranu pristupa baniranim korisnicima
app.use(async (req, res, next) => {
    if(req.session.user == undefined){
        next()
    } else {
        let banned = await UserDataAccess.isAccessForbidden(req.session.user.id)
        if(banned){
            res.status(403).send("Admin vam je zabranio pristup ovoj starnici :(")
        } else {
            next()
        }
    }
})

//defining routes
app.use('/', homeRouter);
app.use('/addComment', addCommentRouter);
app.use('/changePrivacy', changePrivacyRouter);
app.use('/forbidAccess', forbidAccessRouter);
app.use('/item', itemRouter);
app.use('/login', loginRouter);
app.use('/logout', logoutRouter);
app.use('/notifications', notificationsRouter);
app.use('/search', searchRouter);
app.use('/signup', signupRouter);
app.use('/profile', profileRouter);
app.use('/report', reportRouter);

app.listen(3000, () => console.log('Server running on port 3000'));