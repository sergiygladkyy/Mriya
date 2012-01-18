/**
 * Gallery constructor
 *
 * @param string slider_jquery_selector
 * @param string preview_jquery_selector
 * @param object settings - gallery settings
 *
 * @return void
 */
function mriyaGallery(slider_jquery_selector, preview_jquery_selector, settings)
{
    var players_dir = settings.players_dir ? settings.players_dir : '/players/';
	
	var _use_share_point_silver_light = settings.use_share_point_silver_light ? true : false;
	
	var s_selector = slider_jquery_selector  ? slider_jquery_selector  : '.gallery_slider';
	var p_selector = preview_jquery_selector ? preview_jquery_selector : '.gallery_preview';
	
	var data  = settings.data ? settings.data : {};
	var s_set = settings.slider ? settings.slider : {};
	var p_set = settings.preview ? settings.preview : {};
	
	var blanket_id = 'pop_up_blanket';
	var pop_up_id  = 'pop_up_win';
	var opened     = false;
	var options    = {
		show_title: true,
		show_desc:  true
	};
	
	var pop_up_padding = 15;
	
	/* Check slider options */
	
	var w_img = s_set.img_width         ? s_set.img_width      : 80;
	var h_img = s_set.img_height        ? s_set.img_height     : 60;
	var n_max = s_set.max_images        ? s_set.max_images     : 20;
	var n_vis    = s_set.visible_images ? s_set.visible_images : 5;
	var w_slider = s_set.slider_width   ? s_set.slider_width   : 0;
	var duration = s_set.duration       ? s_set.duration       : 400;
	var easing   = s_set.easing         ? s_set.easing         : 'easeOutSine';
	
	if (s_set.item_border)
	{
		var item_border = {
			top:    s_set.item_border.top    ? s_set.item_border.top    : 0,
			right:  s_set.item_border.right  ? s_set.item_border.right  : 0,
			bottom: s_set.item_border.bottom ? s_set.item_border.bottom : 0,
			left:   s_set.item_border.left   ? s_set.item_border.left   : 0
		};
	}
	else var item_border = {top: 0, right: 0, bottom: 0, left: 0};
	
	if (s_set.item_padding)
	{
		var item_padding = {
			top:    s_set.item_padding.top    ? s_set.item_padding.top    : 0,
			right:  s_set.item_padding.right  ? s_set.item_padding.right  : 0,
			bottom: s_set.item_padding.bottom ? s_set.item_padding.bottom : 0,
			left:   s_set.item_padding.left   ? s_set.item_padding.left   : 0
		};
	}
	else var item_padding = {top: 2, right: 2, bottom: 2, left: 2};
	
	var item_spacing = {
		horisontal: item_border.left + item_border.right + item_padding.left + item_padding.right,
		vertical:   item_border.top + item_border.bottom + item_padding.top + item_padding.bottom,
	};
	
	var w_item = w_img + item_spacing.horisontal;
	var h_item = h_img + item_spacing.vertical;
	var w_max  = n_vis * w_item;
	var h_max  = h_item;
	
	var _w_lft = parseInt(jQuery(s_selector).find('.prev_btn_container').css('width'), 10);
	var _w_rgt = parseInt(jQuery(s_selector).find('.next_btn_container').css('width'), 10);
	
	
	/* Check preview options */
	
	var w_preview = p_set.width     ? p_set.width      : 400;
	var h_preview = p_set.height    ? p_set.height     : 300;
	var auto_play = p_set.auto_play ? p_set.auto_play  : false;
	
	if (p_set.border)
	{
		var border = {
			top:    p_set.border.top    ? p_set.border.top    : 0,
			right:  p_set.border.right  ? p_set.border.right  : 0,
			bottom: p_set.border.bottom ? p_set.border.bottom : 0,
			left:   p_set.border.left   ? p_set.border.left   : 0
		};
	}
	else var border = {top: 0, right: 0, bottom: 0, left: 0};
	
	if (p_set.padding)
	{
		var padding = {
			top:    p_set.padding.top    ? p_set.padding.top    : 0,
			right:  p_set.padding.right  ? p_set.padding.right  : 0,
			bottom: p_set.padding.bottom ? p_set.padding.bottom : 0,
			left:   p_set.padding.left   ? p_set.padding.left   : 0
		};
	}
	else var padding = {top: 0, right: 0, bottom: 0, left: 0};
	
	
	/**
	 * Initialize gallery
	 *
	 * @return void
	 */
	this.initialize = function()
	{
		/* Initialize slider */
		
		var slider_width = w_max + _w_lft + _w_rgt;
		var cont_width   = w_max;
		
		if (w_slider > 0)
		{
			cont_width  += w_slider - slider_width;
			slider_width = w_slider;
		}
		
		jQuery(s_selector).css('width', slider_width).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0').
			find('.container').css('width', cont_width).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0').
			find('.slides').css('width', w_max).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0 auto').
			find('table').css('width', w_max).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0 auto').css('borderCollapse', 'collapse').css('position', 'relative');
		
		jQuery(s_selector).find('.zoom').each(function(index) {
			if (index == 0)
			{
				jQuery(this).addClass('first_visible');
			}
			
			if (index == (n_vis - 1))
			{
				jQuery(this).addClass('last_visible');
			}
			
			if (index >= n_vis)
			{
				jQuery(this).css('display', 'none');
			}
		});
		
		jQuery(s_selector).find('.item').css('width', w_img).css('height', h_img).
			css('borderWidth', item_border.top + 'px ' + item_border.right + 'px ' + item_border.bottom + 'px ' + item_border.left + 'px').
			css('padding',  item_padding.top + 'px ' + item_padding.right + 'px ' + item_padding.bottom + 'px ' + item_padding.left + 'px').
			css('margin', '0px');
		
		
		if (jQuery(s_selector).find('.first_visible').hasClass('first'))
		{
			jQuery(s_selector).find('.prev_btn').css('cursor', 'default');
		}
		
		if (jQuery(s_selector).find('.last_visible').hasClass('last'))
		{
			jQuery(s_selector).find('.next_btn').css('cursor', 'default');
		}
		
		
		/* Initialize preview */
		
		initializePreview.call(this);
		
		
		/* Events */
		
		jQuery(s_selector).find('.prev_btn').click(function(event) {
			prevItem(event);
		});
		
		jQuery(s_selector).find('.next_btn').click(function(event) {
			nextItem(event);
		});
		
		var onClick = this.onClick;
		
		jQuery(s_selector).find('.item a').click(function(event) {
			event = event || window.event;
			var node = event.target || event.srcElement;
			
			if (!jQuery(node).hasClass('item')) node = jQuery(node).parents('.item').get(0);
			
			onClick(node);
		});
	};
	
	/**
	 * Initialize preview block
	 *
	 * @return void
	 */
	function initializePreview()
	{
		jQuery(p_selector).css('width', w_preview).css('height', h_preview).
			css('borderWidth', border.top + 'px ' + border.right + 'px ' + border.bottom + 'px ' + border.left + 'px').
			css('padding', padding.top + 'px ' + padding.right + 'px ' + padding.bottom + 'px ' + padding.left + 'px').
			css('margin', '0px');
		
		generatePopUpHtml();
		
		if (jQuery(s_selector).find('.item.current').size() == 0)
		{
			var node = jQuery(s_selector).find('.item:first').get(0);
			
			if (node) this.onClick(node);
		}
		else
		{
			previewEvents();
		}
	}
	
	/**
	 * Add preview events listeneres
	 *
	 * @return void
	 */
	function previewEvents()
	{
		var current = jQuery(s_selector).find('.item.current');
		var node_id = current.attr('node');
		
		if (data[node_id] && !data[node_id].video && data[node_id].b_img)
		{
			jQuery(p_selector).find('img.preview').css('cursor', 'pointer').click(function(event) {
				event = event || window.event;
				var node = event.target || event.srcElement;
				
				onShowFullImage(node);
			});
		}
	}
		
	/**
	 * Process item onClick event
	 *
	 * @param object node - DOMElements
	 *
	 * @return void
	 */
	this.onClick = function(node)
	{
		var html    = '';
		var node_id = node.getAttribute('node');
		
		jQuery(s_selector).find('.item.current').removeClass('current');
		jQuery(node).addClass('current');
		
		if (!data[node_id] || !(data[node_id].video || data[node_id].b_img))
		{
			html = '<img src="' + jQuery(node).find('a img').attr('src') + '" alt="" />';
		}
		else if (data[node_id].video)
		{
			html = generateHTMLForVideo(node, data[node_id]);
		}
		else
		{
			html = '<img class="preview" src="' + data[node_id].b_img + '" alt="" />';
		}
		
		jQuery(p_selector).html(html);
		
		previewEvents();
	};
	
	
	
	
	
	/**
	 * Move left
	 *
	 * @param object event - DOMEvents object
	 *
	 * @return void
	 */
	function prevItem(event)
	{
		var slider = jQuery(s_selector);
		var first  = slider.find('.first_visible');
		var node   = first.prev();
		
		if (node.size() == 0) return;
		
		var last = jQuery(s_selector).find('.last_visible');
		
		jQuery(s_selector).find('.prev_btn').css('cursor', node.hasClass('first') ? 'default' : 'pointer');
		jQuery(s_selector).find('.next_btn').css('cursor', 'pointer');
		
		var tab = slider.find('.slides table');
		
		tab.css('width', w_max + w_item).css('left', -w_item);
		node.css('display', 'table-cell');
		tab.animate({left: '0'}, duration, easing, function() {
			last.removeClass('last_visible').css('display', 'none').prev().addClass('last_visible');
			jQuery(this).css('width', w_max);
			first.removeClass('first_visible').prev().addClass('first_visible');
		});
	}
	
	/**
	 * Move right
	 *
	 * @param object event - DOMEvents object
	 *
	 * @return void
	 */
	function nextItem(event)
	{
		var slider = jQuery(s_selector);
		var last   = slider.find('.last_visible');
		var node   = last.next();
		
		if (node.size() == 0) return;
		
		var first = jQuery(s_selector).find('.first_visible');
		
		jQuery(s_selector).find('.prev_btn').css('cursor', 'pointer');
		jQuery(s_selector).find('.next_btn').css('cursor', node.hasClass('last') ? 'default' : 'pointer');
		
		var tab = slider.find('.slides table');
		
		tab.css('width', w_max + w_item);
		node.css('display', 'table-cell');
		tab.animate({left: '-' + w_item + 'px'}, duration, easing, function() {
			first.removeClass('first_visible').css('display', 'none').next().addClass('first_visible');
			jQuery(this).css('width', w_max).css('left', 0);
			last.removeClass('last_visible').next().addClass('last_visible');	
		});
	}
	
	
	
	

	/**
	 * Return video type
	 *
	 * @param string url
	 *
	 * @return string
	 */
	function getVideoType(url)
	{
		var patt = /\.([^.\s]+)$/g;
		var res  = patt.exec(url);
		
		return (res && res.length == 2) ? res[1] : '';
	}
	
	/**
	 * Generate html for displaying video
	 *
	 * @param object node
	 * @param object data
	 *
	 * @return string
	 */
	function generateHTMLForVideo(node, data)
	{
		var html = '';
		var type = getVideoType(data.video);
		
		if (!type) return html;
		
		var AUTOPLAY   = auto_play ? 'true' : 'false';
	    var BACKGROUND = 'transparent';
		var CONTROLBAR = '&controlbar=bottom';
		var WMODE      = 'transparent'; // window, transparent, opaque
		
		switch (type)
		{
			case 'flv':
				html = 
					'<object type="application/x-shockwave-flash" style="width:' + w_preview + 'px; height:' + h_preview + 'px;" data="' + players_dir + 'mediaplayer_4.0.46.swf">' +
						'<param name="movie" value="' + players_dir + 'pmediaplayer_4.0.46.swf" />' +
						'<param name="quality" value="high" />' +
						'<param name="wmode" value="' + WMODE + '" />' +
						'<param name="bgcolor" value="' + BACKGROUND + '" />' +
						'<param name="autoplay" value="' + AUTOPLAY + '" />' +
						'<param name="allowfullscreen" value="true" />' +
						'<param name="allowscriptaccess" value="always" />' +
						'<param name="flashvars" value="file=' + data.video + (data.b_img ? '&image=' + data.b_img : '') + '&autostart=' + AUTOPLAY + CONTROLBAR + '&fullscreen=true" />' +
					'</object>'
				;
				break;
			
			case 'mp3':
				html =
					'<object type="application/x-shockwave-flash\" style="width:' + w_preview + 'px; height:' + h_preview + 'px;" data="' + players_dir + 'mediaplayer_4.0.46.swf">' +
						'<param name="movie" value="' + players_dir + 'mediaplayer_4.0.46.swf" />' +
						'<param name="quality" value="high" />' +
						'<param name="wmode" value="' + WMODE + '" />' +
						'<param name="bgcolor" value="' + BACKGROUND + '" />' +
						'<param name="autoplay" value="' + AUTOPLAY + '" />' +
						'<param name="allowfullscreen" value="true" />' +
						'<param name="allowscriptaccess" value="always" />' +
						'<param name="flashvars" value="file=' + data.video + '&autostart=' + AUTOPLAY + '" />' +
					'</object>'
				;
				break;
			
			case 'swf':
				html =
					'<object type="application/x-shockwave-flash" style="width:' + w_preview + 'px; height:' + h_preview + 'px;" data="' + data.video + '">' +
						'<param name="movie" value="' + data.video + '" />'
						'<param name="quality" value="high" />' +
						'<param name="wmode" value="' + WMODE + '" />' +
						'<param name="bgcolor" value="' + BACKGROUND + '" />' +
						'<param name="autoplay" value="' + AUTOPLAY + '" />' +
					'</object>'
				;
				break;
			
			case 'wmv':
				var id = data.video.replace(/[\/.\s]/gi, '_');
				
				if (_use_share_point_silver_light)
				{
					html =
					'<span id="' + id + '" style="width:' + w_preview + 'px; height:' + h_preview + 'px;">' +
						'<object data="data:application/x-silverlight-2," height="' + h_preview + '" type="application/x-silverlight-2" width="' + w_preview + '">' +
							'<param name="source" value="OVP.xap" />' +
							'<param name="minRuntimeVersion" value="2.0.30923.0" />' +
							'<param name="onerror" value="onSilverlightError" />' +
							'<param name="background" value="black" />' +
							'<param name="MaxFrameRate" value="30" />' +
							'<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">' +
								'<img alt="Get Microsoft Silverlight" src="http://go.microsoft.com/fwlink/?LinkId=108181" style="border-style: none" />' +
							'</a>' +
							'<param name="initparams" value="showstatistics=false, autoplay=' + AUTOPLAY + ', muted=false, playlistoverlay=false, theme=SmoothHD.xaml, stretchmode=Stretch, stretchmodefullscreen=Stretch, mediasource=' + data.video + '" />' +
						'</object>' +
					'</span>'
					;
					
					break;
				}
				
				html =
					'<span id="' + id + '" style="width:' + w_preview + 'px; height:' + h_preview + 'px;"></span>' +
					'<script type="text/javascript">' +
						"var cnt = document.getElementById('" + id + "');" +
						"var src = '" + players_dir + "wmvplayer.xaml';" +
						"var cfg = {" +
							"file:'" + data.video + "'," +
							"width:'" + w_preview + "'," +
							"height:'" + h_preview + "'," +
							"autostart:'" + AUTOPLAY + "'," +
							"image:'" + (data.b_img ? data.b_img : '' ) + "'" +
						"};" +
						"var ply = new jeroenwijering.Player(cnt,src,cfg);" +
					'</script>'
				;
				break;
			
			case 'wma':
				var id = data.video.replace(/[\/.\s]/gi, '_');
				
				if (_use_share_point_silver_light)
				{
					html =
					'<span id="' + id + '" style="width:' + w_preview + 'px; height:' + h_preview + 'px;">' +
						'<object data="data:application/x-silverlight-2," height="' + h_preview + '" type="application/x-silverlight-2" width="' + w_preview + '">' +
							'<param name="source" value="OVP.xap" />' +
							'<param name="minRuntimeVersion" value="2.0.30923.0" />' +
							'<param name="onerror" value="onSilverlightError" />' +
							'<param name="background" value="black" />' +
							'<param name="MaxFrameRate" value="30" />' +
							'<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">' +
								'<img alt="Get Microsoft Silverlight" src="http://go.microsoft.com/fwlink/?LinkId=108181" style="border-style: none" />' +
							'</a>' +
							'<param name="initparams" value="showstatistics=false, autoplay=' + AUTOPLAY + ', muted=false, playlistoverlay=false, theme=SmoothHD.xaml, stretchmode=Stretch, stretchmodefullscreen=Stretch, mediasource=' + data.video + '" />' +
						'</object>' +
					'</span>'
					;
					
					break;
				}
				
				html =
					'<span id="' + id + '" style="width:' + w_preview + 'px; height:' + h_preview + 'px;"></span>' +
					'<script type="text/javascript">' +
						"var cnt = document.getElementById('" + id + "');" +
						"var src = '" + players_dir + "wmvplayer.xaml';" +
						"var cfg = {" +
							"file:'" + data.video + "'," +
							"width:'" + w_preview + "'," +
							"height:'" + h_preview + "'," +
							"autostart:'" + AUTOPLAY + "'," +
							"usefullscreen: 'false'" +
						"};" +
						"var ply = new jeroenwijering.Player(cnt,src,cfg);" +
					'</script>'
				;
				
				break;
			
			case 'mov':
				html = QT_GenerateOBJECTText_XHTML(data.video, w_preview, h_preview, '', 'AUTOPLAY', AUTOPLAY, 'BGCOLOR', BACKGROUND, 'SCALE', 'Aspect');
				break;
			
			case 'mp4':
				html = QT_GenerateOBJECTText_XHTML(data.video, w_preview, h_preview, '', 'AUTOPLAY', AUTOPLAY, 'BGCOLOR', BACKGROUND, 'SCALE', 'Aspect');
				break;
			
			case '3gp':
				html = QT_GenerateOBJECTText_XHTML(data.video, w_preview, h_preview, '', 'AUTOPLAY', AUTOPLAY, 'BGCOLOR', BACKGROUND, 'SCALE', 'Aspect');
				break;
			
			case 'divx':
				html =
					'<object type="video/divx" data="' + data.video + '" style="width:' + w_preview + 'px; height:' + h_preview + 'px;">' +
						'<param name="type" value="video/divx" />' +
						'<param name="src" value="' + data.video + '" />' +
						'<param name="data" value="' + data.video + '" />' +
						'<param name="codebase" value="' + data.video + '" />' +
						'<param name="url" value="' + data.video + '" />' +
						'<param name="mode" value="full" />' +
						'<param name="pluginspage" value="http://go.divx.com/plugin/download/" />' +
						'<param name="allowContextMenu" value="true" />' +
						'<param name="previewImage" value="' + (data.b_img ? data.b_img : '' ) + '" />' +
						'<param name="autoPlay" value="' + AUTOPLAY + '" />' +
						'<param name="minVersion" value="1.0.0" />' +
						'<param name="custommode" value="none" />' +
						'<p>No video? Get the DivX browser plug-in for <a href="http://download.divx.com/player/DivXWebPlayerInstaller.exe">Windows</a> or <a href="http://download.divx.com/player/DivXWebPlayer.dmg">Mac</a></p>' +
					'</object>'
				;
				break;
			
			default:
				html = '<p class="video_gallery_error">Unknow format</p>';
		}
		
		return html;
	}
	
	function sharePointSilverLight()
	{
		html =
			'<object id="slp" data="data:application/x-silverlight-2," height="100%" type="application/x-silverlight-2" width="100%">' +
				'<param name="source" value="OVP.xap" />' +
				'<param name="minRuntimeVersion" value="2.0.30923.0" />' +
				'<param name="onerror" value="onSilverlightError" />' +
				'<param name="background" value="black" />' +
				'<param name="MaxFrameRate" value="30" />' +
				'<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">' +
					'<img alt="Get Microsoft Silverlight" src="http://go.microsoft.com/fwlink/?LinkId=108181" style="border-style: none" />' +
				'</a>' +
				'<param name="initparams" value="showstatistics=false, autoplay=true, muted=false, playlistoverlay=false, theme=SmoothHD.xaml, stretchmode=Stretch, stretchmodefullscreen=Stretch, mediasource=' + parametr[0] + '" />' +
			'</object>'
		;
	}
	
	
	
	
	/***************************************************** Application functions ********************************************************/
	
	
	
	/**
	 * Set Application in Active
	 * 
	 * @return void
	 */
	function appActive()
	{
		jQuery('#TB_overlay').remove();
		jQuery('#TB_load').remove();
	}
	
	/**
	 * Set Application in Inactive
	 * 
	 * @return void
	 */
	function appInactive()
	{
		if (jQuery('#TB_overlay').size() != 0) return;
		
		jQuery('body').append('<div id="TB_overlay" class="TB_overlayBG"></div>');
	}
	
	/**
	 * Add Loader to page
	 *
	 * @return void
	 */
	function appAddLoader()
	{
		if (jQuery('#TB_load').size() != 0) return;
		
		jQuery('body').append('<div id="TB_load" style="display: block;"><div class="loader"></div></div>');
	}
	
	/**
	 * Display loader
	 *
	 * @param boolean flag - if false - loader hide. Else - lodaer show
	 *
	 * @return void
	 */
	function appDisplayLoader(flag)
	{
		if (jQuery('#TB_load').size() == 0 && flag)
		{
			appAddLoader();
		}
		else
		{
			jQuery('#TB_load').css("display", flag ? 'block' : 'none');
		}
	}
	
	
	
	
	
	/******************************************************* Image gallery function *********************************************************/
	
	
	/**
	 * Show full image
	 * 
	 * @param DOMElements node
	 * 
	 * @return void
	 */
	function onShowFullImage(node)
	{
		if (opened)
		{
			hideImage();
		}
		else
		{
			showBlanket();
		}
		
		appDisplayLoader(true);
		
		var path = jQuery(node).attr('src');
		
		if (!path.length || path.length == 0) return false;
		
		
		var img    = new Image();
		img.onload = function()	{ onLoad(node, this); };
		img.src    = path;
	};

	/**
	 * Close popup window with full image
	 * 
	 * @return void
	 */
	function onCloseFullImage()
	{
		hideImage();
		
		hideBlanket();
	};
	
	/**
	 * Load full image
	 * 
	 * @param DOMElements node - small image container
	 * @param object obj       - full image object
	 * 
	 * @return void
	 */
	function onLoad(node, obj)
	{
		var current = jQuery(s_selector).find('.item.current');
		var node_id = current.attr('node');
		
		var img_title = (data[node_id] && data[node_id]['title']) ? data[node_id]['title'] : '';
		var img_desc  = (data[node_id] && data[node_id]['description']) ? data[node_id]['description'] : '';
		
		var win  = jQuery('#' + pop_up_id);
		var func = 'checkPosition(obj)';
		
		var win_cont  = win.find('.pop_up_content');
		var alt       = (img_title ? img_title + '. ' : '') + (img_desc ? img_desc : '');
		var content   = '<img src="' + obj.src + '" alt="' + alt + '" />';
		var signature = '';
		
		if (options.show_title && img_title)
		{
			signature += '<div class="title">' + img_title + '</div>';
		}
		
		if (options.show_desc && img_desc)
		{
			signature += '<div class="description">' + img_desc + '</div>';
		}
		
		if (signature)
		{
			content += '<div class="signature">' + signature + '</div>';
			
			win_cont.css('padding-bottom', '0px');
			
			func = 'checkPositionWithSignature(obj)';
		}
		
		win_cont.html(content);
		
		if (signature) win_cont.find('.signature :last').css('padding-bottom', '11px');
		
		eval(func);
				
		appDisplayLoader(false);
		
		showImage();
	}
	
	/**
	 * Set popup window position
	 *
	 * @return void
	 */
	function checkPosition(obj)
	{
		var win  = jQuery('#' + pop_up_id);
		var img  = win.find('.pop_up_content img');
		var html = document.documentElement;
		
		var width  = obj.width;
		var height = obj.height;
		
		if ((obj.width + 85) > html.clientWidth)
		{
			if ((obj.height + 85) > html.clientHeight)
			{
				if (obj.width/obj.height < html.clientWidth/html.clientHeight)
				{
					height = html.clientHeight - 85;
					width  = Math.round(obj.width*height/obj.height);
				}
				else
				{
					width  = html.clientWidth - 85;
					height = Math.round(obj.height*width/obj.width);
				}
			}
			else
			{
				width  = html.clientWidth - 85;
				height = Math.round(obj.height*width/obj.width);
			}
		}
		else if ((obj.height + 85) > html.clientHeight)
		{
			height = html.clientHeight - 85;
			width  = Math.round(obj.width*height/obj.height);
		}
		
		if (width  < 0) width  = 0;
		if (height < 0) height = 0;
		
		img.attr('width', width);
		
		var left = Math.round((html.clientWidth - width - 30)/2);
		var top  = Math.round((html.clientHeight - height - 30)/2);
		
		if (html.scrollLeft == 0 && html.scrollTop == 0)
		{
			var body = jQuery(html).find('body').get(0);
			
			left += body.scrollLeft;
			top  += body.scrollTop;
		}
		else
		{
			left += html.scrollLeft;
			top  += html.scrollTop;
		}
		
		win.css('left', left + 'px');
		win.css('top',  top  + 'px');
		
		win.find('.pop_up_content').attr('style', '').css('width', width);
		win.css('width', (width + 32)).css('min-width', (width + 32));
	}
	
	/**
	 * Set popup window position (for window with signature)
	 *
	 * @return void
	 */
	function checkPositionWithSignature(obj)
	{
		var win      = jQuery('#' + pop_up_id);
		var win_cont = win.find('.pop_up_content');
		var sign     = win_cont.find('.signature');
		
	   	var img  = win_cont.find('img');
		var html = document.documentElement;
		
		var i_width  = obj.width;
		var i_height = obj.height;
		
		win_cont.css('width',  i_width).css('height', 'auto');
		sign.css('height', 'auto');
		win.css('width', 'auto').css('height', 'auto').css('min-width', '')
		   .css('top', '9999px').css('display', 'block');
		
		var w_width  = i_width;
		var w_height = win_cont.get(0).offsetHeight - 28;
		
		var s_height = sign.get(0).offsetHeight;
		
		if (s_height > i_height)
		{
			w_height -= s_height - i_height;
			s_height  = i_height;
		}
		
		var width  = w_width;
		var height = w_height;
		
		if ((w_width + 85) > html.clientWidth)
		{
			if ((w_height + 85) > html.clientHeight)
			{
				if (w_width/w_height < html.clientWidth/html.clientHeight)
				{
					height    = html.clientHeight - 85;
					var n_i_h = Math.round(i_height*height/w_height);
					s_height  = height - n_i_h;
					width     = Math.round(i_width*n_i_h/i_height);
				}
				else
				{
					width     = html.clientWidth - 85;
					height    = Math.round(w_height*width/w_width);
					var n_i_w = Math.round(i_width*width/w_width);
					var n_i_h = Math.round(i_height*n_i_w/i_width);
					s_height  = height - n_i_h;
				}
			}
			else
			{
				width     = html.clientWidth - 85;
				height    = Math.round(w_height*width/w_width);
				var n_i_w = Math.round(i_width*width/w_width);
				var n_i_h = Math.round(i_height*n_i_w/i_width);
				s_height  = height - n_i_h;
			}
		}
		else if ((w_height + 85) > html.clientHeight)
		{
			height    = html.clientHeight - 85;
			var n_i_h = Math.round(i_height*height/w_height);
			s_height  = height - n_i_h;
			width     = Math.round(i_width*n_i_h/i_height);
		}
		
		s_height -= 11;
		
		if (width  < 0) width  = 0;
		if (height < 0) height = 0;
		if (s_height < 20) s_height = 20;
		
		img.attr('width', width);
		
		var left = Math.round((html.clientWidth - width - 30)/2);
		var top  = Math.round((html.clientHeight - height - 30)/2);
		
		if (html.scrollLeft == 0 && html.scrollTop == 0)
		{
			var body = jQuery(html).find('body').get(0);
			
			left += body.scrollLeft;
			top  += body.scrollTop;
		}
		else
		{
			left += html.scrollLeft;
			top  += html.scrollTop;
		}
		
		win.css('left', left + 'px');
		win.css('top',  top  + 'px');
		
		win_cont.css('width', width).css('height', 'auto');
		sign.css('height', s_height);
		win.css('width', (width + 32)).css('min-width', (width + 32));
	}
	
	function showBlanket()
	{
		jQuery('#' + blanket_id).css('display', 'block');
	}
	
	function hideBlanket()
	{
		jQuery('#' + blanket_id).css('display', 'none');
	}
	
	function showImage()
	{
		jQuery('#' + pop_up_id).css('display', 'block');
	}
	
	function hideImage()
	{
		jQuery('#' + pop_up_id).css('display', 'none');
	}
	
	function generatePopUpHtml()
	{
		if (jQuery('#' + blanket_id).size() == 0)
		{
			jQuery('body').append('<div id="' + blanket_id + '" style="display: none;"></div>');
			
			jQuery('#' + blanket_id).click(function(event) {
				onCloseFullImage();
			});
		}
		
		if (jQuery('#' + pop_up_id).size() == 0)
		{
			var html = 
				'<div id="' + pop_up_id + '" style="display: none;">' +
					'<div class="close"></div>' +
					'<div class="pop_up_content"></div>' +
				'</div>'
			;
			
			jQuery('body').append(html);
			
			jQuery('#' + pop_up_id + ' .close').click(function(event) {
				onCloseFullImage();
			});
		}
	}
}
