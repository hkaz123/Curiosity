var mongoose = require('mongoose');

// Create a new schema for our curiosity data
var schema = new mongoose.Schema({
    servername  : String
  , datetime    : String
  , description : String
});

// Create a static getCuriosities method to return curiosity data from the db
schema.statics.getCuriosities = function(callback) {


  // Query the db, using skip and limit to achieve page chunks
  Curiosity.find({},'servername datetime description' .sort({date: 'desc'}).exec(function(err,docs){

    // If everything is cool...
    if(!err) {
        curiosities = docs;  // We got curiosities
        curiosities.forEach(function (curiosity) {
            curiosities.active = true; // Set them to active
      });
    }

    // Pass them back to the specified callback
    callback(curiosities);

  });

};

// Return a Tweet model based upon the defined schema
module.exports = Curiosity = mongoose.model('Curiosity', schema);