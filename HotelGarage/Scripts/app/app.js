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
    };

    // kontrola prijezdu pri vytvareni rezervace - zda je odjezd az po prijezdu
    var departureAfterArrivalCheck = function (event) {
        if ($('#Arrival').val() > $('#Departure').val()) {
            alert("Datum odjezdu musí být po datu příjezdu!");
            event.preventDefault();
        };
    };

    // prebarveni stavu parkovaciho mista podle stavu registrace hosta
    var setUpBoxColor = function () {
        if ($('#IsRegistered').attr('checked') === "checked") { $('#isRegisteredDiv').addClass('alert alert-success'); }
        else { $('#isRegisteredDiv').addClass('alert alert-warning'); }
    };

    // prepinani barvy pri kliknuti na checkbox
    var toggleBoxColor = function () { $('#isRegisteredDiv').toggleClass('alert-warning alert-success'); };

    // doplneni SPZ z textboxu aby byl vracen kompletni viewModel
    var passLicensePlaceIntoModel = function () {
        $('input[data-id="spz-hidden"]').val($(this).val());
    };

    return {
        init: init
    };
}();


//
// main parking place =>
//
var ParkingPlace = function () {
    var init = function () {
        $('div.alert-prijezd').each(highlightTodaysReservationWithoutPPlaceAssigned);
        $('a.odkaz-prijezd').each(hideCheckInBtnForReservationWithoutPPlaceAssigned);
        $('.select-link').on("click", assignReservationIntoParkingPlace);
        $('div.card-body>a').each(parkingPlaceLinksAndStateSetUp);
        $(".card-title").each(employeeStateSetUp);
        $(document).on("click", "a.js-checkout", checkOutModalSetup);
        $(document).on("click", ".js-sOPChange", parkingPlaceModalSetUp);

        // obnoveni stranky po ajax callu do controlleru
        $(document).ajaxStop(function () { window.location.reload(); });
    };

    var highlightTodaysReservationWithoutPPlaceAssigned = function () {
        $(this).children(".js-pPlacePrijezd:contains('Nepřiřazeno')").addClass("alert-link")
    };


     var hideCheckInBtnForReservationWithoutPPlaceAssigned = function () {
        if ($(this).attr('isregistered') === 'False') {
            $(this).addClass("disabled");
        }
    };

    var assignReservationIntoParkingPlace = function () {
        var vId = $(this).attr('value-id');
        var btnType = $(this).attr('buttonType');
        var resId = $('button[value-id="' + vId + '"][buttonType="' + btnType + '"]').attr('data-id');
        var pPName = $('select[value-id="' + vId + '"][buttonType="' + btnType + '"]').val();

        url = '@Url.Action("Reserve", "Parking")';
        var model = { ParkingPlaceName: pPName, ReservationId: resId };
        $.ajax({
            type: "POST",
            url: "/Parking/Reserve",
            data: { ParkingPlaceName: pPName, ReservationId: resId }
        });
    };

    var parkingPlaceLinksAndStateSetUp = function () {
        var btnState = function (btn, state, addClasses, hideLinks) {
            if ($(btn).text() === state) {
                $(btn).addClass(addClasses);
                $(btn).next().children(hideLinks).hide();
            }
        };

        var leaveReserve = ".js-checkout, .js-checkin, .js-upravit";
        var leaveCOutEdit = ".js-checkin, .js-reserve";
        var leaveEdit = ".js-checkout, .js-checkin, .js-reserve";
        var leaveCInEdit = ".js-checkout, .js-reserve";

        btnState(this, 'Volno', 'btn btn-success', leaveReserve);
        btnState(this, 'Obsazeno', 'btn btn-primary', leaveCOutEdit);
        btnState(this, 'Rezervováno', 'btn btn-warning', leaveCInEdit);
        btnState(this, 'Neregistrován!', 'btn btn-secondary', leaveEdit);
        btnState(this, 'Odjezd', 'btn btn-danger', leaveCOutEdit);
        btnState(this, 'Volno Staff', 'btn btn-light', leaveReserve);
    } ;

    var employeeStateSetUp = function () {
        if ($(this).next().attr("data-bbox-zamestnanec") === 'Zaměstnanec'
            && $(this).next().next().hasClass("btn-primary") === true) {
            $(this).next().next().toggleClass('btn-primary btn-outline-success');
        }
    };

    var checkOutModalSetup = function (e) {
        if ($(this).attr('isregistered') === 'False') {
            return false;
        }
        else {
            var id = $(this).parent().attr('Id');
            var spz = "SPZ:  " + $(this).hasClass("odkaz-prijezd") ? $(this).attr('data-bbox-spz') :
                $(this).parent().prev().prev().attr('data-bbox-spz');
            var odjezd = $(this).parent().prev().prev().attr('data-bbox-odjezd');

            var msgCheckOut = "<div class=\"row alert alert-primary\"><div class=\"nav-link\">Chcete ukončit pobyt?</div>" +
                "<a class=\"nav-link js-checkout\" href=\"/Parking/CheckOut?pPlaceId=" + id + "\">Check Out</a></div>";
            var msgVyjezd = "<div class=\"row alert alert-primary\"><div class=\"nav-link\">Dočasný výjezd?</div>" +
                "<a class=\"nav-link js-checkout\" href=\"/Parking/TemporaryLeave?pPlaceId=" + id + "\">Výjezd</a></div>";

            var d = new Date();
            var date = d.getDate();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            date = date + ". " + month + ". " + year;

            var dialog = bootbox.dialog({
                title: spz,
                message: date === odjezd ? msgCheckOut : msgCheckOut + msgVyjezd,
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

        var dialog = bootbox.dialog({
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
