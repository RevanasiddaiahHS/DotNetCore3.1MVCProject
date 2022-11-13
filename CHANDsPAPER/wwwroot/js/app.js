$(function () {
    $('.sidebar-main-toggle').on('click', function (e) {
        e.preventDefault();
        // Toggle min sidebar class
        $('body').toggleClass('sidebar-xs');
    });

    // Main navigation
    $('.navigation-main').find('li').has('ul').children('a').on('click', function (e) {
        e.preventDefault();

        // Collapsible
        $(this).parent('li').not('.disabled').not($('.sidebar-xs').not('.sidebar-xs-indicator').find('.navigation-main').children('li')).toggleClass('active').children('ul').slideToggle(250);

        // Accordion
        if ($('.navigation-main').hasClass('navigation-accordion')) {
            $(this).parent('li').not('.disabled').not($('.sidebar-xs').not('.sidebar-xs-indicator').find('.navigation-main').children('li')).siblings(':has(.has-ul)').removeClass('active').children('ul').slideUp(250);
        }
    });

    // Toggle main sidebar
    $('.sidebar-mobile-main-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-mobile-main').removeClass('sidebar-mobile-secondary sidebar-mobile-opposite sidebar-mobile-detached');
    });

    // Toggle detached sidebar
    $('.sidebar-mobile-detached-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-mobile-detached').removeClass('sidebar-mobile-main sidebar-mobile-secondary sidebar-mobile-opposite');
    });

    $('.import_flow').on('click', function () {
        $(this).hide();
        $(this).siblings('.importing').show();
        setTimeout(window.location.href = "edit-flow.php", 1000);
    })

    $('#landing .import_flow1').on('click', function () {
        $(this).addClass('btnremove');
        $(this).siblings('.importing').show();
        setTimeout(function () {
            $('.import_flow1').siblings('.importing').hide();
            $('#import-1').modal('hide');
            $('.import_flow1').removeClass('btnremove');
        }, 1000);
        var data = '<li class="ui-state-default panel pl-10"><h6><i class="icon-menu7 mr-10"></i>Landing Page <span class="pull-right"><a href="#" class="mr-20 text-default"><i class=" icon-eye mr-10"></i>View</a><a class="mr-20" href="#"><i class=" icon-pencil mr-10"></i>Edit</a><a class="mr-20 text-default" href="#"><i class="icon-copy3 mr-10"></i>Clone</a><a class="mr-20 text-danger" href="#"><i class="icon-bin mr-10"></i>Delete</a></span></h6></li>';
        $('ul#sortable').append(data);

    })

    $('#checkout .import_flow1').on('click', function () {
        $(this).addClass('btnremove');
        $(this).siblings('.importing').show();
        setTimeout(function () {
            $('.import_flow1').siblings('.importing').hide();
            $('#import-1').modal('hide');
            $('.import_flow1').removeClass('btnremove');
        }, 1000);
        var data = '<li class="ui-state-default panel pl-10"><h6><i class="icon-menu7 mr-10"></i>Checkout Page <span class="pull-right"><a href="#" class="mr-20 text-default"><i class=" icon-eye mr-10"></i>View</a><a class="mr-20" href="#"><i class=" icon-pencil mr-10"></i>Edit</a><a class="mr-20 text-default" href="#"><i class="icon-copy3 mr-10"></i>Clone</a><a class="mr-20 text-danger" href="#"><i class="icon-bin mr-10"></i>Delete</a></span></h6></li>';
        $('ul#sortable').append(data);

    })


    $('#add_attribute').on('click', function () {
        var data = '<div class="display-block border-bottom pb-20"><div class="text-right mt-10"><button class="btn btn-danger remove_attr">Remove</button></div><div class="row no-margin"><div class="col-sm-4"><div><label class="control-label">Name</label><input type="text" class="form-control"></div><div class="display-block"><div class="display-inline-block pt-20"><label class="customcheckbox no-margin-bottom " style="display:table;width: 100%;" for="allcheck5"><input type="checkbox" checked="checked" id="allcheck5"><span></span><label class="pl-10" style="display: table-cell;vertical-align: middle; padding-top: 5px;">Enable Product Option</label></label></div></div></div><div class="col-sm-8"><div><label class="control-label">Value(s)</label><textarea class="form-control" cols="5" rows="5" placeholder="Enter some text, or some attributes by | separating values."></textarea></div></div></div></div>';

        $('#add_attribute_here').append(data);

        $('.remove_attr').on('click', function () {
            $(this).parent().parent('div.display-block').remove();
        });
    });



    $('#payment').change(function (e) {
        if ($(this).val() == "recurring_monthly" || $(this).val() == "recurring_yearly") {
            $('#recurring').slideDown(200);


        } else {
            $('#recurring').slideUp(200);
        }


    });

    $("#fileToUpload").on('change', function () {
        ///// Your code
        var data = '<span class="action btn btn-danger" style="user-select: none;position: absolute;right: 95px;z-index:11">&times;</span>';
        $('#uniform-fileToUpload').append(data);

    });

});


