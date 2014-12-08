$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'PurchaseInvoice/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'PurchaseInvoice/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
        });
    }

    $("#item_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_purchasereceival").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#PurchaseReceivalId").hide();
    $("#ItemId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'PurchaseInvoice/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Nomor Surat', 'PurchaseReceipt Id', 'PR', 'Description', 'Disc(%)', 'Tax(%)',
                   'Invoice Date','Due Date', 'Amount',
                    'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 70 },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 140 },
				  { name: 'purchasereceiptid', index: 'purchasereceiptid', width: 100, hidden: true },
                  { name: 'purchasereceipt', index: 'purchasereceipt', width: 70 },
                  { name: 'description', index: 'description', width: 100 },
                  { name: 'discount', index: 'discount', width: 50, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'tax', index: 'tax', width: 50, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'invoicedate', index: 'invoicedate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'duedate', index: 'duedate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'amountpayable', index: 'amountpayable', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
                url: base_url + "PurchaseInvoice/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/ReportPurchaseInvoice?Id=" + id);
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
        $('#btnPurchaseReceival').removeAttr('disabled');
        $('#NomorSurat').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#InvoiceDateDiv').show();
        $('#InvoiceDateDiv2').hide();
        $('#DueDateDiv').show();
        $('#DueDateDiv2').hide();
        $("#Discount").removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#form_btn_save').show();
        $('#form_div').dialog('open');
        $('#Currency').data("kode", "");

    });


    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "PurchaseInvoice/GetInfo?Id=" + id,
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
                            $('#PurchaseReceivalId').val(result.PurchaseReceivalId);
                            $('#PurchaseReceival').val(result.PurchaseReceival);
                            $('#Description').val(result.Description);
                            $('#Discount').val(result.Discount);
                            $('#Tax').val(result.Tax);
                            $('#Currency').val(result.currency).data("kode", result.CurrencyId);
                            $('#ExchangeRateAmount').val(result.ExchangeRateAmount);
                            $('#ExchangeRateAmount').attr('disabled', true);
                            $("#Discount").attr('disabled', true);
                            $('#AmountPayable').val(result.AmountPayable);
                            $('#InvoiceDate').datebox('setValue', dateEnt(result.InvoiceDate));
                            $('#InvoiceDate2').val(dateEnt(result.InvoiceDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#InvoiceDateDiv').hide();
                            $('#InvoiceDateDiv2').show();
                            $('#DueDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#form_btn_save').hide();
                            $('#btnPurchaseReceival').attr('disabled', true);
                            $('#NomorSurat').attr('disabled', true);
                            $('#Description').attr('disabled', true);
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
                url: base_url + "PurchaseInvoice/GetInfo?Id=" + id,
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
                            $('#PurchaseReceivalId').val(result.PurchaseReceivalId);
                            $('#PurchaseReceival').val(result.PurchaseReceival);
                            $('#Description').val(result.Description);
                            $('#Discount').val(result.Discount);
                            $('#Tax').val(result.Tax);
                            $('#Currency').val(result.currency).data("kode", result.CurrencyId);
                            $('#ExchangeRateAmount').val(result.ExchangeRateAmount);
                            $('#ExchangeRateAmount').removeAttr('disabled');
                            $("#Discount").removeAttr('disabled');
                            $('#AmountPayable').val(result.AmountPayable);
                            $('#InvoiceDate').datebox('setValue', dateEnt(result.InvoiceDate));
                            $('#InvoiceDate2').val(dateEnt(result.InvoiceDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#InvoiceDateDiv').show();
                            $('#InvoiceDateDiv2').hide();
                            $('#DueDateDiv').show();
                            $('#DueDateDiv2').hide();
                            $('#form_btn_save').hide();
                            $('#btnPurchaseReceival').removeAttr('disabled');
                            $('#NomorSurat').removeAttr('disabled');
                            $('#Description').removeAttr('disabled');
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
                        url: base_url + "PurchaseInvoice/Unconfirm",
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
        $.ajax({
            url: base_url + "PurchaseInvoice/Confirm",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
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
            url: base_url + "PurchaseInvoice/Delete",
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
            submitURL = base_url + 'PurchaseInvoice/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'PurchaseInvoice/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, PurchaseReceivalId: $("#PurchaseReceivalId").val(), Description: $("#Description").val(),
                Discount: $("#Discount").numberbox('getValue'), Tax: $("#Tax").val(),
                InvoiceDate: $('#InvoiceDate').datebox('getValue'), DueDate: $('#DueDate').datebox('getValue'),
                NomorSurat: $('#NomorSurat').val(),CurrencyId: $("#Currency").data('kode'), ExchangeRateAmount: $("#ExchangeRateAmount").numberbox('getValue'),
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
        colNames: ['Code', 'Purchase Receival Id', 'PRD', 'Item Id', 'Item Sku', 'Name', 'QTY', 'Amount'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'purchasereceivaldetailid', index: 'purchasereceivaldetailid', width: 70, sortable: false, hidden: true },
                  { name: 'purchasereceivaldetailcode', index: 'purchasereceivaldetailcode', width: 70, sortable: false },
                  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } ,sortable: false },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
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
                url: base_url + "PurchaseInvoice/GetInfoDetail?Id=" + id,
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
                            $('#PurchaseReceivalDetailId').val(result.PurchaseReceivalDetailId);
                            $('#ItemId').val(result.ItemId);
                            $('#Item').val(result.Item);
                            $('#Quantity').numberbox('setValue', result.Quantity);
                            $('#Price').numberbox('setValue', result.Price);
                            $('#PurchaseOrderDetailId').val(result.PurchaseOrderDetailId);
                            $('#item_div').dialog('open');
                            $("#Discount").attr('disabled', true);
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
                        url: base_url + "PurchaseInvoice/DeleteDetail",
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
                                $('#AmountPayable').val(result.AmountPayable);
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
            submitURL = base_url + 'PurchaseInvoice/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'PurchaseInvoice/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, PurchaseInvoiceId: $("#id").val(), PurchaseReceivalDetailId: $("#PurchaseReceivalDetailId").val(), ItemId: $("#ItemId").val(), Quantity: $("#Quantity").numberbox('getValue'),
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
                    $('#AmountPayable').val(result.AmountPayable);
                    ReloadGridDetail();
                    ReloadGrid();
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

    // -------------------------------------------------------Look Up purchasereceival-------------------------------------------------------
    $('#btnPurchaseReceival').click(function () {
        var lookUpURL = base_url + 'PurchaseReceival/GetListConfirmed';
        var lookupGrid = $('#lookup_table_purchasereceival');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_purchasereceival').dialog('open');
    });

    jQuery("#lookup_table_purchasereceival").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Nomor Surat', 'PurchaseOrder Id', 'PO Code', 'Nomor Surat PO', 'Warehouse Id', 'Warehouse Name', 'Receival Date',
                   'CurrencyId','Currency','Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At', 'Tax (%)'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center", hidden: true },
                  { name: 'code', index: 'code', width: 80, hidden: true },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 120 },
				  { name: 'purchaseorderid', index: 'purchaseorderid', width: 100, hidden: true },
                  { name: 'purchaseordercode', index: 'purchaseordercode', width: 80, hidden: true },
                  { name: 'nomorsuratpo', index: 'nomorsuratpo', width: 120 },
				  { name: 'warehouseid', index: 'warehouseid', width: 100, hidden: true },
                  { name: 'warehousename', index: 'warehousename', width: 100 },
                  { name: 'receivaldate', index: 'receivaldate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'CurrencyId', index: 'CurrencyId', width: 100, hidden: true },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
                  { name: 'tax', index: 'tax', width: 50 },
        ],
        page: '1',
        pager: $('#lookup_pager_purchasereceival'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_purchasereceival").width() - 10,
        height: $("#lookup_div_purchasereceival").height() - 110,
    });
    $("#lookup_table_purchasereceival").jqGrid('navGrid', '#lookup_toolbar_purchasereceival', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_purchasereceival').click(function () {
        $('#lookup_div_purchasereceival').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_purchasereceival').click(function () {
        var id = jQuery("#lookup_table_purchasereceival").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_purchasereceival").jqGrid('getRowData', id);

            $('#PurchaseReceivalId').val(ret.id).data("kode", id);
            $('#PurchaseReceival').val(ret.code);
            $('#Discount').val(0);
            $('#Tax').val(ret.tax);
            $('#Currency').val(ret.currency).data("kode", ret.CurrencyId);

            $('#lookup_div_purchasereceival').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup purchasereceival----------------------------------------------------------------

    // -------------------------------------------------------Look Up item-------------------------------------------------------
    $('#btnItem').click(function () {
        var lookUpURL = base_url + 'PurchaseReceival/GetListDetail?Id=' + $("#PurchaseReceivalId").val();
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
        colNames: ['Id', 'Code', 'Purchase Order Detail Id', 'POD', 'Item Id', 'Item Sku', 'Name', 'QTY', 'Price',
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 70, sortable: false, hidden: true },
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'purchaseorderdetailid', index: 'purchaseorderdetailid', width: 100, sortable: false, hidden: true },
                  { name: 'purchaseorderdetailcode', index: 'purchaseorderdetailcode', width: 70, sortable: false },
                  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 40, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'price', index: 'price', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
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
            $('#PurchaseReceivalDetailId').val(id);
            $('#ItemId').val(ret.itemid);
            $('#Item').val(ret.itemname);

            $('#lookup_div_item').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup item----------------------------------------------------------------

    
}); //END DOCUMENT READY