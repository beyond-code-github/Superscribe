/*
 * Clingify v1.0.1
 *
 * A jQuery 1.7+ plugin for sticky elements
 * http://github.com/theroux/clingify
 *
 * MIT License
 *
 * By Andrew Theroux
 */
// ';' protects against concatenated scripts which may not be closed properly.
;(function($, window, document, undefined) {
    'use strict';

    // defaults
    var pluginName = 'clingify',
        defaults = {
            breakpoint: 0,
            // Media query width breakpoint in pixels. 
            // Below this value, Clingify behavior is disabled. (Useful for small screens.)
            // Use 0 if you want Clingify to work at all screen widths.

            extraClass: '',
            // Add an additional class of your choosing to the sticky element 
            // and its parent wrapper & placeholder divs

            throttle: 100,
            // Delay Clingify functions, in milliseconds, when scrolling/resizing.
            // Too fast is bad for performance, especially on older browsers/machines.

            // Callback functions
            detached: $.noop, // Fires before element is detached
            locked: $.noop, // Fires before element is attached
            resized: $.noop // Fires after window resize event, benefits from the throttle
        },
        wrapperClass = 'js-clingify-wrapper',
        lockedClass = 'js-clingify-locked',
        placeholderClass = 'js-clingify-placeholder',
        $buildPlaceholder = $('<div>').addClass(placeholderClass),
        $buildWrapper = $('<div>').addClass(wrapperClass),
        $window = $(window);

    // plugin constructor
    function Plugin(element, options) {
        this.element = element; // The element is the thing you passed to clingify()

        // turn our Clingify element into jQuery object
        this.$element = $(element);

        // Overwrites defaults with options
        this.options = $.extend({}, defaults, options);

        this._defaults = defaults;
        this._name = pluginName;

        this.vars = {
            elemHeight: this.$element.height()
        };
        this.init();
    }

    Plugin.prototype = {

        init: function() {
            var cling = this,
                scrollTimeout,
                throttle = cling.options.throttle,
                extraClass = cling.options.extraClass;

            // Give Clingify element two wrapper divs.
            // Placeholder div is set to same height as element
            // This ensures content beneath element does not re-flow.
            // Wrapper div is 100% width, which eases styling/centering/positioning
            cling.$element
                .wrap($buildPlaceholder.height(cling.vars.elemHeight))
                .wrap($buildWrapper);

            if ((extraClass !== '') && (typeof extraClass === 'string')) {
                cling.findWrapper().addClass(extraClass);
                cling.findPlaceholder().addClass(extraClass);
            }


            $window.on('scroll resize', function(event) {
                if (!scrollTimeout) {
                    scrollTimeout = setTimeout(function() {
                        if ((event.type === 'resize') && (typeof cling.options.resized === 'function')) {
                            cling.options.resized();
                        }
                        cling.checkElemStatus();
                        scrollTimeout = null;
                    }, throttle);
                }
            });
        },

        //Other functions below
        checkCoords: function() {
            var coords = {
                windowWidth: $window.width(),
                windowOffset: $window.scrollTop(),
                // Y-position for Clingify placeholder
                // needs to be recalculated in DOM has shifted
                placeholderOffset: this.findPlaceholder().offset().top
            };
            return coords;
        },

        detachElem: function() {
            if (typeof this.options.detached === 'function') {
                this.options.detached(); // fire callback
            }
            this.findWrapper().removeClass(lockedClass);
        },

        lockElem: function() {
            if (typeof this.options.locked === 'function') {
                this.options.locked(); // fire callback
            }
            this.findWrapper().addClass(lockedClass);
        },

        findPlaceholder: function() {
            return this.$element.closest('.'+placeholderClass);
        },

        findWrapper: function() {
            return this.$element.closest('.'+wrapperClass);
        },

        checkElemStatus: function() {
            var cling = this,
                currentCoords = cling.checkCoords(),
                isScrolledPast = function() {
                    if (currentCoords.windowOffset >= currentCoords.placeholderOffset) {
                        return true;
                    } else {
                        return false;
                    }
                },
                isWideEnough = function() {
                    if (currentCoords.windowWidth >= cling.options.breakpoint) {
                        return true;
                    } else {
                        return false;
                    }
                };

            if (isScrolledPast() && isWideEnough()) {
                cling.lockElem();
            } else if (!isScrolledPast() || !isWideEnough()) {
                cling.detachElem();
            }
        }
    };

    // wrapper that prevents multiple instantiations
    $.fn[pluginName] = function(options) {
        return this.each(function() {
            if (!$.data(this, 'plugin_' + pluginName)) {
                $.data(this, 'plugin_' + pluginName, new Plugin(this, options));
            }
        });
    };

})(jQuery, window, document);