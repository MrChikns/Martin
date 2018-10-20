$(document).ready(function () {

    $('div.card-body>a').each(function () {
        if ($(this).text() === 'Volno')$(this).addClass('btn btn-success'); 
        if($(this).text() === 'Obsazeno')$(this).addClass('btn btn-primary');
        if($(this).text() === 'Rezervováno')$(this).addClass('btn btn-warning');
        if($(this).text() === 'Neregistrován!')$(this).addClass('btn btn-danger');
        if($(this).text() === 'Odjezd')$(this).addClass('btn btn-warning');
        if($(this).text() === 'Volno Staff')$(this).addClass('btn btn-light');
    });

    $(document).on("click", ".js-sOPChange", function (e) {
        bootbox.alert("Hello world!", function () {
            console.log("Alert Callback");
        });
    });

});