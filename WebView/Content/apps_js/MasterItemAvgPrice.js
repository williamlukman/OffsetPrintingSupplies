$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode,
		currentpage = '1';
   
    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstItemAvgPrice/GetList', postData: { }, page: currentpage }).trigger("reloadGrid");
    }
    
    function ClearData() {
        ClearErrorMessage();
    }

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstItemAvgPrice/GetList',
        datatype: "json",
        colNames: ['ID', 'Sku', 'Name', 
                    'Ready', 'PendRcv', 'PendDlv', 'MIN', 'Virtual', "Cust's QTY",
                    'UoM', 'AvgPrice', "Cust's AvgPrice", 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 480 },
                  { name: 'quantity', index: 'quantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'minimumquantity', index: 'minimumquantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'virtual', index: 'virtual', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'customerquantity', index: 'customerquantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 70 },
                  { name: 'avgprice', index: 'avgprice', width: 90, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'customeravgprice', index: 'customeravgprice', width: 90, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
			      { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: currentpage,
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
            function () {
                var ids = $(this).jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    rowQuantity = $(this).getRowData(cl).quantity;
                    rowMinimumQuantity = $(this).getRowData(cl).minimumquantity;
                    if (rowMinimumQuantity - rowQuantity >= 0) {
                        $(this).jqGrid('setRowData', ids[i], false, 'fontred');
                    }

                    rowIsTradeable = $(this).getRowData(cl).istradeable;
                    if (rowIsTradeable == 'true') {
                        rowIsTradeable = "Yes";
                    } else {
                        rowIsTradeable = "No";
                    }
                    $(this).jqGrid('setRowData', ids[i], { istradeable: rowIsTradeable });
                }
            }
    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        $('input[id*="gs_"]').val("");
        $('select[id*="gs_"]').val("ALL");
        currentpage = '1';
        $("#list").setGridParam({ url: base_url + 'MstItemAvgPrice/GetList', postData: { "filters": "" }, page: currentpage }).trigger("reloadGrid");
    });
}); //END DOCUMENT READY