/********************************************************
 *
 * Custom Javascript code for AppStrap Bootstrap theme
 * Written by Themelize.me (http://themelize.me)
 *
 *******************************************************/
/*global jRespond */
$(document).ready(function() {
  "use strict";
  
  
  //colour switch - demo only
  // --------------------------------
  var defaultColour = 'green';
  $('.colour-switcher a').click(function() {
    var c = $(this).attr('href').replace('#','');
    var cacheBuster = 3 * Math.floor(Math.random() * 6);
    $('.colour-switcher a').removeClass('active');
    $('.colour-switcher a.'+ c).addClass('active');
    
    if (c !== defaultColour) {
      $('#colour-scheme').attr('href','css/colour-'+ c +'.css?x='+ cacheBuster);
    }
    else {
      $('#colour-scheme').attr('href', '#');
    }
  });
  
  //IE placeholders
  // --------------------------------
  $('[placeholder]').focus(function() {
    var input = $(this);
    if (input.val() === input.attr('placeholder')) {
      if (this.originalType) {
        this.type = this.originalType;
        delete this.originalType;
      }
      input.val('');
      input.removeClass('placeholder');
    }
  }).blur(function() {
    var input = $(this);
    if (input.val() === '') {
      input.addClass('placeholder');
      input.val(input.attr('placeholder'));
    }
  }).blur();
  
  // Detect Bootstrap fixed header
  // @see: http://getbootstrap.com/components/#navbar-fixed-top
  // --------------------------------
  if ($('.navbar-fixed-top').size() > 0) {
    $('html').addClass('has-navbar-fixed-top');
  }
  
  // Bootstrap tooltip
  // @see: http://getbootstrap.com/javascript/#tooltips
  // --------------------------------
  // invoke by adding data-toggle="tooltip" to a tags (this makes it validate)
  if(jQuery().tooltip) {
    $('body').tooltip({
      selector: "[data-toggle=tooltip]",
      container: "body"
    });
  }
    
  // Bootstrap popover
  // @see: http://getbootstrap.com/javascript/#popovers
  // --------------------------------
  // invoke by adding data-toggle="popover" to a tags (this makes it validate)
  if(jQuery().popover) {
    $('body').popover({
      selector: "[data-toggle=popover]",
      container: "body",
      trigger: "hover"
    });
  }

  //allow any page element to set page class
  // --------------------------------  
  $('[data-page-class]').each(function() {
    $('html').addClass($(this).data('page-class'));
  });
  
  //show hide for hidden header
  // --------------------------------
  $('[data-toggle=show-hide]').each(function() {
    $(this).click(function() {
      var state = 'open'; //assume target is closed & needs opening
      var target = $(this).attr('data-target');
      var targetState = $(this).attr('data-target-state');
      
      //allows trigger link to say target is open & should be closed
      if (typeof targetState !== 'undefined' && targetState !== false) {
        state = targetState;
      }
      
      if (state === 'undefined') {
        state = 'open';
      }
      
      $(target).toggleClass('show-hide-'+ state);
      $(this).toggleClass(state);
    });
  });

  //Plugin: jPanel Menu
  // data-toggle=jpanel-menu must be present on .navbar-btn
  // @todo - allow options to be passed via data- attr
  // --------------------------------
  if($.jPanelMenu && $('[data-toggle=jpanel-menu]').size() > 0) {
    var jpanelMenuTrigger = $('[data-toggle=jpanel-menu]');

    var jPM = $.jPanelMenu({
      menu: jpanelMenuTrigger.data('target'),
      direction: 'left',
      trigger: '.'+ jpanelMenuTrigger.attr('class'),
      excludedPanelContent: '.jpanel-menu-exclude',
      openPosition: '280px',
      afterOpen: function() {
        jpanelMenuTrigger.addClass('open');
        $('html').addClass('jpanel-menu-open');
      },
      afterClose: function() {
        jpanelMenuTrigger.removeClass('open');
        $('html').removeClass('jpanel-menu-open');
      }
    });
  
    //jRespond settings
    var jRes = jRespond([
      {
        label: 'small',
        enter: 0,
        exit: 1010
      }
    ]);
    
    //turn jPanel Menu on/off as needed
    jRes.addFunc({
        breakpoint: 'small',
        enter: function() {
          jPM.on();
        },
        exit: function() {
          jPM.off();
        }
    });
  }
  
  /*
  //Plugin: clingify (sticky navbar)
  // --------------------------------
  if (jQuery().clingify) {
    $('[data-toggle=clingify]').clingify({
      breakpoint: 1010,
    });
  }*/
  
  //Plugin: flexslider
  // --------------------------------
  $('.flexslider').each(function() {
    var sliderSettings =  {
      animation: $(this).attr('data-transition'),
      selector: ".slides > .slide",
      controlNav: true,
      smoothHeight: true,
      start: function(slider) {
        //hide all animated elements
        slider.find('[data-animate-in]').each(function() {
          $(this).css('visibility','hidden');
        });
        
        //animate in first slide
        slider.find('.slide').eq(1).find('[data-animate-in]').each(function() {
          $(this).css('visibility','hidden');
          if ($(this).data('animate-delay')) {
            $(this).addClass($(this).data('animate-delay'));
          }
          if ($(this).data('animate-duration')) {
            $(this).addClass($(this).data('animate-duration'));
          }
          $(this).css('visibility','visible').addClass('animated').addClass($(this).data('animate-in'));
          $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
            function() {
              $(this).removeClass($(this).data('animate-in'));
            }
          );
        });
      },
      before: function(slider) {
        //hide next animate element so it can animate in
        slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function() {
          $(this).css('visibility','hidden');
        });
      },
      after: function(slider) {
        //hide animtaed elements so they can animate in again
        slider.find('.slide').find('[data-animate-in]').each(function() {
          $(this).css('visibility','hidden');
        });
        
        //animate in next slide
        slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function() {
          if ($(this).data('animate-delay')) {
            $(this).addClass($(this).data('animate-delay'));
          }
          if ($(this).data('animate-duration')) {
            $(this).addClass($(this).data('animate-duration'));
          }
          $(this).css('visibility','visible').addClass('animated').addClass($(this).data('animate-in'));
          $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
            function() {
              $(this).removeClass($(this).data('animate-in'));
            }
          );
        });
      }
    };
    
    var sliderNav = $(this).attr('data-slidernav');
    if (sliderNav !== 'auto') {
      sliderSettings = $.extend({}, sliderSettings, {
        manualControls: sliderNav +' li a',
        controlsContainer: '.flexslider-wrapper'
      });
    }
    
    $('html').addClass('has-flexslider');
    $(this).flexslider(sliderSettings);
  });
  $('.flexslider').resize(); //make sure height is right load assets loaded
  
  //Plugin: jQuery Quicksand plugin
  //@based on: http://www.evoluted.net/thinktank/web-development/jquery-quicksand-tutorial-filtering
  // --------------------------------
  $('[data-js=quicksand]').each(function() {
    var quicksandTrigger = $(this).find($(this).data('quicksand-trigger'));
    var quicksandTarget = $($(this).data('quicksand-target'));
    var quicksandTargetData = quicksandTarget.clone();
    var filterId = 'all';
    var filteredData;
    
    quicksandTrigger.click(function(e) {
      filterId = $(this).data('quicksand-fid');
      filteredData = '';
      quicksandTrigger.parents('li').removeClass('active');
      $(this).parents('li').addClass('active');
      
      if (filterId === 'all') {
        filteredData = quicksandTargetData.find('[data-quicksand-id]');
      }
      else {
        filteredData = quicksandTargetData.find('[data-quicksand-tid="'+ filterId +'"]');
      }
      
      quicksandTarget.quicksand(filteredData,
        {
          duration: 600,
          attribute: 'data-quicksand-id',
          adjustWidth: 'auto',
        }
      ).addClass('quicksand-target');
      e.preventDefault();
    });
  });
  
});