var Curiosity = require('');

module.exports = function(stream, io){

  // When curiosities get sent our way ...
  stream.on('data', function(data) {

    // Construct a new curiosity object
    var curiosity = {
      Name: data['text'],
      DateTime: data[''],
      Description: data['text'],
      
    };

    // Create a new model instance with our object
    var curiosityEntry = new Curiosity(curiosity);

    // Save 'er to the database
    curiosityEntry.save(function(err) {
      if (!err) {
        // If everything is cool, socket.io emits the curiosity.
        io.emit('curiosity', curiosity);
      }
    });

  });

};