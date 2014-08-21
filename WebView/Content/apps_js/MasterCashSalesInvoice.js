$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstCashSalesInvoice/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Code').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_cashbank").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstCashBankInvoice/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Description', 'Sales Date', 'Due Date',
                   'Discount', 'Tax', 'Allowance', 'Is Confirmed', 'Confirmation Date', 'Amount Paid',
                   'CashBank ID', 'CashBank Name', 'Is Bank', 'Is Paid', 'Is Full Payment',
                   'Total', 'CoGS', 'Warehouse ID', 'Warehouse Name',
                   'Is Deleted', 'Deleted At', 'Created At', 'Updated At', 'CashSalesInvoiceDetails', 'CashSalesReturns'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
                  { name: 'description', index: 'description', width: 100 },
                  { name: 'salesdate', index: 'salesdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'duedate', index: 'duedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'discount', index: 'discount', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'tax', index: 'tax', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency' },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'amountpaid', index: 'amountpaid', width: 80, formatter: 'currency' },
                  { name: 'cashbankid', index: 'cashbankid', width: 80 },
                  { name: 'cashbank', index: 'cashbank', width: 100 },
                  { name: 'isbank', index: 'isbank', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'ispaid', index: 'ispaid', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'isfullpayment', index: 'isfullpayment', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'total', index: 'total', width: 80, formatter: 'currency' },
                  { name: 'cogs', index: 'cogs', width: 80, formatter: 'currency' },
                  { name: 'warehouseid', index: 'warehouseid', width: 80 },
                  { name: 'warehouse', index: 'warehouse', width: 100 },
                  { name: 'isdeleted', index: 'isdeleted', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'deletedat', index: 'deletedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'cashsalesinvoicedetails', index: 'cashsalesinvoicedetails', width: 80 },
                  { name: 'cashsalesreturns', index: 'cashsalesreturns', width: 80 },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		      //var ids = $(this).jqGrid('getDataIDs');
		      //for (var i = 0; i < ids.length; i++) {
		      //    var cl = ids[i];
		      //    rowDel = $(this).getRowData(cl).deletedimg;
		      //    if (rowDel == 'true') {
		      //        img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";

		      //    } else {
		      //        img = "";
		      //    }
		      //    $(this).jqGrid('setRowData', ids[i], { deletedimg: img });
		      //}
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

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "MstGetSalesInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Description').val(result.Description);
                            $('#SalesDate').datepicker('setValue', (result.SalesDate));
                            $('#DueDate').datepicker('setValue', (result.DueDate));
                            $('#Discount').numberbox('setValue', (result.Quantity));
                            $('#Tax').numberbox('setValue', (result.Quantity));
                            $('#Allowance').numberbox('setValue', (result.Quantity));
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBankName').val(result.CashBank);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#WarehouseName').val(result.Warehouse);
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //if (ret.deletedimg != '') {
            //    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
            //    return;
            //}
            $('#delete_confirm_btn_submit').data('Id', ret.id);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "MstCashSalesInvoice/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
            }),
            success: function (result) {
                ReloadGrid();
                $("#delete_confirm_div").dialog('close');
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {



        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'MstCashSalesInvoice/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstCashSalesInvoice/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Code: $("#Code").val(), Description: $("#Description").val(),
                SalesDate: $("#SalesDate").val(), DueDate: $("#DueDate").val(), Discount: $("#Discount").val(),
                Tax: $('#Tax').val(), Allowance: $('#Allowance').val(),
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.model.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.model.Errors[key], 'warning');
                        }
                    }
                    //var error = '';
                    //for (var key in result.model.Errors) {
                    //    error = error + "<br>" + key + " "+result.model.Errors[key];
                    //}
                    //$.messager.alert('Warning',error, 'warning');
                }
                else {
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
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

    // -------------------------------------------------------Look Up CashBank-------------------------------------------------------
    $('#btnCashBank').click(function () {

        var lookUpURL = base_url + 'MstCashSalesInvoice/GetList';
        var lookupGrid = $('#lookup_table_cashbank');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_cashbank').dialog('open');
    });

    $("#lookup_table_cashbank").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_cashbank'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_cashbank").width() - 10,
        height: $("#lookup_div_cashbank").height() - 110,
    });
    $("#lookup_table_cashbank").jqGrid('navGrid', '#lookup_toolbar_cashbank', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_cashbank').click(function () {
        $('#lookup_div_cashbank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_cashbank').click(function () {
        var id = jQuery("#lookup_table_cashbank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_cashbank").jqGrid('getRowData', id);

            $('#CashBankId').val(ret.id).data("kode", id);
            $('#CashBankName').val(ret.name);

            $('#lookup_div_cashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    
    // ---------------------------------------------End Lookup CashBank----------------------------------------------------------------

    // -------------------------------------------------------Look Up Warehouse-------------------------------------------------------
    $('#btnWarehouse').click(function () {
        var lookUpURL = base_url + 'MstCashSalesInvoice/GetList';
        var lookupGrid = $('#lookup_table_warehouse');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_warehouse').dialog('open');
    });

    jQuery("#lookup_table_warehouse").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description', 'Code'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 200 },
                  { name: 'code', index: 'code', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_warehouse'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_warehouse").width() - 10,
        height: $("#lookup_div_warehouse").height() - 110,
    });
    $("#lookup_table_warehouse").jqGrid('navGrid', '#lookup_toolbar_warehouse', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_warehouse').click(function () {
        $('#lookup_div_warehouse').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_warehouse').click(function () {
        var id = jQuery("#lookup_table_warehouse").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_warehouse").jqGrid('getRowData', id);

            $('#WarehouseId').val(ret.id).data("kode", id);
            $('#WarehouseName').val(ret.name);

            $('#lookup_div_warehouse').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup Warehouse----------------------------------------------------------------

}); //END DOCUMENT READY