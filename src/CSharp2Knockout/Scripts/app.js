//
// documentation can be generated using docco or nocco (.NET port)
//
// comments are using markdown syntax
//
// Copyright (c) 2012 Jakub Gutkowski;
//
(function (window, $, undefined) {

    'use strict';

    String.prototype.format = function () {
        var str = this, i;
        for (i = 0; i < arguments.length; i++) {
            str = str.replace('{' + i + '}', arguments[i]);
        }
        return str;
    };

    var Alerts;


    // alerts encapsulate alerting information that use Twitter Bootstrap to add
    // close functionality to messages/alerts
    //
    // use it like this:
    //
    //     window.[Name].msg.success('my message', 'my title', true);
    //
    Alerts = (function () {

        // global alerts placeholder, where alerts will be added
        var $alerts = $('.alerts'),
            getSettings,
            getAlert;

        // function that checks what has been provided and returns defaults
        // for instance if title is bool it probably is what user provided for pernament
        //
        // rules:
        //
        //  - if `title` is undefined, use defaultTitle value
        //  - if `title` is bool, it means it's `permanent` value
        //  - if `permanent` is undefined, user `false` value for it
        getSettings = function (defaultTitle, message, title, permanent) {
            if (title && !permanent) {
                if (title === true || title === false) {
                    permanent = title;
                    title = undefined;
                } else {
                    permanent = false;
                }
            }

            if (!title) {
                title = defaultTitle;
            }

            return {
                message: message,
                title: title,
                permanent: permanent
            };
        };

        // method constructs an alert element - HTML string based on the options
        // passed by `getSettings`
        //
        // `type` defines a type of the alert to show:
        //
        //  - success
        //  - warning
        //  - error
        //  - info
        getAlert = function (type, options) {
            // default alert template
            var permanent, alertType,
                msg = '<div class="alert alert-block {0} fade in" id="licence-submit-error">' +
                '{1}' +
                '<h4 class="alert-heading">{2}</h4>' +
                '<p>' +
                '{3}' +
                '</p>' +
                '</div>';
            // checks if permanent is true or false, if true do not show X button
            permanent = function (isPermanent) {
                if (isPermanent) {
                    return '';
                }

                return '<a class="close" data-dismiss="alert" href="#">×</a>';
            };
            // checks for type of alert, if it's `warning` there is no point
            // of adding a class
            alertType = function (baseType) {
                if (baseType === 'warning') {
                    return '';
                }

                return 'alert-' + baseType;
            };

            msg = msg.format(alertType(type), permanent(options.permanent), options.title, options.message);

            return msg;
        };

        return {
            // access property to alerts functionality
            msg: {
                // show warning alert
                warn: function (message, title, permanent) {
                    var alert = getAlert('warning', getSettings('Warning!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                },
                // show success alert. last parameter `timeout` is used to hide alert after
                // time pass
                success: function (message, title, permanent, timeout) {
                    var alert = getAlert('success', getSettings('Success!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                    if (timeout) {
                        setTimeout(function () {
                            $alert.alert('close');
                        }, timeout);
                    }
                },
                // show error alert
                error: function (message, title, permanent) {
                    var alert = getAlert('error', getSettings('Error!', message, title, permanent)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                },
                // show info alert
                info: function (message, title, pernament) {
                    var alert = getAlert('info', getSettings('Information', message, title, pernament)),
                        $alert = $(alert);
                    $alerts.append($alert);
                    $alert.alert();
                }
            }
        };
    })();

    var findoutTimeout, getErros;

    getErros = function (data) {
        var errors = '';

        if (!data.success && data.message) {
            errors += data.message;
        } else {
            errors += 'Errors or Warning had occured during parsing:';
        }

        errors += '<ul>';

        $.each(data.errors, function (idx, item) {
            errors += '<li>';
            errors += item;
            errors += '</li>';
        });

        errors += '</ul>';

        return errors;
    };

    $('#generate').click(function (evt) {
        evt.preventDefault();

        var code = $.trim($("#csharp").val()),
            loading = $('#loading'),
            url = $(this).data().url, post;

        if (code.length === 0) {
            clearTimeout(findoutTimeout);
            $("#no-csharp").fadeIn("fast");
            findoutTimeout = setTimeout(function () {
                $("#no-csharp").fadeOut("slow");
            }, 4e3);

            return;
        }

        loading.show();

        post = $.post(url, { csharp: code }, function (data) {
            var vm = '', errors = '', temp;
            if (!data.success) {
                Alerts.msg.error(getErros(data), ' ');
                return;
            }

            temp = prettyPrintOne(data.message, 'js', true);

            vm += '<pre class="prettyprint linenums">';
            vm += temp;
            vm += '</pre>';

            if (data.errors && data.errors.length) {
                Alerts.msg.error(getErros(data), ' ');
            }

            Alerts.msg.info(vm, ' ');
            //prettyPrint();
        });

        post.complete(function () {
            loading.hide();
        });

    });

    window.Allerts = Alerts.msg;

})(window, jQuery);