// JavaScript source code
var JSX = require('node-jsx').install(),
  React = require('react'),
  CuriositiesApp = require('./components/CuriositiesApp.react'),
  Curiosity = require('./models/Curiosity');

module.exports = {

    index: function (req, res) {
        // Call static model method to get curiosities in the db
        Curiosity.getCuriosities(0, 0, function (curiosities, pages) {

            // Render React to a string, passing in our fetched tweets
            var markup = React.renderComponentToString(
              CuriositiesApp({
                  curiosities: curiosities
              })
            );

            // Render our 'home' template
            res.render('home', {
                markup: markup, // Pass rendered react markup
                state: JSON.stringify(curiosities) // Pass current state to client side
            });

        });
    },

    page: function (req, res) {
        // Fetch curiosities by page via param
        Curiosity.getCuriosities(req.params.page, req.params.skip, function (curiosities) {

            // Render as JSON
            res.send(curiosities);

        });
    }

}