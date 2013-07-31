/*
 * clickable 0.1.9	http://jlix.net/extensions/jquery/clickable/0.1.9
 *
 * Copyright (c) 2009 Sander Aarts	(jlix.net)
 * Dual licensed under the MIT (http://jlix.net/MIT.txt)
 * and GPL (http://jlix.net/GPL.txt) licenses.
 *
 * Requires jQuery to work	(jquery.com). Tested with version 1.2.6
 *
 * 2008-12-01
 */
(function($) {
	$.fn.extend( {
		clickable: function( cpTtl ) { // v0.1.9
			//	Makes elements clickable, linking to location specified by href= attribute of first descending link (where href is not "#" and does not start with "javascript:").
			// Args.:	cpTtl:	[Boolean | optional]	Specifies whether or not to copy the title= attribute from the link to clickable element (default: true)
			//	CSS:		.jsClickable (on this)
			//				.jsClickableHover (on this:hover)
			//				.jsClickableFocus (on this, when guiding link gets focus)
			//				.jsGuide (on guiding link)
			return this
				.each( function() {
					var jClickElem = $(this);
					var jGuideLink = $("a[href]:not([href='#']):not([href^='javascript:'])", jClickElem).eq(0);
					if ( !jGuideLink ) return true;	// continue
					
					var href = jGuideLink.attr("href");
					if ( !href ) return true;	// continue
					
					$(this).data("href", href);
					if ( ( cpTtl || cpTtl == null ) && !this.title && jGuideLink[0].title ) this.title = jGuideLink[0].title;
					jClickElem
						.click( function() { window.location.href = $(this).data("href"); } )
						.hover(
							function() { $(this).addClass("jsClickableHover"); },
							function() { $(this).removeClass("jsClickableHover"); }
						)
						.addClass("jsClickable");
						
					jGuideLink
						.focus( function() { $(this).parents(".jsClickable").eq(0).addClass("jsClickableFocus"); })
						.blur( function() { $(this).parents(".jsClickable").eq(0).removeClass("jsClickableFocus"); })
						.addClass("jsGuide");
				} );
		}
	} );
} )(jQuery);<!--gen-->