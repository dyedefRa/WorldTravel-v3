jQuery(document).ready(function () {
    $('.phone').mask('0(999)-999-99-99');
    $('.mobilephone').mask('0(599)-999-99-99');
    $(".datepicker").mask('99.99.9999');

    $(".mobilephone,.phone").keyup(function (e) {
        var value = $(this).val();
        if (value[0] !== "0") {
            $(this).val("0" + value.substr(1, value.length));
        }
    });
    //    $(".numeric").numeric({ negative: false, decimal: "," });
});

function showLoading() {
    $('#pre-loader').fadeIn('slow');
}

function hideLoading() {
    $('#pre-loader').fadeOut('slow');
}


toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-custom",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "5000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut",
    "progressBar": true,
    "icon": false,
    enableHtml: true
};


const AlertType = {
    success: "success",
    error: "error",
    warning: "warning",
    info: "info"
}



//After Alert <<<<<
function pageLoadAndAfterAlert_Post(storageKey, type, message) {
    var message = { 'type': type, 'message': message };
    localStorage.setItem(storageKey, JSON.stringify(message));
    window.location.reload();
}

function pageLoadAndAfterAlert_Get(storageKey) {
    var storage = JSON.parse(localStorage.getItem(storageKey));
    if (storage) {
        localStorage.removeItem(storageKey);

        switch (storage.type) {
            case AlertType.success:
                toastr.success(storage.message)
                break;
            case AlertType.error:
                toastr.error(storage.message)
                break;
            case AlertType.warning:
                toastr.warning(storage.message)
                break;
            case AlertType.info:
                toastr.info(storage.message)
                break;
            default:
        }
    }
}
//After Alert >>>>>>
