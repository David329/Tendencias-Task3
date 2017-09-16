var express=require('express');
var bodyParser=require('body-parser');
var cors = require('cors');
var app=express();

var port = process.env.PORT || 8080;

app.use(bodyParser.urlencoded({extended:true}));
app.use(bodyParser.json());

var apis=express.Router();
require('./routes')(apis);

app.use('/',apis);

const corsOptions = {
    origin: 'http://localhost:8080'
};
  
app.use(cors(corsOptions))


app.listen(port, function() {
    console.log('Our app is running on http://localhost:' + port);
});