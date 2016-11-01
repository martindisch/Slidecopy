var express = require('express'),
    multer = require('multer'),
    date = require('date-and-time'),
    pers = require('node-persist');
    
pers.init();

var storage = multer.diskStorage({
    destination: 'upload',
    filename: function(req, file, cb) {
        cb(null, file.originalname)
    }
});
var upload = multer({ storage: storage });
    
var app = express();

app.post('/upload', upload.single('file'), function(req, res) {
    dateLog('Received file ' + req.file.originalname);
    res.send('File received');
});

app.get('/code', function(req, res) {
    pers.getItem('currentCode', function(err, value) {
        if (err || value == undefined) value = 1;
        pers.setItem('currentCode', value + 1);
        res.send(value.toString());
        dateLog('Returned code ' + value);
    });
});

/*app.get('/photo/:filename', function(req, res) {
    var filename = req.params.filename;
    res.download('upload/' + filename, function(err) {
        // TODO: response for error
        dateLog('served ' + filename);
    });
});*/
app.use('/photo', express.static('upload'));

app.listen(8080);
dateLog('Server listening on 8080');

function dateLog(text) {
    console.log(date.format(new Date(), 'YY/MM/DD HH:mm:ss') + '    ' + text);
}
