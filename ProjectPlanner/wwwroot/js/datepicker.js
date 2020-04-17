$(document).ready(function () {
    $(".datepicker").datepicker({
        dateFormat: 'DD dd M yy',
        minDate: 0,
        autoclose: true,
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        autoclose: true,
        todayHighlight: true,


    });

});