$(document).ready(function () {

    $('div.card-body>a').each(function () {
        if ($(this).text() === 'Volno')$(this).addClass('btn btn-success'); 
        if($(this).text() === 'Obsazeno')$(this).addClass('btn btn-primary');
        if($(this).text() === 'Rezervováno')$(this).addClass('btn btn-warning');
        if($(this).text() === 'Neregistrován!')$(this).addClass('btn btn-danger');
        if($(this).text() === 'Odjezd')$(this).addClass('btn btn-warning');
        if($(this).text() === 'Volno Staff')$(this).addClass('btn btn-light');
    });

    $('input[data-id="spz-visible"]').on('keyup', function () {
        $('input[data-id="spz-hidden"]').val($(this).val());
    });

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
            success: function (data) {
                alert("poslano");
            },
            error: function (data) {
                alert("posrano");
            }
        });

    });

       

    $(document).ajaxStop(function () {
        window.location.reload();
    });

    $(document).on("click", ".js-sOPChange", function (e) {
        var dialog = bootbox.dialog({
            title: 'Vyberte najíždějící rezervaci anebo vytvořte novou.',
            message: "<p>This dialog has buttons. Each button has it's own callback function.</p>",
            buttons: {
                cancel: {
                    label: "cancel!",
                    className: 'btn-danger',
                    callback: function () {
                        Example.show('Custom cancel clicked');
                        return;
                    }
                },
                noclose: {
                    label: "I don't close modal!",
                    className: 'btn-warning',
                    callback: function () {
                        Example.show('Custom button clicked');
                        return false;
                    }
                },
                ok: {
                    label: "custom OK!",
                    className: 'btn-info',
                    callback: function () {
                        Example.show('Custom OK clicked');
                    }
                }
            }
        });
    });

});