/**
 * Gallery constructor
 *
 * @param string slider_tag_id
 * @param string preview_tag_id
 * @param object settings       - gallery settings
 *
 * @return void
 */
function oiltecGallery(slider_tag_id, preview_tag_id, settings)
{
    var players_dir = settings.players_dir ? settings.players_dir : '/players/';
	
	var _use_share_point_silver_light = settings.use_share_point_silver_light ? true : false;
	
	var s_tag_id = slider_tag_id;
	var p_tag_id = preview_tag_id;
	
	var data  = settings.data ? settings.data : {};
	var s_set = settings.slider ? settings.slider : {};
	var p_set = settings.preview ? settings.preview : {};
	
	
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
	
	var _w_lft = parseInt(jQuery('#' + s_tag_id + ' .prev_btn_container').css('width'), 10);
	var _w_rgt = parseInt(jQuery('#' + s_tag_id + ' .next_btn_container').css('width'), 10);
	
	
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
		
		jQuery('#' + s_tag_id).css('width', slider_width).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0').
			find('.container').css('width', cont_width).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0').
			find('.slides').css('width', w_max).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0 auto').
			find('table').css('width', w_max).css('height', h_max).css('borderWidth', '0').css('padding', '0').css('margin', '0 auto').css('borderCollapse', 'collapse').css('position', 'relative');
		
		jQuery('#' + s_tag_id + ' .zoom').each(function(index) {
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
		
		jQuery('#' + s_tag_id + ' .item').css('width', w_img).css('height', h_img).
			css('borderWidth', item_border.top + 'px ' + item_border.right + 'px ' + item_border.bottom + 'px ' + item_border.left + 'px').
			css('padding',  item_padding.top + 'px ' + item_padding.right + 'px ' + item_padding.bottom + 'px ' + item_padding.left + 'px').
			css('margin', '0px');
		
		
		if (jQuery('#' + s_tag_id + ' .first_visible').hasClass('first'))
		{
			jQuery('#' + s_tag_id + ' .prev_btn').css('cursor', 'default');
		}
		
		if (jQuery('#' + s_tag_id + ' .last_visible').hasClass('last'))
		{
			jQuery('#' + s_tag_id + ' .next_btn').css('cursor', 'default');
		}
		
		
		/* Initialize preview */
		
		jQuery('#' + p_tag_id).css('width', w_preview).css('height', h_preview).
			css('borderWidth', border.top + 'px ' + border.right + 'px ' + border.bottom + 'px ' + border.left + 'px').
			css('padding', padding.top + 'px ' + padding.right + 'px ' + padding.bottom + 'px ' + padding.left + 'px').
			css('margin', '0px');
		
		
		/* Events */
		
		jQuery('#' + s_tag_id + ' .prev_btn').click(function(event) {
			prevItem(event);
		});
		
		jQuery('#' + s_tag_id + ' .next_btn').click(function(event) {
			nextItem(event);
		});
		
		var onClick = this.onClick;
		
		jQuery('#' + s_tag_id + ' .item a').click(function(event) {
			event = event || window.event;
			var node = event.target || event.srcElement;
			
			if (!jQuery(node).hasClass('item')) node = jQuery(node).parents('.item').get(0);
			
			onClick(node);
		});
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
		var slider = jQuery('#' + s_tag_id);
		var first  = slider.find('.first_visible');
		var node   = first.prev();
		
		if (node.size() == 0) return;
		
		var last = jQuery('#' + s_tag_id + ' .last_visible');
		
		jQuery('#' + s_tag_id + ' .prev_btn').css('cursor', node.hasClass('first') ? 'default' : 'pointer');
		jQuery('#' + s_tag_id + ' .next_btn').css('cursor', 'pointer');
		
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
		var slider = jQuery('#' + s_tag_id);
		var last   = slider.find('.last_visible');
		var node   = last.next();
		
		if (node.size() == 0) return;
		
		var first = jQuery('#' + s_tag_id + ' .first_visible');
		
		jQuery('#' + s_tag_id + ' .prev_btn').css('cursor', 'pointer');
		jQuery('#' + s_tag_id + ' .next_btn').css('cursor', node.hasClass('last') ? 'default' : 'pointer');
		
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
			html = '<img src="' + data[node_id].b_img + '" alt="" />';
		}
		
		jQuery('#' + p_tag_id).html(html);
	};

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
}
