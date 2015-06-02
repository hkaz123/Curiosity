/** @jsx React.DOM */

var React = require('react');
var Tweet = require('./Curiosity.react.js');

module.exports = Tweets = React.createClass({

  // Render our tweets
  render: function(){

    // Build list items of single tweet components using map
    var content = this.props.curiosities.map(function(curiosity){
      return (
        <Curiosity key={curiosity._id} curiosity={curiosity} />
      )
    });

    // Return ul filled with our mapped curiosities
    return (
      <ul className="curiosities">{content}</ul>
    )

  }

}); 