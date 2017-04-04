var openpost_api = openpost_api || (function () {
    var sourceapi = '';
    var language = 'en';

    var platform = '';

    var identifier = '';
    var url = '';
    var isforum = false;
    
    var userid = '';
    var token = '';

    var converter;

    return {
        init: function (cb) {
            if (cb) cb.call(this);
            if (!window.jQuery)
            {
                var script = document.createElement('script');
                var head = document.head || document.getElementsByTagName('head')[0];
                script.type = 'text/javascript';
                script.src = "//ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js";
                script.async = false;
                head.insertBefore(script, head.firstChild);
            }
            var css = document.createElement("link");
            document.head.appendChild(css);
            css.rel = "stylesheet";
            css.type = "text/css";
            css.href = this.sourceapi + "/theme.css";
            if (this.isforum)
            {
                //First check if fonts-awesome is available
                var span = document.createElement('span');

                span.className = 'fa';
                span.style.display = 'none';
                document.body.insertBefore(span, document.body.firstChild);

                if ((window.getComputedStyle(span, null).getPropertyValue('font-family')) !== 'FontAwesome') {
                    //Page doesn't have font-awesome, so load through cdn
                    css = document.createElement("link");
                    document.head.appendChild(css);
                    css.rel = "stylesheet";
                    css.type = "text/css";
                    css.href = "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css";
                }
                document.body.removeChild(span);
                //End check
                //Load Custom MD-Editor
                css = document.createElement("link");
                document.head.appendChild(css);
                css.rel = "stylesheet";
                css.type = "text/css";
                css.href = this.sourceapi + "/md-edit.css";
                //End loading
                //Load showdown 
                //script = document.createElement('script');
                //head = document.head || document.getElementsByTagName('head')[0];
                //script.type = 'text/javascript';
                //script.src = "//cdnjs.cloudflare.com/ajax/libs/showdown/1.6.4/showdown.min.js";
                //script.async = false;
                //head.insertBefore(script, head.firstChild);
                //End Loading
                //Init showdownjs
                converter = new showdown.Converter({ 'tasklists': true, 'strikethrough': true, 'simpleLineBreaks': true });

            }
        },
        execRequest: function (target)
        {
            var request = { Id: this.userid, Token: this.token, SourcePlatform: this.platform, Page: this.identifier, Language: this.language, IsForum: this.isforum }

            $.post(this.sourceapi + "/comment/show", request, function (data) {
                //Display html inside target div element
                $('#' + target).html(data);
                //Enable reply link buttons
                $('.dapi-100_comment-reply-link').click(function () {
                    var parentid = $(this).data('id');
                    $('#dapi-100_PostComment textarea').val('');
                    $('#dapi-100_PostComment input[name="Current"]').val('');
                    $('p.dapi-100_comment-edit-msg').hide();
                    $('#dapi-100_PostComment input[name="Parent"]').val(parentid);
                    var linkToPost = $('p.dapi-100_comment-response-msg a').first();
                    linkToPost.attr('href', '#' + parentid);
                    linkToPost.html($(this).data('author'));
                    $('p.dapi-100_comment-response-msg').show();
                    $('#dapi-100_PostComment textarea').focus();
                });
                //enable edit post buttons
                $('.dapi-100_edit').click(function () {
                    var msgid = $(this).data('id');
                    $('#dapi-100_PostComment input[name="Parent"]').val('');
                    $('p.dapi-100_comment-response-msg').hide();
                    $('#dapi-100_PostComment input[name="Current"]').val(msgid);
                    var linkToPost = $('p.dapi-100_comment-edit-msg a').first();
                    linkToPost.attr('href', '#' + msgid);
                    $('p.dapi-100_comment-edit-msg').show();
                    $('#dapi-100_PostComment textarea').val($('#dapi-100_comment-text-' + msgid).val());
                    $('.dapi-100_sub-add-button').hide();
                    $('.dapi-100_sub-edit-button').show();
                    $('#dapi-100_PostComment textarea').focus();
                });
                //enable close button in delete comment modal
                $('.dapi-100_confirm-modal-close').click(function () {
                    $('.dapi-100_confirm-modal').hide();
                });
                //enable delete post button
                $('.dapi-100_delete').click(function () {
                    $('#dapi-100_DeleteComment input[name="CommentId"]').val($(this).data('id'));
                    $('.dapi-100_confirm-modal').show();
                });
                //On delete post form submit
                $('#dapi-100_DeleteComment').submit(function () {
                    event.preventDefault();
                    $('#dapi-100_DeleteComment input[type="submit"]').hide();
                    $.post(openpost_api.sourceapi + "/comment/delete", $('#dapi-100_DeleteComment').serialize(), function (data) {
                        openpost_api.execRequest('openpost-system');
                    });
                });
                //Cancel delete post
                $('.dapi-100_cancel-delete').click(function () {
                    $('#dapi-100_DeleteComment input[name="CommentId"]').val('');
                    $('.dapi-100_confirm-modal').hide();
                });
                //On new or edited comment form submit
                $('#dapi-100_PostComment').submit(function (event) {
                    event.preventDefault();
                    $('#dapi-100_PostComment input[type="submit"]').hide();
                    $('.dapi-100_updating-text').show();
                    $.post(openpost_api.sourceapi + "/comment/post", $('#dapi-100_PostComment').serialize(), function (data) {
                        openpost_api.execRequest('openpost-system');
                    });
                });
                //Cancel Replying mode
                $('p.dapi-100_comment-response-msg .dapi-100_cancel-comment-link').click(function () {
                    $('#dapi-100_PostComment input[name="Parent"]').val('');
                    $('p.dapi-100_comment-response-msg').hide();
                });
                //Cancel editing mode
                $('p.dapi-100_comment-edit-msg .dapi-100_cancel-edit-comment-link').click(function () {
                    $('#dapi-100_PostComment input[name="Current"]').val('');
                    $('p.dapi-100_comment-edit-msg').hide();
                    $('#dapi-100_PostComment textarea').val('');
                    $('.dapi-100_sub-edit-button').hide();
                    $('.dapi-100_sub-add-button').show();
                });
                if (openpost_api.isforum)
                {
                    //Activate md edit button if we are in forum mode
                    $('.dapi-100_md-edit-toolbar i.fa').on('click', function () {
                        $('#dapi-100_md-textarea').applymd($(this));
                    });
                    //Activate showdown preview
                    $('#dapi-100_md-textarea').keyup(function () {
                        openpost_api.previewmd();
                    });
                    $('.dapi-100_md-content').each(function (index, data) {
                        var html = $(data).text();
                        openpost_api.converter = new showdown.Converter({ 'tasklists': true, 'strikethrough': true, 'simpleLineBreaks': true });
                        var text = openpost_api.converter.makeHtml(html);
                        $(data).html(text);
                    });
                }
            });
        },
        previewmd: function ()
        {
            var html = $('#dapi-100_md-textarea').val();
            var text = converter.makeHtml(html);
            $('.dapi-100_md-preview').html(text);
        }
    };
}());

$.fn.selectRange = function (start, end) {
    var e = document.getElementById($(this).attr('id')); // I don't know why... but $(this) don't want to work today :-/
    if (!e) return;
    else if (e.setSelectionRange) { e.focus(); e.setSelectionRange(start, end); } /* WebKit */
    else if (e.createTextRange) { var range = e.createTextRange(); range.collapse(true); range.moveEnd('character', end); range.moveStart('character', start); range.select(); } /* IE */
    else if (e.selectionStart) { e.selectionStart = start; e.selectionEnd = end; }
};

$.fn.getLine = function () {
    var e = document.getElementById($(this).attr('id')); // I don't know why... but $(this) don't want to work today :-/
    if (!e) return -1;
    return e.value.substr(0, e.selectionStart).split("\n").length;
};

$.fn.jumpToLine = function (line) {
    var e = document.getElementById($(this).attr('id'));
    if (!e) return;
    var lineHeight = e.clientHeight / e.rows;
    var jump = (line - 1) * lineHeight;
    e.scrollTop = jump;
};

$.fn.applymd = function (elem) {
    var e = document.getElementById($(this).attr('id')); // I don't know why... but $(this) don't want to work today :-/
    if (!e) return -1;
    if ($(elem).data('modifier')) {
        var selectionStart = e.selectionStart;
        var selectionEnd = e.selectionEnd;
        var mod = $(elem).data('modifier');
        var v = $(e).val();
        var textBefore = v.substring(0, selectionStart);
        var textAfter = v.substring(selectionEnd, v.length);
        var textBetween = v.substring(selectionStart, selectionEnd);
        if ($(elem).hasClass('fa-code')) {
            textBetween += '\n';
        }
        $(e).val(textBefore + mod + textBetween + mod + textAfter);
        $(e).selectRange(selectionStart + mod.length, selectionEnd + mod.length);
    }
    if ($(elem).data('level')) {
        var level = parseInt($(elem).data('level'));
        var v = $(e).val();
        var textBefore = v.substring(0, e.selectionStart);
        var indexNewLine = textBefore.lastIndexOf('\n') + 1;
        var textBetween = '';
        if (indexNewLine > 0) {
            textBefore = v.substring(0, indexNewLine);
            textBetween = v.substring(indexNewLine, e.selectionStart);;
        }
        var textAfter = v.substring(e.selectionEnd, v.length);
        var prefix = '#'.repeat(level) + ' ';
        $(e).val(textBefore + prefix + textBetween + textAfter);
        $(e).selectRange(indexNewLine + prefix.length, indexNewLine + prefix.length);
    }
    if ($(elem).data('prefix')) {
        var prefix = $(elem).data('prefix');
        var v = $(e).val();
        var selectionStart = e.selectionStart;
        var textBefore = v.substring(0, selectionStart);
        var textAfter = v.substring(e.selectionEnd, v.length);
        if (v.charAt(selectionStart - 1) != '\n') {
            prefix = '\n' + prefix;
        }
        $(e).val(textBefore + prefix + textAfter);
        $(e).selectRange(selectionStart + prefix.length, selectionStart + prefix.length);
    }
    if ($(elem).data('sample-text') && $(elem).data('sample-url')) {
        var text = $(elem).data('sample-text');
        var url = $(elem).data('sample-url');

        var selectionStart = e.selectionStart;
        var selectionEnd = e.selectionEnd;

        var mod = $(elem).attr('title');

        var v = $(e).val();
        var textBefore = v.substring(0, selectionStart);
        var textAfter = v.substring(selectionEnd, v.length);
        var textBetween = v.substring(selectionStart, selectionEnd);

        if (textBetween != '') {
            text = textBetween;
        }

        if (mod == 'Link') {
            text = '[' + text + '](' + url + ')';
        } else {
            text = '![' + text + '](' + url + ')';
        }
        $(e).val(textBefore + text + textAfter);
        $(e).selectRange(selectionStart + text.length, selectionStart + text.length);
    }
    e.focus();
};

(function () {
    "use strict";

    /* Initialize api */
    openpost_api.init(openpost_config);
    openpost_api.execRequest('openpost-system');
})();


