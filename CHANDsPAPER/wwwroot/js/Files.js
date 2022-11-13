$(document).ready(function () {

    var datatable = $("#localfileslist").dataTable();
    datatable.fnDestroy();
    //bindLocalfilesList();
});

function bindLocalfilesList() {

    var mentorid = $("#hdnmentorid").val();
    var startdate = Cookies.get("customrangestartdate")
    var enddate = Cookies.get("customrangeenddate")
    if (startdate == undefined) {
        startdate = "";
    }
    if (enddate == undefined) {
        enddate = "";
    }
    var table = $("#localfileslist").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "searchable": true,
        "order": [[0, "asc"]],
        "stateSave": false,
        "ajax": {
            type: "Post",
            dataType: "Json",
            data: function () {
                var info = $('#localfileslist').DataTable().page.info();
                var TemplateStr = 'pageNumber=' + (info.page + 1);
                if ($('#localfileslist_length select').val() > 0) {
                    TemplateStr += '&pagesize=' + $('#localfileslist_length select').val();
                } else {
                    TemplateStr += '&pageSize=100';
                }
                TemplateStr += '&mentorid=' + mentorid;
                TemplateStr += '&startdate=' + startdate;
                TemplateStr += '&enddate=' + enddate;
                $('#localfileslist').DataTable().ajax.url(
                    '/Mentor/GetLocalFilesList?' + TemplateStr
                );
            },
            "dataSrc": function (json) {

                return json.data;
            }
        },
        "columns": [
            {
                "data": "fileName", "name": "fileName",
            },
            {
                "data": "uploadeddate", "name": "uploadeddate", "className": "text-center", orderable: false, sorting: false,
            },
            {
                "data": "islive", "name": "islive", "className": "text-center",
                "render": function (data, type, row, meta) {
                    var Action = '';
                    if (row.islive == 0) {
                        Action = "<span class='bg-slate label'>No</span>";
                    }
                    else {
                        Action = "<span class='bg-yes label'>Yes</span>"
                    }
                    return Action;

                }                
            },           
            {
                "data": "Action", "name": "Action", "className": "text-center", orderable: false, sorting: false,
                "render": function (data, type, row, meta) {
                    var Action = '';
                    //if (row.numberOfFunnels != 0) {
                    //    $("#hdn_funnellimit").val(row.numberOfFunnels);
                    //}
                    //if (row.totalAmount > 0) {
                    //    Action += number_format(row.totalAmount);
                    //}
                    //else {
                    //    Action += '-';
                    //}
                    Action += '<a class="" title="download"  href="/Mentor/Downloadfile?filename=' +row.fileuniquepath+'"><i class="icon-file-download2 mr-10"></i></a>';
                    if (row.islive == 0) {
                        Action += '<a class="" title="Post" onclick="MovetoLive()"><i class="icon-move"></i></a>';

                        Action += '<a class=" text-danger"  title="delete"  onclick="deletefile(' + row.fileuniqueID + ',' + mentorid + ',\'' + row.fileuniquepath+'\')"><i class="icon-trash ml-10"></i></a>';
                    }
                    return Action;

                } 
            }
        ]
    });
    selectWithoutSearchAutoWidth();
}

function selectWithoutSearchAutoWidth() {
    $('.dataTables_length select, .calendar-time select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });
}

function deletefile(fileuniqueid, mentorid, fileuniquepath) {


    swal({
        title: "Delete",
        text: "Are you sure you want to delete this file?",
        showCancelButton: true,
        confirmButtonColor: "#EF5350",
        confirmButtonText: "Ok",
        cancelButtonText: "Cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "get",
                    dataType: "Json",
                    url: '/Mentor/DeletefileLocal?fileuniqueid=' + fileuniqueid + '&mentorid=' + mentorid + '&fileuniquepath=' + fileuniquepath,
                    success: function (data) {
                        window.location.href ='/Mentor/LocalFiles';
                    }
                })
            }
        });
}

