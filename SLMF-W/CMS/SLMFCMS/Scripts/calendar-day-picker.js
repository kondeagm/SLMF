$(function () {
    $.datepicker.regional['es'] = {
        clearText: '-', clearStatus: '',
        closeText: 'Cerrar', closeStatus: '',
        prevText: '&#x3c;Ant', prevStatus: '',
        prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: '',
        nextText: 'Sig&#x3e;', nextStatus: '',
        nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: '',
        currentText: 'Hoy', currentStatus: '',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        yearStatus: '', monthStatus: '',
        weekText: 'Sm', weekStatus: '', weekHeader: 'Sm',
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'S&aacute;'],
        todayText: 'Hoy', todayStatus: '',
        dayStatus: 'DD d MM',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        defaultStatus: '',
        initStatus: 'Seleccione la Fecha', isRTL: false
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);
    function InitializeDateStart() {
        var parts = $('#Fecha').val().split('/');
        var fecha = new Date(parts[2], parts[1] - 1, parts[0]);
        return (fecha);
    };
    $('.day-picker-star').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        defaultDate: InitializeDateStart(),
        onSelect: function (dateText, inst) {
            var date = $(this).datepicker('getDate');
            var dateFormat = inst.settings.dateFormat || $.datepicker._defaults.dateFormat;
            $('#Fecha').val($.datepicker.formatDate(dateFormat, date, inst.settings));
        }
    });
});