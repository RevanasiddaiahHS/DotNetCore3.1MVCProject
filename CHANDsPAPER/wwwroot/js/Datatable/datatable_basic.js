$(function () {

$(document).on('click', '.dataTable .dropdown-toggle', function () {
    $('body').addClass('dropDownOpenCustom')
});

$(document).ready(function(){
    $('body').on('hide.bs.dropdown', function () {
        $('body').removeClass('dropDownOpenCustom');
    });
});
    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        dom: '<"datatable-header"f><"datatable-scroll"t><"datatable-footer"ilp>',
        language: {
            search: '<span>' + "Filter" + ':</span> _INPUT_',
            searchPlaceholder: '' + "Type_to_filter" + '...',
            lengthMenu: '<span>' + "Show" + ':</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&rarr;', 'previous': '&larr;' }
        },
        //language: {
        //    search: '<span>Filter:</span> _INPUT_',
        //    searchPlaceholder: 'Type to filter...',
        //    lengthMenu: '<span>Show:</span> _MENU_',
        //    paginate: {
        //        'first': 'First','last': 'Last','next': '&rarr;','previous': '&larr;'
        //    }
        //},
        drawCallback: function () {
            if($(this).find('tbody tr').length > 3){
                $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
            }
        },
        preDrawCallback: function() {
            if($(this).find('tbody tr').length > 3){
                $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
            }
        }
    });


    // Default ordering example
    $('.datatable-basic').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [
        {
                width: '75%',
                targets: [0]
        },
        {
            orderable: false,
            targets:[2]

        }
        
        ],

    });

    $('.datatable-basic-product').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [
        //    {
        //        width:'20%',
        //        targets: [0]
        //},
        //{
        //    orderable: false,
        //    targets:[2]

        //}
        
        ],
    });

$('.datatable-basic-report').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [],
    });

$('.datatable-basic-order').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [{
            orderable: false,
            targets: [5]
        }],
    });

$('.datatable-basic-management').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [{
            orderable: false,
            targets: [4]
        }],
    });
$('.datatable-basic-customer').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [{
            orderable: false,
            targets: [4]
        }],
    });

$('.datatable-basic-payment_history').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [],
        scrollX: true
    });

$('.datatable-basic-order_history').DataTable({
        pageLength: 50,
        order: [],
        columnDefs: [{
            orderable: false,
            targets: [6]
        }],
        scrollX: true
    });
     // Enable Select2 select for the length option
    $('.dataTables_length select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });
});