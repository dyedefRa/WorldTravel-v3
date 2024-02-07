$(window).on('load', function () {
    $('.dataTables_scrollBody').on('show.bs.dropdown', function () {
        $('.dataTables_scrollBody').css("overflow", "inherit");
    });

    $('.dataTables_scrollBody').on('hide.bs.dropdown', function () {
        $('.dataTables_scrollBody').css("overflow", "auto");
    });
});

// Bir modal açıldığında.
$(window).on('shown.bs.modal', function () {
    $.BlockUI.hide();
    $.Format.phoneFormat();
});

$(document).ready(function () {
    $('.custom-money').mask('000.000.000.000.000,00', { reverse: true });
    $('.custom-number').mask('#', { reverse: true });
    $('.custom-telephone').mask('0(000) 000 00 00', { reverse: true });
    $('.custom-taxId').mask('0000000000', { reverse: true });

    var elems = Array.prototype.slice.call(document.querySelectorAll('.form-check-input-switchery'));
    elems.forEach(function (html) {
        var switchery = new Switchery(html);
    });

    $('select.select-search').select2({
        theme: "bootstrap"
    });

    $('select.multiselect-select-all-filtering').multiselect({
        includeSelectAllOption: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true
    });


    $.Format.phoneFormat();
});

$.MONEYFORMAT = {

    formatTurkishMoney: function (price) {
        var currency_symbol = "TL"

        var formattedOutput = new Intl.NumberFormat('tr-TR', {
            style: 'currency',
            currency: 'TRY',
            minimumFractionDigits: 2,
        });

        return formattedOutput.format(price).replace(currency_symbol, '')
    }

};

$.BlockUI = {

    show: function () {
        $.blockUI({
            message: '<i class="icon-spinner4 spinner"></i>',
            overlayCSS: {
                backgroundColor: '#1b2024',
                opacity: 0.8,
                cursor: 'wait'
            },
            css: {
                border: 0,
                color: '#fff',
                padding: 0,
                backgroundColor: 'transparent'
            }
        });
    },
    hide: function () {
        $.unblockUI();
    }

};

$.Format = {

    phoneFormat: function () {
        $('.phone-format').formatter({
            pattern: '0({{999}}) {{999}} {{9999}}'
        });

        $('.taxId-format').formatter({
            pattern: '{{9999999999}}'
        });

        $('.discount-format').formatter({
            pattern: '{{999}},{{99}}'
        });

    }

};

$.City = {

    getCity: function (cityId) {
        var city = cities.filter(function (index) {
            if (index.value === cityId) {
                return index.text;
            }
        });

        return city[0].text;
    }
}

$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(".", ",");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}
$.validator.methods.number = function (value, element) {
    return this.optional(element) || /-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}


//------------------------------------------------

function isNumber(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}


$('.accordion-toggle').click(function () {
    $i = $(this).find("i");
    var className = $i.attr('class');
    if (className === 'icon-plus2') {
        $i.removeClass('icon-plus2');
        $i.addClass('icon-minus2');
    }
    else if (className === 'icon-minus2') {
        $i.removeClass('icon-minus2');
        $i.addClass('icon-plus2');
    }
});


//function GetNormalizeDate(date) {
//    if (date === '') {
//        return '01.01.2000'; //default date
//    }
//    var arr = date.split('.');
//    arrayMove(arr, 1, 0);
//    return arr.join('.');
//}

//function arrayMove(arr, fromIndex, toIndex) {
//    var element = arr[fromIndex];
//    arr.splice(fromIndex, 1);
//    arr.splice(toIndex, 0, element);
//}

function applyShortening(value) {
    if (value === null || value === '' || value === 0) {
        return '-';
    }
    if (value.length > 50) {
        return value.substring(0, 50) + ' ...';
    }
    return value;
}

//function checkNullable(value) {
//    if (value === null || value === '' || value === 0) {
//        return '-'
//    }
//    return value;
//}

//TODOO Buraya default image gelmelı
function applyImage(value) {
    if (value === null || value === '' || value === 0) {
        return '-';
    }
    return '<img  src= "' + value + '"  class="form-group" width="100" />';
}

//TODO DINAMAIK YAP!!!
function applyImage_h(value) {
    if (value === null || value === '' || value === 0) {
        return '-';
    }
    return '<img  src= "' + value + '"  class="form-group" width="110" height="70" />';
}

//function IsDateBiggerThanCurrentDate(date) {
//    if (date >= new Date()) {
//        return true;
//    }
//    else {
//        return false;
//    }
//}


function setImage100(image) {
    if (image === null || image === '') {
        return ''; //TODOO dfeult resim ekle.
    }
    return '<img  src= "' + image + '"  class="form-group" width="100" style="margin: 0 auto;" />';
};

function setDate(data) {
    if (data === null) {
        return '-';
    }
    return luxon
        .DateTime
        .fromISO(data, {
            locale: abp.localization.currentCulture.name
        }).toLocaleString(luxon.DateTime.DATE_MED);
    //.toLocaleString(luxon.DateTime.DATETIME_SHORT);
};

function setBoolean(data) {
    if (data === null) {
        return '-';
    }
    else if (data === true) {
        return 'Evet';
    }
    return 'Hayır';
};

