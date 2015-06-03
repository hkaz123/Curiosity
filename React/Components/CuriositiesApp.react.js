/** @jsx React.DOM */

var React = require('react');
var Curiosities = require('./Curiosities.react.js');
var Loader = require('./Loader.react.js');
var NotificationBar = require('./NotificationBar.react.js');

// Export the CuriositiesApp component
module.exports = CuriositiesApp = React.createClass({

    // Method to add a curiosity to our timeline
    addCuriosity: function(curiosity){

        // Get current application state
        var updated = this.state.curiosities;

        // Add curiosity to the beginning of the curiosities array
        updated.unshift(curiosity);

        // Set application state
        this.setState({curiosities: updated, count: count});

    },

    // Method to get JSON from server by page
    getPage: function(page){

        // Setup our ajax request
        var request = new XMLHttpRequest(), self = this;
        request.open("GET", "http://curiositystore.azurewebsites.net/api/curiosityevent/GetAll", true);
        request.onload = function() {

            // If everything is cool...
            if (request.status >= 200 && request.status < 400){

                // Load our next curiosity
                self.loadPagedCuriosities(JSON.parse(request.responseText));

            } else {

                // Set application state (Not paging, paging complete)
                self.setState({paging: false, done: true});

            }
        };

        // Fire!
        request.send();

    },
    
    // Method to show the unread curiosities
    showNewCuriosities: function(){

        // Get current application state
        var updated = this.state.Curiosities;

        // Mark our curiosities active
        updated.forEach(function(curiosity){
            curiosity.active = true;
        });

        // Set application state (active curiosities + reset unread count)
        this.setState({curiosities: updated, count: 0});

    },

    // Method to load curiosities fetched from the server
    loadPagedCuriosities: function(curiosities){

        
        var self = this;

        // If we still have curiosities...
        if(curiosities.length > 0) {

            // Get current application state
            var updated = this.state.curiosities;

            // Push them onto the end of the current curiosities array
            curiosities.forEach(function(curiosity){
                updated.push(curiosity);
            });

            
            setTimeout(function(){

                // Set application state (Not paging, add curiosities)
                self.setState({curiosities: updated, paging: false});

            }, 1000);

        } else {

            // Set application state (Not paging, paging complete)
            this.setState({done: true, paging: false});

        }
    },

    // Method to check if more curiosities should be loaded, by scroll position
    checkWindowScroll: function(){

        // Get scroll pos & window data
        var h = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
        var s = (document.body.scrollTop || document.documentElement.scrollTop || 0);
        var scrolled = (h + s) > document.body.offsetHeight;

        // If scrolled enough, not currently paging and not complete...
        if(scrolled && !this.state.paging && !this.state.done) {

            // Set application state (Paging, Increment page)
            this.setState({paging: true, page: this.state.page + 1});

            // Get the next page of curiosities from the server
            this.getPage(this.state.page);

        }
    },

    // Set the initial component state
    getInitialState: function(props){

        props = props || this.props;

        // Set initial application state using props
        return {
            curiosities: props.curiosities,
            count: 0,
            page: 0,
            paging: false,            
            done: false
        };

    },

    componentWillReceiveProps: function(newProps, oldProps){
        this.setState(this.getInitialState(newProps));
    },

    // Called directly after component rendering, only on client
    componentDidMount: function(){

      
        var self = this;

        // Initialize socket.io
        var socket = io.connect();

        // On curiosity event emission...
        socket.on('curiosity', function (data) {

            // Add a curiosity to our queue
            self.addCuriosity(data);

        });

        // Attach scroll event to the window for infinity paging
        window.addEventListener('scroll', this.checkWindowScroll);

    },

    // Render the component
    render: function(){

        return (
          <div className="curiosities-app">
            <Curiosities curiosities={this.state.curiosities} />
            <Loader paging={this.state.paging}/>
            <NotificationBar count={this.state.count} onShowNewCuriosities={this.showNewCuriosities}/>
          </div>
      )

  }

});