// Filter a column in datatable based on a specific date range and depending on the selected radio button

$.fn.dataTableExt.afnFiltering.push(
    function (oSettings, aData, iDataIndex) {
        var startDateFilter = document.getElementById('min').value;
        var endDateFilter = document.getElementById('max').value;
        

        startDateFilter = startDateFilter.substring(0, 4) + startDateFilter.substring(5, 7) + startDateFilter.substring(8, 10);
        endDateFilter = endDateFilter.substring(0, 4) + endDateFilter.substring(5, 7) + endDateFilter.substring(8, 10);
        var iStartDateCol = 1;
        var iEndDateCol = 2;

        var arrivalDate = aData[iStartDateCol].substring(0, 4) + aData[iStartDateCol].substring(5, 7) + aData[iStartDateCol].substring(8, 10);
        var departureDate = aData[iEndDateCol].substring(0, 4) + aData[iEndDateCol].substring(5, 7) + aData[iEndDateCol].substring(8, 10);

        if ($("#optionArrival").hasClass('active')) {
            if (startDateFilter === "" && endDateFilter === "") {
                return true;
            }
            else if (startDateFilter <= arrivalDate && endDateFilter === "") {
                return true;
            }
            else if (endDateFilter >= departureDate && startDateFilter === "") {
                return true;
            }
            else if (arrivalDate >= startDateFilter && arrivalDate <= endDateFilter) {
                return true;
            }
        }

        if ($("#optionDeparture").hasClass('active')) {
            if (startDateFilter === "" && endDateFilter === "") {
                return true;
            }
            else if (startDateFilter <= arrivalDate && endDateFilter === "") {
                return true;
            }
            else if (endDateFilter >= departureDate && startDateFilter === "") {
                return true;
            }
            else if (departureDate >= startDateFilter && departureDate <= endDateFilter) {
                return true;
            }
        }

        if ($("#optionStay").hasClass('active')) {
            if (startDateFilter === "" && endDateFilter === "") {
                return true;
            }
            else if (startDateFilter <= arrivalDate && endDateFilter === "") {
                return true;
            }
            else if (endDateFilter >= departureDate && startDateFilter === "") {
                return true;
            }
            else if (startDateFilter <= arrivalDate && endDateFilter >= departureDate) {
                return true;
            }
        }
                
        return false;
    }
);
