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
            $(this).addClass('btn btn-secondary');
            $(this).next().children(".js-checkout, .js-checkin, .js-reserve").hide();
        }

        if ($(this).text() === 'Odjezd')
        {
            $(this).addClass('btn btn-danger');
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

    // obarveni policka s registraci rezervace
    if ($('#IsRegistered').attr('checked') === "checked") { $('#isRegisteredDiv').addClass('alert alert-success'); }
    else { $('#isRegisteredDiv').addClass('alert alert-warning'); }

    // prepinani barvy pri kliknuti
    $('#IsRegistered').on("click", function () { $('#isRegisteredDiv').toggleClass('alert-warning alert-success'); });

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
            data: { ParkingPlaceName: pPName, ReservationId: resId }
        });
    });

    // obnoveni stranky po ajax callu do controlleru
    $(document).ajaxStop(function () {
        window.location.reload();
    });

    // zvyrazneni neprirazeneho mista u najezdovych rezervaci
    $('div.alert-prijezd').each(function () {
        $(this).children(".js-pPlacePrijezd:contains('Nepřiřazeno')").addClass("alert-link");
    });

    // modal pro potvrzeni checkoutu rezervace
    $(document).on("click", ".js-checkout", function (e) {
        var id = $(this).parent().attr('Id');
        var spz = $(this).parent().prev().prev().attr('data-bbox-spz'); 

        var msgCO = "<div class=\"row alert alert-warning\"><div class=\"nav-link\">Chcete ukončit pobyt?</div>" +
            "<a class=\"nav-link js-checkout\" href=\"/Parking/CheckOut?pPlaceId=" + id + "\">Check Out</a></div>";
        var msgVyjezd = "<div class=\"row alert alert-warning\"><div class=\"nav-link\">Dočasný výjezd?</div>" +
            "<a class=\"nav-link js-checkout\" href=\"/Parking/CheckOut?pPlaceId=" + id + "\">Výjezd</a></div>";

        var odjezd = $(this).parent().prev().prev().attr('data-bbox-odjezd'); 
        var d = new Date();
        var date = d.getDate();
        var month = d.getMonth()+1;
        var year = d.getFullYear();
        date = date + "._" + month + "._" + year;

        var dialog = bootbox.dialog({
            title: spz,
            message: (date === odjezd) ? msgCO : msgCO + msgVyjezd,
            buttons: {
                ok: { label: "Zavřít", className: 'btn-info' }
            }
        });
    });

    // modal window pri kliknuti na stav parkovaciho mista
    $(document).on("click", ".js-sOPChange", function (e) {

        var spz = $(this).prev().attr("data-bbox-spz").replace(/_/g," ");
        var prijezd = $(this).prev().attr("data-bbox-prijezd").replace(/_/g, " ");
        var odjezd = $(this).prev().attr("data-bbox-odjezd").replace(/_/g, " ");
        var pokoj = $(this).prev().attr("data-bbox-pokoj").replace(/_/g, " ");
        var jmeno = $(this).prev().attr("data-bbox-jmeno").replace(/_/g, " ");
        var cena = $(this).prev().attr("data-bbox-cena").replace(/_/g, " ");
        var zamestnanec = $(this).prev().attr("data-bbox-zamestnanec").replace(/_/g, " ");
        var jeRegistrovan = $(this).prev().attr("data-bbox-jeRegistrovan").replace(/_/g, " ");
        var typAuta = $(this).prev().attr("data-bbox-typAuta").replace(/_/g, " ");

        var dialog = bootbox.dialog({
            title: spz,
            message: 
                "<div class= 'container'>" + "<div class=\"row\"><div class=\"col-sm-4\" style=\"text-align:right\">" +
                "Příjezd: <br> Odjezd: <br>Pokoj: <br>" + zamestnanec +
                ": <br>Registrován?: <br>Cena: <br>Typ Auta: " +
                "</div ><div class=\"col-sm\">" + prijezd + "<br>" + odjezd + "<br>" + pokoj +
                "<br>" + jmeno + "<br>" + jeRegistrovan + "<br>" + cena + "<br>" + typAuta + "</div ></div ></div > "
                ,
            buttons: {
                
                ok: {
                    label: "Zavřít",
                    className: 'btn-info'
                }
            }
        });
    });
});