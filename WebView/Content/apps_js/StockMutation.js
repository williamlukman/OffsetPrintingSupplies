﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'StockMutation/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'StockMutation/GetListByDate', postData: { filters: null, startdate: StartDate, enddate: EndDate }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#search_div").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'StockMutation/GetList',
        datatype: "json",
        colNames: ['ID', 'Item Id', 'Item Sku', 'Name',
                   'Warehouse Id', 'Warehouse', 'Warehouse Item Id',
                   'Ready', 'PendReceival', 'PendDelivery', 'Virtual', 'UoM', 'Source', 'Id', 'Detail Source', 'Detail Id',
                   'Created At', 'Mutation Date'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
    			  { name: 'itemid', index: 'itemid', width: 35, align: "center", hidden: true},
                  { name: 'itemsku', index: 'itemsku', width: 70 },
				  { name: 'item', index: 'item', width: 480 },
				  { name: 'warehouseid', index: 'warehouseid', width: 70, hidden: true },
				  { name: 'warehouse', index: 'warehouse', width: 150 },
				  { name: 'warehouseitemid', index: 'warehouseitemid', width: 120, hidden: true },
                  { name: 'ready', index: 'quantity', width: 70, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'virtual', index: 'virtual', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 40 },
				  { name: 'sourcedocumenttype', index: 'sourcedocumenttype', width: 140, align: 'right' },
				  { name: 'sourcedocumentid', index: 'sourcedocumentid', width: 55 },
				  { name: 'sourcedocumentdetailtype', index: 'sourcedocumentdetailtype', width: 180, align: 'right' },
				  { name: 'sourcedocumentdetailid', index: 'sourcedocumentdetailid', width: 55 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'mutationdate', index: 'mutationdate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });


    $('#btn_search').click(function () {
        $('#StartDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EndDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $("#search_div").dialog("open");
    });

    $('#search_btn_submit').click(function () {
        ClearErrorMessage();
        ReloadGridByDate($('#StartDate').datebox('getValue'), $('#EndDate').datebox('getValue'));
        $("#search_div").dialog('close');
    });

    $('#search_btn_cancel').click(function () {
        $('#search_div').dialog('close');
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
            if ($(this).hasClass('easyui-numberbox'))
                $(this).numberbox('clear');
        });
    }
}); //END DOCUMENT READY