$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        //$("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        //$('#Description').val('').text('').removeClass('errormessage');
        //$('#Name').val('').text('').removeClass('errormessage');
        //$('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    //$("#lookup_div_itemtype").dialog('close');


    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Report/ReportSalesPersonnelComparisonYearlyByQty?start1Date=' + $('#Start1Date').datebox('getValue') + '&end1Date=' + $('#End1Date').datebox('getValue') + '&start2Date=' + $('#Start2Date').datebox('getValue') + '&end2Date=' + $('#End2Date').datebox('getValue') + '&start3Date=' + $('#Start3Date').datebox('getValue') + '&end3Date=' + $('#End3Date').datebox('getValue'));
    });



    function clearForm(form) {

        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
        });
    }



}); //END DOCUMENT READY