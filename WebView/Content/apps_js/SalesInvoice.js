﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'SalesInvoice/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'SalesInvoice/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#form_btn_save').data('kode', '');
        $('#item_btn_submit').data('kode', '');
        ClearErrorMessage();
    }

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

    $("#item_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_deliveryorder").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#DeliveryOrderId").hide();
    $("#ItemId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'SalesInvoice/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Contact', 'Nomor Surat', 'Delivery Order Id', 'DO', 'Description', 'Disc(%)', 'Tax(%)','Currency',
                   'Rate','Invoice Date', 'Due Date', 'Amount',
                    'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 80 },
                  { name: 'contact', index: 'contact', width: 200 },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 140 },
				  { name: 'deliveryorderid', index: 'deliveryorderid', width: 100, hidden: true },
                  { name: 'deliveryorder', index: 'deliveryorder', width: 70 },
                  { name: 'description', index: 'description', width: 100 },
                  { name: 'discount', index: 'discount', width: 50, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false, hidden: true },
                  { name: 'tax', index: 'tax', width: 50, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'ExchangeRateAmount', index: 'ExchangeRateAmount', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'invoicedate', index: 'invoicedate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'duedate', index: 'duedate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'amountreceivable', index: 'amountreceivable', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsConfirmed = $(this).getRowData(cl).isconfirmed;
		          if (rowIsConfirmed == 'true') {
		              rowIsConfirmed = "YES";
		          } else {
		              rowIsConfirmed = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isconfirmed: rowIsConfirmed });
		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });



    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalesInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/PrintoutSalesInvoiceConfirm?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_printpajak').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalesInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/PrintoutFakturPajak?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#InvoiceDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#btnDeliveryOrder').removeAttr('disabled');
        $('#NomorSurat').removeAttr('disabled');
        $('#Discount').removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#ExchangeRateAmount').removeAttr('disabled');
        $('#ExchangeRateAmount').val(0);
        $('#tabledetail_div').hide();
        $('#InvoiceDateDiv').show();
        $('#InvoiceDateDiv2').hide();
        $('#DueDateDiv').show();
        $('#DueDateDiv2').hide();
        $('#form_btn_save').show();
        $('#form_div').dialog('open');
        $('#Currency').data("kode","");

    });


    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalesInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Code').val(result.Code);
                            $('#NomorSurat').val(result.NomorSurat);
                            $('#DeliveryOrderId').val(result.DeliveryOrderId);
                            $('#DeliveryOrder').val(result.DeliveryOrder);
                            $('#Description').val(result.Description);
                            $('#Discount').val(result.Discount);
                            $('#Tax').val(result.Tax);
                            $('#Currency').val(result.currency).data("kode", result.CurrencyId);
                            $('#Discount').attr('disabled', true);
                            $('#Description').attr('disabled', true);
                            $('#NomorSurat').attr('disabled', true);
                            $('#AmountReceivable').val(result.AmountReceivable);
                            $('#ExchangeRateAmount').val(result.ExchangeRateAmount);
                            $('#ExchangeRateAmount').attr('disabled', true);
                            $('#InvoiceDate').datebox('setValue', dateEnt(result.InvoiceDate));
                            $('#InvoiceDate2').val(dateEnt(result.InvoiceDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#InvoiceDateDiv').show();
                            $('#InvoiceDateDiv2').hide();
                            $('#DueDateDiv').show();
                            $('#DueDateDiv2').hide();
                            $('#form_btn_save').hide();
                            $('#btnDeliveryOrder').attr('disabled', true);
                            $('#tabledetail_div').show();
                            ReloadGridDetail();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalesInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Code').val(result.Code);
                            $('#NomorSurat').val(result.NomorSurat);
                            $('#DeliveryOrderId').val(result.DeliveryOrderId);
                            $('#DeliveryOrder').val(result.DeliveryOrder);
                            $('#Description').val(result.Description);
                            $('#Discount').val(result.Discount);
                            $('#Tax').val(result.Tax);
                            $('#ExchangeRateAmount').val(result.ExchangeRateAmount);
                            $('#ExchangeRateAmount').removeAttr('disabled');
                            $('#Currency').val(result.currency).data("kode",result.CurrencyId);
                            $('#Discount').removeAttr('disabled');
                            $('#Description').removeAttr('disabled');
                            $('#NomorSurat').removeAttr('disabled');
                            $('#AmountReceivable').val(result.AmountReceivable);
                            $('#InvoiceDate').datebox('setValue', dateEnt(result.InvoiceDate));
                            $('#InvoiceDate2').val(dateEnt(result.InvoiceDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#InvoiceDateDiv').show();
                            $('#InvoiceDateDiv2').hide();
                            $('#DueDateDiv').show();
                            $('#DueDateDiv2').hide();
                            $('#form_btn_save').hide();
                            $('#btnDeliveryOrder').removeAttr('disabled');
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_confirm').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idconfirm').val(ret.id);
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unconfirm').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unconfirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "SalesInvoice/Unconfirm",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
                        }),
                        success: function (result) {
                            if (JSON.stringify(result.Errors) != '{}') {
                                for (var key in result.Errors) {
                                    if (key != null && key != undefined && key != 'Generic') {
                                        $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                        $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                    }
                                    else {
                                        $.messager.alert('Warning', result.Errors[key], 'warning');
                                    }
                                }
                            }
                            else {
                                ReloadGrid();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#confirm_btn_submit').click(function () {
        ClearErrorMessage();
        ClickableButton($("#confirm_btn_submit"), false);
        $.ajax({
            url: base_url + "SalesInvoice/Confirm",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
            }),
            success: function (result) {
                ClickableButton($("#confirm_btn_submit"), true);
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGrid();
                    $("#confirm_div").dialog('close');
                }
            }
        });
    });

    $('#confirm_btn_cancel').click(function () {
        $('#confirm_div').dialog('close');
    });


    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
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
            url: base_url + "SalesInvoice/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
            }),
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                    $("#delete_confirm_div").dialog('close');
                }
                else {
                    ReloadGrid();
                    $("#delete_confirm_div").dialog('close');
                }
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
        //ReloadGrid();
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#id").val();
        if ($('#InvoiceDate').datebox('getValue') == "") {
            return $($('#InvoiceDate').addClass('errormessage').before('<span class="errormessage">**' + "Adjustment Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'SalesInvoice/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'SalesInvoice/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, DeliveryOrderId: $("#DeliveryOrderId").val(), Description: $("#Description").val(),
                Discount: $("#Discount").numberbox('getValue'), Tax: $("#Tax").val(),
                InvoiceDate: $('#InvoiceDate').datebox('getValue'), DueDate: $('#DueDate').datebox('getValue'),
                NomorSurat: $('#NomorSurat').val(), CurrencyId: $("#Currency").data('kode'), ExchangeRateAmount: $("#ExchangeRateAmount").numberbox('getValue'),
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
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Id', 'Code', 'Delivery Order Id', 'DOD', 'Item Id', 'Item Sku', 'Name', 'QTY', 'Amount (Quantity x Price)'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false },
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'deliveryorderdetailid', index: 'deliveryorderdetailid', width: 70, sortable: false, hidden: true },
                  { name: 'deliveryorderdetailcode', index: 'deliveryorderdetailcode', width: 70, sortable: false },
                  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 300, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "SalesInvoice/GetInfoDetail?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#item_btn_submit").data('kode', result.Id);
                            $('#ItemId').val(result.ItemId);
                            $('#Item').val(result.Item);
                            $('#Quantity').numberbox('setValue',result.Quantity);
                            $('#Price').numberbox('setValue',result.Price);
                            $('#SalesOrderDetailId').val(result.SalesOrderDetailId);
                            $('#Discount').attr('disabled', true);
                            $('#Description').attr('disabled', true);
                            $('#item_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del_detail').click(function () {
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "SalesInvoice/DeleteDetail",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
                        }),
                        success: function (result) {
                            if (JSON.stringify(result.Errors) != '{}') {
                                for (var key in result.Errors) {
                                    if (key != null && key != undefined && key != 'Generic') {
                                        $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                        $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                                    }
                                    else {
                                        $.messager.alert('Warning', result.Errors[key], 'warning');
                                    }
                                }
                            }
                            else {
                                $('#AmountReceivable').val(result.AmountReceivable);
                                ReloadGridDetail();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // item_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'SalesInvoice/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'SalesInvoice/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, SalesInvoiceId: $("#id").val(), DeliveryOrderDetailId: $("#DeliveryOrderDetailId").val(), ItemId: $("#ItemId").val(), Quantity: $("#Quantity").val(),
            }),
            //async: false,
            //cache: false,
            //timeout: 30000,
            //error: function () {
            //    return false;
            //},
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    $('#AmountReceivable').val(result.AmountReceivable);
                    ReloadGridDetail();
                    $("#item_div").dialog('close')
                }
            }
        });
    });


    // item_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up deliveryorder-------------------------------------------------------
    $('#btnDeliveryOrder').click(function () {
        var lookUpURL = base_url + 'DeliveryOrder/GetListConfirmed';
        var lookupGrid = $('#lookup_table_deliveryorder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_deliveryorder').dialog('open');
    });

    jQuery("#lookup_table_deliveryorder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Contact', 'Nomor Surat DO', 'SalesOrder Id', 'SalesOrder Code', 'Nomor Surat SO', 'Delivery Date', 'Warehouse Id', 'Warehouse Name',
                   'CurrencyId','Currency','Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At', 'Tax (%)'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center", hidden: true },
                  { name: 'code', index: 'code', width: 50, hidden: true },
                  { name: 'contact', index: 'contact', width: 200 },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 140 },
				  { name: 'salesorderid', index: 'salesorderid', width: 100, hidden: true },
                  { name: 'salesorder', index: 'salesorder', width: 85, hidden: true },
                  { name: 'nomorsuratso', index: 'nomorsuratso', width: 140 },
                  { name: 'deliverydate', index: 'deliverydate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'warehouseid', index: 'warehouseid', width: 100, hidden: true },
                  { name: 'warehousename', index: 'warehousename', width: 100 },
                  { name: 'CurrencyId', index: 'CurrencyId', width: 100, hidden: true },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
                  { name: 'tax', index: 'tax', width: 50 },
        ],
        page: '1',
        pager: $('#lookup_pager_deliveryorder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_deliveryorder").width() - 10,
        height: $("#lookup_div_deliveryorder").height() - 110,
    });
    $("#lookup_table_deliveryorder").jqGrid('navGrid', '#lookup_toolbar_deliveryorder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_deliveryorder').click(function () {
        $('#lookup_div_deliveryorder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_deliveryorder').click(function () {
        var id = jQuery("#lookup_table_deliveryorder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_deliveryorder").jqGrid('getRowData', id);

            $('#DeliveryOrderId').val(ret.id).data("kode", id);
            $('#DeliveryOrder').val(ret.code);
            $('#Discount').val(0);
            $('#Tax').val(ret.tax);
            $('#Currency').val(ret.currency).data("kode", ret.CurrencyId);
            $('#lookup_div_deliveryorder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup deliveryorder----------------------------------------------------------------

    // -------------------------------------------------------Look Up item-------------------------------------------------------
    $('#btnItem').click(function () {
        var lookUpURL = base_url + 'DeliveryOrder/GetListDetail?Id=' + $("#DeliveryOrderId").val();
        var lookupGrid = $('#lookup_table_item');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_item').dialog('open');
    });

    jQuery("#lookup_table_item").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'DOD Code', 'Sales Order Detail Id', 'SOD Code', 'Item Id', 'Item Sku', 'Name', 'QTY',
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false, hidden: true },
                  { name: 'code', index: 'code', width: 70, sortable: false, hidden: true },
                  { name: 'salesorderdetailid', index: 'salesorderdetailid', width: 100, sortable: false, hidden: true },
                  { name: 'salesorderdetailcode', index: 'salesorderdetailcode', width: 70, sortable: false, hidden: true },
                  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 40, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
        ],
        page: '1',
        pager: $('#lookup_pager_item'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_item").width() - 10,
        height: $("#lookup_div_item").height() - 110,
    });
    $("#lookup_table_item").jqGrid('navGrid', '#lookup_toolbar_item', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_item').click(function () {
        $('#lookup_div_item').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_item').click(function () {
        var id = jQuery("#lookup_table_item").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_item").jqGrid('getRowData', id);
            $('#DeliveryOrderDetailId').val(id);
            $('#ItemId').val(ret.itemid);
            $('#Item').val(ret.itemname);

            $('#lookup_div_item').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup item----------------------------------------------------------------


}); //END DOCUMENT READY