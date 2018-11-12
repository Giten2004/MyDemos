
/*
 * GET home page.
 */

exports.index = function (req, res) {
    res.render('index', { title: 'Express', year: new Date().getFullYear() });
};

exports.about = function (req, res) {
    res.render('about', { title: 'About', year: new Date().getFullYear(), message: 'Your application description page' });
};

exports.contact = function (req, res) {
    res.render('contact', { title: 'Contact', year: new Date().getFullYear(), message: 'Your contact page' });
};

exports.hft = function (req, res) {
    res.render('hft', { title: 'HFT', year: new Date().getFullYear(), message: 'High Frequency Trading' });
};

exports.gaoxin = function (req, res) {
    res.render('gaoxin', { title: '高薪', year: new Date().getFullYear(), message: '架构师' });
};