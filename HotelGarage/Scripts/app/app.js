//
// res list =>
//
var ReservationList = function () { 
    var init = function () {
        table.draw();
        $("#filterBtn").on("click", function () { table.draw(); });
    };

    // prepsani datatable s nastavenymi daty a filtrem
    var table = $('#example').DataTable({
        columnDefs: [{
            targets: [0, 1, 2],
            className: 'mdl-data-table__cell--non-numeric'
        }]
    });

    return {
        init: init
    };
}();

//
// res form =>
//
var ReservationForm = function () { 
    var init = function () {
        setUpBoxColor();
        $('.js-res-save-btn').on("click", departureAfterArrivalCheck);
        $('#IsRegistered').on("click", toggleBoxColor);
        $('input[data-id="spz-visible"]').on('keyup', passLicensePlaceIntoModel);

        var reservations = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            prefetch: {
                //url: '/api/reservations/',
                url: '/api/reservations/',
                ttl: 1 // in milliseconds
            }
        });

        // passing in `null` for the `options` arguments will result in the default
        // options being used
        $('.typeahead').typeahead(null, {
            name: 'reservations',
            source: reservations
        });
       
    };

    // kontrola prijezdu pri vytvareni rezervace - zda je odjezd az po prijezdu
    var departureAfterArrivalCheck = function (event) {
        if ($('#Arrival').val() > $('#Departure').val()) {
            alert("Odjezd musí být po příjezdu!");
            event.preventDefault();
        }
    };

    // prebarveni stavu parkovaciho mista podle stavu registrace hosta
    var setUpBoxColor = function () {
        if ($('#IsRegistered').attr('checked') === "checked") 
            $('#isRegisteredDiv').addClass('alert alert-success');
        else 
            $('#isRegisteredDiv').addClass('alert alert-warning');
    };

    // prepinani barvy pri kliknuti na checkbox
    var toggleBoxColor = function () {
        $('#isRegisteredDiv').toggleClass('alert-warning alert-success');
    };

    // doplneni SPZ z textboxu aby byl vracen kompletni viewModel
    var passLicensePlaceIntoModel = function () {
        $('input[data-id="spz-hidden"]').val($(this).val());
    };

    return {
        init: init
    };
}();

// Main parking place.
var ParkingPlace = function () {
    var init = function () {
        $('div.alert-prijezd').each(highlightReservations);
        $('a.odkaz-prijezd').each(hideCheckInButton);
        $('.select-link').on("click", assignReservationIntoParkingPlace);
        $('div.card-body>a').each(parkingPlaceLinksAndStateSetUp);
        $(".card-title").each(employeeStateSetUp);
        $(document).on("click", "a.js-checkout", checkOutModalSetup);
        $(document).on("click", ".js-sOPChange", parkingPlaceModalSetUp);

        // Refresh page after ajax call.
        $(document).ajaxStop(function () { window.location.reload(); });
    };

    // Highlight todays reservations without assigned parking place.
    var highlightReservations = function () {
        $(this).children(".js-parkingPlaceArrival:contains('Nepřiřazeno')").addClass("alert-link");
    };

    // Hide check in button for reservations without assigned parking place.
    var hideCheckInButton = function () {
        if ($(this).attr('isregistered') === 'False')
        {
            $(this).addClass("disabled");
        }
    };

    var assignReservationIntoParkingPlace = function () {
        var vId = $(this).attr('value-id');
        var btnType = $(this).attr('buttonType');

        $.ajax({
            type: "POST",
            url: "/Parking/Reserve",
            data: {
                ParkingPlaceName: $('select[value-id="' + vId + '"][buttonType="' + btnType + '"]').val(),
                ReservationId: $('button[value-id="' + vId + '"][buttonType="' + btnType + '"]').attr('data-id')
            }
        });
    };

    var btnState = function (btn, state, addClasses, hideLinks) {
        if ($(btn).text() === state) {
            $(btn).addClass(addClasses);
            $(btn).next().children(hideLinks).hide();
        }
    };

    var parkingPlaceLinksAndStateSetUp = function () {
        var showReserve = ".js-checkout, .js-checkin, .js-upravit";
        var showCOutEdit = ".js-checkin, .js-reserve";
        var showEdit = ".js-checkout, .js-checkin, .js-reserve";
        var showCInEdit = ".js-checkout, .js-reserve";

        btnState(this, 'Volno', 'btn btn-success', showReserve);
        btnState(this, 'Obsazeno', 'btn btn-primary', showCOutEdit);
        btnState(this, 'Rezervováno', 'btn btn-warning', showCInEdit);
        btnState(this, 'Neregistrován!', 'btn btn-secondary', showEdit);
        btnState(this, 'Odjezd', 'btn btn-danger', showCOutEdit);
        btnState(this, 'Volno Staff', 'btn btn-light', showReserve);
    };

    var employeeStateSetUp = function () {
        if ($(this).next().attr("data-bbox-zamestnanec") === 'Zaměstnanec'
            && ($(this).next().next().hasClass("btn-primary") === true || 
                $(this).next().next().hasClass("btn-danger") === true))
        {
            $(this).next().next().toggleClass('btn-primary btn-outline-success');
        }
    };

    var todaysDate = function () {
        var d = new Date();
        return d.getDate() + ". " + (d.getMonth() + 1) + ". " + d.getFullYear();
    };

    var getCheckOutHtml = function (id) {
        return "<div class=\"row alert alert-primary\"><div class=\"nav-link\">Chcete ukončit pobyt?</div>" +
            "<a class=\"nav-link js-checkout ml-auto\" href=\"/Parking/CheckOut?parkingPlaceId=" + id + "\">Check Out</a></div>";
    };

    var getVyjezdHtml = function (id) {
        return "<div class=\"row alert alert-primary\"><div class=\"nav-link\">Dočasný výjezd?</div>" +
            "<a class=\"nav-link js-checkout ml-auto\" href=\"/Parking/TemporaryLeave?parkingPlaceId=" + id + "\">Výjezd</a></div>";
    };


    var checkOutModalSetup = function (e) {
        if ($(this).attr('isregistered') === 'False') 
            return false;
        else {
            var id = $(this).parent().attr('Id');
            var spz = $(this).hasClass("odkaz-prijezd") ?
                $(this).attr('data-bbox-spz') :
                $(this).parent().prev().prev().attr('data-bbox-spz');
            var odjezd = $(this).parent().prev().prev().attr('data-bbox-odjezd');

            bootbox.dialog({
                title: "SPZ:  " + spz,
                message: (todaysDate() === odjezd) ? getCheckOutHtml(id) : getCheckOutHtml(id) + getVyjezdHtml(id),
                buttons: {
                    ok: { label: "Zavřít", className: 'btn-primary' }
                }
            });
        }
    };

    var parkingPlaceModalSetUp = function (e) {

        var getDataAttr = function (elem, attribute) {
            return $(elem).prev().attr(attribute);
        };

        var spz = getDataAttr(this, "data-bbox-spz");
        var prijezd = getDataAttr(this, "data-bbox-prijezd");
        var odjezd = getDataAttr(this, "data-bbox-odjezd");
        var pokoj = getDataAttr(this, "data-bbox-pokoj");
        var jmeno = getDataAttr(this, "data-bbox-jmeno");
        var cena = getDataAttr(this, "data-bbox-cena");
        var zamestnanec = getDataAttr(this, "data-bbox-zamestnanec");
        var jeRegistrovan = getDataAttr(this, "data-bbox-jeRegistrovan");
        var typAuta = getDataAttr(this, "data-bbox-typAuta");
        var poznamka = getDataAttr(this, "data-bbox-poznamka");

        bootbox.dialog({
            title: spz,
            message:
                "<div class= 'container'>" + "<div class=\"row\"><div class=\"col-sm-4\" style=\"text-align:right\">" +
                "Příjezd: <br> Odjezd: <br>Pokoj: <br>" + zamestnanec +
                ": <br>Registrován: <br>Cena: <br>Typ Auta: <br>Poznámka: " +
                "</div ><div class=\"col-sm\">" + prijezd + "<br>" + odjezd + "<br>" + pokoj +
                "<br>" + jmeno + "<br>" + jeRegistrovan + "<br>" + cena + "<br>" + typAuta + "<br>" +
                poznamka + "</div ></div ></div > "
            ,
            buttons: {

                ok: {
                    label: "Zavřít",
                    className: 'btn-info'
                }
            }
        });
    };

    return {
        init: init
    };
}();
