// JavaScript source code
/** @jsx React.DOM */

var React = require('react');

module.exports = Curiosity = React.createClass({
    render: function(){
        var tweet = this.props.curiosity;
        return (
          <li className={"curiosity" + (curiosity.name ? ' name' : '')}>
          
        <blockquote>
          <cite>
            <a href={"http://curiositystore.azurewebsites.net/api/curiosityevent/GetAll" + curiosity.description}>{curiosity.datetime}</a> 
            <span className="description">@{curiosity.name}</span> 
          </cite>
          <span className="description">{curiosity.datetime}</span>
        </blockquote>
      </li>
    )
}
});