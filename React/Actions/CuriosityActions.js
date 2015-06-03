/*
 * Copyright (c) 2014-2015, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * TodoActions
 */

var AppDispatcher = require('../dispatcher/AppDispatcher');
var TodoConstants = require('../constants/CuriosityConstants');

var CuriosityActions = {

  /**
   * @param  {string} text
   */
  create: function(text) {
    AppDispatcher.dispatch({
      actionType: CuriosityConstants.CURIOSITY_CREATE,
      text: text
    });
  },

  /**
   * @param  {string} id The ID of the curiosity item
   * @param  {string} text
   */
  updateText: function(id, text) {
    AppDispatcher.dispatch({
      actionType: CuriosityConstants.CURIOSITY_CREATE,
      id: id,
      text: text
    });
  },


  /**
   * Mark all curiosities as complete
   */
  toggleCompleteAll: function() {
    AppDispatcher.dispatch({
        actionType: CuriosityConstants.CURIOSITY_TOGGLE_COMPLETE_ALL
    });
  },



};

module.exports = TodoActions;
