var openpost_api = openpost_api || (function () {
    var sourceapi = '';
    var language = 'en';

    var platform = '';

    var identifier = '';
    var url = '';
    
    var userid = '';
    var token = '';

    return {
        init: function (cb) {
            if (cb) cb.call(this);
            if (!window.jQuery)
            {
                var script = document.createElement('script');
                document.head.appendChild(script);
                script.type = 'text/javascript';
                script.src = "//ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js";
            }
            var css = document.createElement("link");
            document.head.appendChild(css);
            css.rel = "stylesheet";
            css.type = "text/css";
            css.href = this.sourceapi + "/theme.css";
        },
        execRequest: function (target)
        {
            var request = { Id: this.userid, Token: this.token, SourcePlatform: this.platform }
            
            request.page = this.identifier;
            request.language = this.language;

            $.post(this.sourceapi + "/comment/show", request, function (data) {
                //Display html inside target div element
                $('#' + target).html(data);
                //Enable reply link buttons
                $('.dapi-100_comment-author .dapi-100_comment-reply-link').click(function () {
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
                        openpost_api.execRequest('test-discutech');
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
                        openpost_api.execRequest('test-discutech');
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
            });
        }

    };
}());

(function () {
    "use strict";

    /* Initialize api */
    openpost_api.init(openpost_config);
    openpost_api.execRequest('openpost-system');
})();
