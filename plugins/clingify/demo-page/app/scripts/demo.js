$(function() {
	'use strict';

	var $firstTarget = $('.nav-persistent'),
		$secondTarget = $('.second-clingy'),
		$fluidParent = $('.content'),
		matchWidths = function($elem) {
			$elem.width($fluidParent.width());
		};

	$firstTarget.clingify({
		extraClass : 'primaryClingifyElement',
		locked : function() {
			matchWidths($firstTarget);
		},
		resized : function() {
			matchWidths($firstTarget);
		}
	});
	$secondTarget.clingify({
		extraClass : 'secondaryClingifyElement',
		locked : function() {
			matchWidths($secondTarget);
		},
		resized : function() {
			matchWidths($secondTarget);
		}
	});
});