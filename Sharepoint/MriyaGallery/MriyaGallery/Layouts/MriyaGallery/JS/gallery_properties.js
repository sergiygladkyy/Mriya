
function GalleryPropertiesForm(tag_id, settings) {
    var form_id = tag_id;
    var generated = false;
    var max_rows = settings.max_rows ? settings.max_rows : 9999;
    var show_video = settings.show_video;
    var prefix = 'fdata';
    var data = settings.data ? settings.data : {};
    var index = -1;
    var labels = (typeof settings.labels == 'object') ? settings.labels : {};

    var label_s_img = labels.small_image ? labels.small_image : 'Small image:';
    var label_b_img = labels.big_image ? labels.big_image : 'Big image:';
    var label_video = labels.video ? labels.video : 'Video:';
    var label_add = labels.add_row ? labels.add_row : 'Add';
    var label_del = labels.delete_row ? labels.delete_row : 'Delete';

    var cnt = 0;

    /**
    * Genertae form
    *
    * @return void
    */
    this.generate = function () {
        cnt = 0;

        jQuery('#' + form_id).html();

        var html = '<table border="0" cellspacing="0" cellpadding="0" class="gallery_properties"><tbody>';

        html += '<tr><td class="rows_container">';

        for (var i in data) {
            if (cnt >= max_rows) break;

            html += generateRow(data[i]);

            cnt++;
        }

        html += '</td></tr>';
        html += '<tr><td class="controls"><a href="#" class="add_row" onClick="return false;"' + (cnt >= max_rows ? ' style="display: none;"' : '') + '>' + label_add + '</a></td></tr>';
        html += '</tbody></table>';

        jQuery('#' + form_id).html(html);

        initializeForm.call(this);

        generated = true;
    };

    /**
    * Generate row content
    *
    * @param object data - row data
    *
    * @return string
    */
    function generateRow(data) {
        index++;

        var html = '<table border="0" cellspacing="0" cellpadding="0" class="row row_' + index + '"><tbody>';

        html += '<tr><td class="row_id" style="display: none;"><span class="row_index">' + index + '</span>';

        if (data['id']) {
            html += '<input type="hidden" id="id_' + index + '" name="' + prefix + '[' + index + '][id]" value="' + data['id'] + '" class="row_id" />';
        }

        html += '</td></tr>';

        html += '<tr><td><div class="label small_image_label"><nobr>' + label_s_img + '</nobr></div>';
        html += '<input type="text" id="s_img_' + index + '" name="' + prefix + '[' + index + '][s_img]" value="' + (data['s_img'] ? data['s_img'] : '') + '" class="small_image" /></td></tr>';

        html += '<tr><td><div class="label big_image_label"><nobr>' + label_b_img + '</nobr></div>';
        html += '<input type="text" id="b_img_' + index + '" name="' + prefix + '[' + index + '][b_img]" value="' + (data['b_img'] ? data['b_img'] : '') + '" class="big_image" /></td></tr>';

        if (show_video) {
            html += '<tr><td><div class="label video_label"><nobr>' + label_video + '</nobr></div>';
            html += '<input type="text" id="video_' + index + '" name="' + prefix + '[' + index + '][video]" value="' + (data['video'] ? data['video'] : '') + '" class="video" /></td></tr>';
        }

        html += '<tr><td class="delete_control"><a href="#" class="delete_row" onClick="return false;">' + label_del + '</a></td></tr>';

        return html + '</tbody></table>'
    }

    /**
    * Initialize form (add event listeners)
    *
    * @return void
    */
    function initializeForm() {
        var addFunc = this.addRow;

        jQuery('#' + form_id + ' .controls .add_row').click(function (event) {
            /*event = event || window.event;
            var node = event.target || event.srcElement;*/

            addFunc();

            if (cnt >= max_rows) hideAddControl();
        });

        initializeRows();
    }

    /**
    * Initialize rows (add event listeners). If is set param index - initialize row with this index
    *
    * @param int index - row index
    *
    * @return void
    */
    function initializeRows(index) {
        var selector = '#' + form_id + ' .rows_container' + (index >= 0 ? ' .row_' + index : '');

        jQuery(selector).find('.delete_row').click(function (event) {
            event = event || window.event;
            var node = event.target || event.srcElement;

            node = jQuery(node).parents('.row').get(0);

            _delete(node);
        });
    }

    /**
    * Delete row
    *
    * @param DOMElements row - row node
    *
    * @return void
    */
    function _delete(row) {
        row = jQuery(row);

        var id = row.find('input.row_id').attr('value');

        if (id > 0) {
            var ind = row.find('.row_index').text();

            row.css('display', 'none');
            row.html('<input type="hidden" id="deleted_' + ind + '" name="' + prefix + '[deleted][' + ind + ']" value="' + id + '">');
        }
        else {
            row.remove();
        }

        if (cnt == max_rows) {
            showAddControl();
        }

        cnt--;
    }

    /**
    * Show "Add" link
    *
    * @return void
    */
    function showAddControl() {
        jQuery('#' + form_id + ' .controls .add_row').css('display', 'inline');
    }

    /**
    * Hide "Add" link
    *
    * @return void
    */
    function hideAddControl() {
        jQuery('#' + form_id + ' .controls .add_row').css('display', 'none');
    }

    /**
    * Show form
    *
    * @return void
    */
    this.show = function () {
        if (!generated) this.generate();

        jQuery('#' + form_id).css('dispaly', 'block');
    };

    /**
    * Hide form
    *
    * @return void
    */
    this.hide = function () {
        jQuery('#' + form_id).css('dispaly', 'none');
    };

    /**
    * Add new row
    *
    * @param object data - row data
    *
    * @return void
    */
    this.addRow = function (data) {
        if (typeof data != 'object') data = {};

        if (cnt >= max_rows) return;

        var html = generateRow(data);

        jQuery('#' + form_id + ' .rows_container').append(html);

        cnt++;

        initializeRows(index);
    };

    /**
    * Delete row by index
    *
    * @param int index - row index
    *
    * @return void
    */
    this.deleteRow = function (index) {
        if (jQuery('#' + form_id + ' .rows_container .row_' + index).size() == 0) return false;

        var row = jQuery('#' + form_id + ' .rows_container .row_' + index).get(0);

        _delete(row);

        return true;
    };
}