/* Hello, World! program in node.js */
console.log("BlockingCodeExample")

var fs = require("fs");

var data = fs.readFileSync('input.txt');

console.log(data.toString());
console.log("Program Ended");
