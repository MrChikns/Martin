$(document).ready(function () {
      
    // prepsani stavu parkovaciho mista a skryti nepotrebnych akci pro stav parkovaciho mista
    $('div.card-body>a').each(function () {
        if ($(this).text() === 'Volno')
        {            
            $(this).addClass('btn btn-success');
            $(this).next().children(".js-checkout, .js-checkin, .js-upravit").hide();
        }

        if ($(this).text() === 'Obsazeno')
        {
            $(this).addClass('btn btn-primary');
            $(this).next().children(".js-checkin, .js-reserve").hide();
        }

        if ($(this).text() === 'Rezervováno')
        {
            $(this).addClass('btn btn-warning');
            $(this).next().children(".js-checkout, .js-reserve").hide();
        }

        if ($(this).text() === 'Neregistrován!')
        {
            $(this).addClass('btn btn-danger');
            $(this).next().children(".js-checkout, .js-checkin, .js-reserve").hide();
        }

        if ($(this).text() === 'Odjezd')
        {
            $(this).addClass('btn btn-warning');
            $(this).next().children(".js-checkin, .js-reserve").hide();
        }

        if ($(this).text() === 'Volno Staff')
        {
            $(this).addClass('btn btn-light');
            $(this).next().children(".js-checkout, .js-checkin, .js-upravit").hide();
        }
    });

    // zvyrazneni neprirazeneho mista u najizdejici rezervace
    $('div.alert-prijezd').each(function () {
        $(this).children(".js-pPlacePrijezd:contains('Nepřiřazeno')").addClass("alert-link");
    });

    // vyplneni SPZ z jineho okna aby byl vracen kompletni viewModel
    $('input[data-id="spz-visible"]').on('keyup', function () {
        $('input[data-id="spz-hidden"]').val($(this).val());
    });

    // prirazeni najizdejici reservace na parkovaci misto
    $('.select-link').on("click",function () {
        //reservation id
        var vId = $(this).attr('value-id');
        var resId = $('button[value-id="' + vId + '"]').attr('data-id');
        var pPName = $('select[value-id="' + vId + '"]').val();
        url = '@Url.Action("Reserve", "Parking")';
        var model = { ParkingPlaceName: pPName, ReservationId: resId };
        $.ajax({
            type: "POST",
            url: "/Parking/Reserve",
            data: { ParkingPlaceName: pPName, ReservationId: resId },

        });
    });

    // obnoveni stranky po ajax callu do controlleru
    $(document).ajaxStop(function () {
        window.location.reload();
    });


    $('div.alert-prijezd').each(function () {
        $(this).children(".js-pPlacePrijezd:contains('Nepřiřazeno')").addClass("alert-link");
    });
    // SMAZAT? modal window pri kliknuti na stav parkovaciho mista
    $(document).on("click", ".js-sOPChange", function (e) {

        var spz = $(this).prev().attr("data-bbox-spz");
        var prijezd = $(this).prev().attr("data-bbox-prijezd");
        var odjezd = $(this).prev().attr("data-bbox-odjezd");
        var pokoj = $(this).prev().attr("data-bbox-pokoj");
        var jmeno = $(this).prev().attr("data-bbox-jmeno");
        var cena = $(this).prev().attr("data-bbox-cena");
        var zamestnanec = $(this).prev().attr("data-bbox-zamestnanec");
        var typAuta= $(this).prev().attr("data-bbox-typAuta");

        var dialog = bootbox.dialog({
            title: 'SPZ' + spz,
            message: "Příjezd: " + prijezd +
                "<br>Odjezd: " + odjezd +
                "<br>Pokoj: " + pokoj +
                "<br>"+ zamestnanec + ": " + jmeno +
                "<br>Cena: " + cena +
                "<br>Zaměstnanec: " + zamestnanec +
                "<br>Typ Auta: " + typAuta,
            buttons: {
                
                ok: {
                    label: "Zavřít",
                    className: 'btn-info',                    
                }
            }
        });

        
    });

});