﻿$(function () {

    $('.datepicker').datepicker({
        dateFormat: "dd-mm-yy"
    });

    if ($.fn.datetimepicker != undefined) {
        $.fn.datetimepicker.dates['pl'] = {
            days: ["Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela"],
            daysShort: ["Nie", "Pn", "Wt", "Śr", "Czw", "Pt", "So", "Nie"],
            daysMin: ["N", "Pn", "Wt", "Śr", "Cz", "Pt", "So", "N"],
            months: ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"],
            monthsShort: ["Sty", "Lu", "Mar", "Kw", "Maj", "Cze", "Lip", "Sie", "Wrz", "Pa", "Lis", "Gru"],
            today: "Dzisiaj",
            weekStart: 1
        };
    }


})