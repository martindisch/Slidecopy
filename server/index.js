var express = require('express'),
    multer = require('multer'),
    date = require('date-and-time');

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
    res.status(200).send('File received');
});

app.listen(8080);
dateLog('Server listening on 8080');

function dateLog(text) {
    console.log(date.format(new Date(), 'YY/MM/DD HH:mm:ss') + '    ' + text);
}
