var express = require('express'),
    multer = require('multer'),
    date = require('date-and-time'),
    pers = require('node-persist');
    
pers.init();

// define upload destination and filename
var storage = multer.diskStorage({
    destination: 'upload',
    filename: function(req, file, cb) {
        cb(null, file.originalname)
    }
});
var upload = multer({ storage: storage });
    
var app = express();

// handle file upload
app.post('/upload', upload.single('file'), function(req, res) {
    dateLog('Received file ' + req.file.originalname);
    res.sendStatus(200);
});

// handle code requests
app.get('/code', function(req, res) {
    // try to load current code from storage
    pers.getItem('currentCode', function(err, value) {
        // return 1 if no values have been read
        if (err || value == undefined) value = 1;
        // save new current value
        pers.setItem('currentCode', value + 1);
        res.send(value.toString());
        dateLog('Returned code ' + value);
    });
});

// handle download requests
app.get('/photo/:filename', function(req, res) {
    var filename = req.params.filename;
    res.download('upload/' + filename, function(err) {
        if (err) {
            res.sendStatus(404);
            dateLog('Requested image ' + filename + ' not found');
        } else {
            dateLog('Served ' + filename);
        }
    });
});

app.listen(8080);
dateLog('Server listening on 8080');

/**
 * Prints the given text next to the system's current date and time to console.
 * 
 * @param text the text to print to console
 */
function dateLog(text) {
    console.log(date.format(new Date(), 'YY/MM/DD HH:mm:ss') + '    ' + text);
}
