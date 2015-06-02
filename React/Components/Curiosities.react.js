/** @jsx React.DOM */

var React = require('react');
var Curiosity = require('./Curiosity.react.js');
var Curiosity = require('./streamHandler.js');

module.exports = Curiosities = React.createClass({

  // Render our tweets
  render: function(){

    // Build list items of single curiosity components using map
    var content = this.props.curiosities.map(function(curiosity){
      return (
        <li><Curiosity name={curiosity.Name} curiosity={curiosity} /></li>
        <li><Curiosity date={curiosity.DateTime} curiosity={curiosity} /></li>
        <li><Curiosity description={curiosity.Description} curiosity={curiosity} />);</li>

      )
    });

    // Return ul filled with our mapped curiosities
    return (
      <ul className="curiosities">{content}</ul>
    )

  }

}); 