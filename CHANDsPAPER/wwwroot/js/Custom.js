function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function ChandsLogout() {

    $.ajax({
        type: "GET",
        url: "/Login/Logout",
        dataType: "JSON",
        success: function (response) {
            //window.history.go(-window.history.length);

            window.location.href = '/Login/Index';
        }
    });
}


function summerNoteInit() {
    $('.templatedata  .summernote-airmode').summernote({
        //airMode: true,
        fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Helvetica', 'Impact', 'Roboto', 'Tahoma', 'Times New Roman', 'Verdana'],
        fontSizes: ['2', '4', '6', '8', '10', '12', '14', '16', '18', '20', '22', '24', '26', '28', '30', '32', '34', '36', '38', '40', '42', '44', '46', '48', '50', '52', '54', '56', '58', '60', '62', '64', '66', '68', '70', '72', '74', '76', '78', '80', '82', '84', '86', '88', '90', '92', '94', '96', '98', '100'],
        popover: {
            /*image: [
                //['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                ['float', ['floatLeft', 'floatRight', 'floatNone']],
                ['remove', ['removeMedia']]
            ],*/
            link: [
                ['link', ['linkDialogShow', 'unlink']]
            ],
            air: [
                ['fontname', ['fontname']],
                ['fontsize', ['fontsize']],
                ['font', ['bold', 'italic', 'underline', 'strikethrough', 'height']],
                ['color', ['color']],
                ['para', ['ul', 'paragraph']],
                ['insert', ['link']]
            ]
        },

        dialogsInBody: true,

        shortcuts: false,
        disableDragAndDrop: true,
        callbacks: {
            onMouseup: function (contents) {
                if (contents.target.classList.contains('dragResizable')) {
                    $('.editormoveDelete').remove();
                    if (contents.target.classList.contains('dynamichtml')) {
                        $(contents.target).append(addHtmlMoveDelete);
                    }
                    else {
                        $(contents.target).append(addMoveDelete);
                    }
                }
            },
            onBlur: function () {
                $('.editormoveDelete').remove();
            },
            onChange: function (contents) {
                //console.log('onChange:', contents);
            }
        }
    });
}
/* ------------------------------------------------------------------------------
*
*  # Summernote editor
*
*  Specific JS code additions for editor_summernote.html page
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */

$(function () {


    // Basic editors
    // ------------------------------

    // Default initialization
   // $('.summernote').summernote();


    // Control editor height
    

  

});