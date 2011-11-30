function isSlider(sTag, cTag, settings)
{
	var s_tag  = sTag;
	var c_tag  = cTag;
	var slides = jQuery('#' + s_tag + ' .slides');
	
	if (!settings) settings = {};
	
	var s_width  = settings.width    ? settings.width    : 0;
	var s_height = settings.height   ? settings.height   : 0;
	var interval = settings.interval ? settings.interval : 5000;
	var duration = settings.duration ? settings.duration : 1000;
	var easing   = settings.easing   ? settings.easing   : 'easeInSine';
	var s_numb   = 0;
	var last_pos = 0;
	
	var timeoutID = null;
	
	this.initialize = function()
	{
		var slides = jQuery('#' + s_tag + ' .slides');
		
		s_numb = slides.find('.slide').size();
		
		if (s_numb > 1)
		{
			slides.append('<div class="slide">' + slides.find('.slide:first').html() + '</div>');
		}
		else s_numb = 1;
		
		last_pos = s_width * s_numb;
		
		slides.css('width', s_width * (s_numb + 1) + 'px')
			.css('height', s_height + 'px')
			.css('overflow', 'hidden')
			.css('position', 'relative')
			.css('left', 0)
			.css('padding', 0)
		;
		
		slides.find('.slide')
			.css('width', s_width + 'px')
			.css('height', s_height + 'px')
			.css('float', 'left')
			.css('overflow', 'hidden')
			.css('margin', 0)
			.css('padding', 0)
			.css('border', '0 none')
		;
		
		var displaySlide = this.displaySlide;
		
		jQuery('#' + c_tag + ' .control').each(function(index)
		{
			if (index == 0)
			{
				jQuery(this).addClass('current');
			}
			else
			{
				jQuery(this).removeClass('current');
			}
			
			jQuery(this).attr('slide', index).click(function(event) {
				event = event || window.event;
				var node = event.target || event.srcElement;
				
				if (!jQuery(node).hasClass('control')) node = jQuery(node).parents('.control').get(0);
				
				displaySlide(node.getAttribute('slide'));
			})
		});
		
		__setTimeout();
	};
	
	this.displaySlide = function(number)
	{
		__clearTimeout();
		
		var nxt = jQuery('#' + c_tag + ' .control[slide="' + number + '"]');
		
		if (nxt.size() == 0)
		{
			__setTimeout();
			
			return;
		}
		
		var cur = jQuery('#' + c_tag + ' .current');
		var cur_numb = cur.attr('slide');
		
		if (cur_numb == number)
		{
			__setTimeout();
			
			return;
		}
		
		nxt.addClass('current');
		cur.removeClass('current');
		
		slides.animate({left: '-' + number*s_width + 'px'}, duration, easing, function() {});
		
		__setTimeout();
	};
	
	function next()
	{
		if (last_pos == 0) return;
		
		var _lft = Math.abs(parseInt(slides.css('left'), 10));
		
		if (_lft >= last_pos)
		{
			slides.css('left', '0px');
			
			_lft = s_width;
		}
		else _lft += s_width;
		
		var cur = jQuery('#' + c_tag + ' .current');
		var nxt = jQuery('#' + c_tag + ' .current').next();
		
		if (nxt.size() == 0)
		{
			nxt = jQuery('#' + c_tag + ' .control:first');
		}
		
		nxt.addClass('current');
		cur.removeClass('current');
		
		slides.animate({left: '-' + _lft + 'px'}, duration, easing, function() {
			if (_lft >= last_pos) slides.css('left', '0px');
		});
		
		__setTimeout();
	}
	
	function __setTimeout()
	{
		timeoutID = setTimeout(next, interval);
	}
	
	function __clearTimeout()
	{
		clearTimeout(timeoutID);
	}
}
