/*
 * Copyright (c) 2014, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * TodoStore
 */

var AppDispatcher = require('../dispatcher/AppDispatcher');
var EventEmitter = require('events').EventEmitter;
var TodoConstants = require('../constants/CuriosityConstants');
var assign = require('object-assign');

var CHANGE_EVENT = 'change';

var _curiosities= {};



/**
 * Update a curiosity item.
 * @param  {string} id
 * @param {object} updates An object literal containing only the data to be
 *     updated.
 */
function update(id, updates) {
    _curiosities[id] = assign({}, curiosities[id], updates);
}

/**
 * Update all of the curiosity items with the same object.
 *     the data to be updated.  Used to mark all curiosities as completed.
 * @param  {object} updates An object literal containing only the data to be
 *     updated.

 */
function updateAll(updates) {
    for (var id in _curiosities) {
        update(id, updates);
    }
}



var CuriosityStore = assign({}, EventEmitter.prototype, {

    /**
     * Tests whether all the remaining curiosity items are marked as completed.
     * @return {boolean}
     */
    areAllComplete: function () {
        for (var id in _curiosities) {
            if (!_curiosities[id].complete) {
                return false;
            }
        }
        return true;
    },

    /**
     * Get the entire collection of curiosities.
     * @return {object}
     */
    getAll: function () {
        return _curiosities;
    },

    emitChange: function () {
        this.emit(CHANGE_EVENT);
    },

    /**
     * @param {function} callback
     */
    addChangeListener: function (callback) {
        this.on(CHANGE_EVENT, callback);
    },

    /**
     * @param {function} callback
     */
    removeChangeListener: function (callback) {
        this.removeListener(CHANGE_EVENT, callback);
    }
});

// Register callback to handle all updates

AppDispatcher.register(function (action) {
    var text;

    switch (action.actionType) {
        case CuriosityConstants.CURIOSITY_CREATE:
            text = action.text.trim();
            if (text !== '') {
                create(text);
                CuriosityStore.emitChange();
            }
            break;

        case CuriosityConstants.CURIOSITY_TOGGLE_COMPLETE_ALL:
            if (CuriosityStore.areAllComplete()) {
                updateAll({ complete: false });
            } else {
                updateAll({ complete: true });
            }
            CuriosityStore.emitChange();
            break;

        case CuriosityConstants.CURIOSITY_UPDATE_TEXT:
            text = action.text.trim();
            if (text !== '') {
                update(action.id, { text: text });
                CuriosityStore.emitChange();
            }
            break;
         

        default:
            
    }
});

module.exports = CuriosityStore;