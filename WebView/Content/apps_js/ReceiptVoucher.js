﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'ReceiptVoucher/GetList', postData: { filters: null } }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'ReceiptVoucher/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
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
    $("#reconcile_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_cashbank").dialog('close');
    $("#lookup_div_receivable").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#ContactId").hide();
    $("#CashBankId").hide();
    $("#ReceivableId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'ReceiptVoucher/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Contact Id', 'Contact Name', 'CashBank Id', 'CashBank Name', 'Receipt Date',
                   'Is GBCH', 'GBCH No.', 'Due Date', 'Total Amount','Currency', 'Rate','Is Reconciled', 'ReconciliationDate',
                    'Is Confirmed', 'Confirmation Date', 'No Bukti', 'Total PPh23', 'Biaya Bank', 'Pembulatan', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 70 },
				  { name: 'contactid', index: 'contactid', width: 100, hidden: true },
                  { name: 'contact', index: 'contact', width: 150 },
                  { name: 'cashbankid', index: 'cashbankid', width: 100, hidden: true },
                  { name: 'cashbank', index: 'cashbank', width: 100 },
                  { name: 'receiptdate', index: 'receiptdate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isgbch', index: 'isgbch', width: 45 },
                  { name: 'gbch_no', index: 'gbch_no', width: 100 },
                  { name: 'duedate', index: 'duedate', width: 80, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'totalamount', index: 'totalamount', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'exchangerateamount', index: 'exchangerateamount', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'isreconciled', index: 'isreconciled', width: 100, hidden: true },
                  { name: 'reconciliationdate', index: 'reconciliationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'nobukti', index: 'nobukti', width: 100 },
                  { name: 'totalpph23', index: 'totalpph23', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'biayabank', index: 'biayabank', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pembulatan', index: 'pembulatan', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
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

		          rowIsGBCH = $(this).getRowData(cl).isgbch;
		          if (rowIsGBCH == 'true') {
		              rowIsGBCH = "YES";
		          } else {
		              rowIsGBCH = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isgbch: rowIsGBCH });

		          rowIsReconciled = $(this).getRowData(cl).isreconciled;
		          if (rowIsReconciled == 'true') {
		              rowIsReconciled = "YES";
		          } else {
		              rowIsReconciled = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isreconciled: rowIsReconciled });
		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });



    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "ReceiptVoucher/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/PrintoutReceiptVoucher?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#TotalAmount').numberbox('clear');
        $('#TotalPPH23').numberbox('clear');
        $('#ReceiptDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#btnContact').removeAttr('disabled');
        $('#btnCashBank').removeAttr('disabled');
        $('#NoBukti').removeAttr('disabled');
        $('#BiayaBank').removeAttr('disabled');
        $('#Pembulatan').removeAttr('disabled');
        $('#Status').removeAttr('disabled');
        $('#IsGBCH').removeAttr('disabled');
        $('#GBCH_No').removeAttr('disabled');
        $('#RateToIDR').removeAttr('disabled');
        $('#ExchangeRateAmount').removeAttr('disabled');
        $('#CurrencyId').removeAttr('disabled');
        $('#ExchangeRateAmount').numberbox('setValue', 0);
        $('#tabledetail_div').hide();
        $('#ReceiptDateDiv').show();
        $('#ReceiptDateDiv2').hide();
        $('#DueDateDiv').show();
        $('#DueDateDiv2').hide();
        $('#form_btn_save').show();
        $('#form_div').dialog('open');
    });


    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "ReceiptVoucher/GetInfo?Id=" + id,
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
                            $('#NoBukti').val(result.NoBukti);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            $('#TotalPPH23').numberbox('setValue', result.TotalPPH23);
                            $('#BiayaBank').numberbox('setValue', result.BiayaBank);
                            $('#Pembulatan').numberbox('setValue', result.Pembulatan);
                            $('#GBCH_No').val(result.GBCH_No);
                            var e = document.getElementById("IsGBCH");
                            if (result.IsGBCH == true) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var e = document.getElementById("Status");
                            if (result.StatusPembulatan == 1) {
                                e.selectedIndex = 0;
                            }
                            else if (result.StatusPembulatan == 2) {
                                e.selectedIndex = 1;
                            }
                            $('#CurrencyCashBank').val(result.Currency);
                            $('#RateToIDR').attr('disabled', true);
                            $('#RateToIDR').numberbox('setValue', result.RateToIDR);
                            $('#ReceiptDate').datebox('setValue', dateEnt(result.ReceiptDate));
                            $('#ReceiptDate2').val(dateEnt(result.ReceiptDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#ReceiptDateDiv2').show();
                            $('#ReceiptDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#DueDateDiv').hide();
                            $('#form_btn_save').hide();
                            $('#btnContact').attr('disabled', true);
                            $('#btnCashBank').attr('disabled', true);
                            $('#TotalAmount').attr('disabled', true);
                            $('#NoBukti').attr('disabled', true);
                            $('#IsGBCH').attr('disabled', true);
                            $('#GBCH_No').attr('disabled', true);
                            $('#BiayaBank').attr('disabled', true);
                            $('#Pembulatan').attr('disabled', true);
                            $('#Status').attr('disabled', true);
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
                url: base_url + "ReceiptVoucher/GetInfo?Id=" + id,
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
                            $('#NoBukti').val(result.NoBukti);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            $('#TotalPPH23').numberbox('setValue', result.TotalPPH23);
                            $('#BiayaBank').numberbox('setValue', result.BiayaBank);
                            $('#Pembulatan').numberbox('setValue', result.Pembulatan);
                            $('#GBCH_No').val(result.GBCH_No);
                            var e = document.getElementById("IsGBCH");
                            if (result.IsGBCH == true) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var e = document.getElementById("Status");
                            if (result.StatusPembulatan == 1) {
                                e.selectedIndex = 0;
                            }
                            else if (result.StatusPembulatan == 2) {
                                e.selectedIndex = 1;
                            }
                            $('#CurrencyCashBank').val(result.Currency);    
                            $('#ReceiptDate').datebox('setValue', dateEnt(result.ReceiptDate));
                            $('#ReceiptDate2').val(dateEnt(result.ReceiptDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#ReceiptDateDiv2').show();
                            $('#ReceiptDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#DueDateDiv').hide();
                            $('#CurrencyId').val(result.CurrencyId);
                            $('#RateToIDR').numberbox('setValue', result.RateToIDR);
                            $('#CurrencyId').removeAttr('disabled');
                            $('#RateToIDR').removeAttr('disabled');
                            $('#btnContact').removeAttr('disabled');
                            $('#btnCashBank').removeAttr('disabled');
                            $('#NoBukti').removeAttr('disabled');
                            $('#IsGBCH').removeAttr('disabled');
                            $('#GBCH_No').removeAttr('disabled');
                            $('#BiayaBank').removeAttr('disabled');
                            $('#Pembulatan').removeAttr('disabled');
                            $('#Status').removeAttr('disabled');
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
                        url: base_url + "ReceiptVoucher/Unconfirm",
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
            url: base_url + "ReceiptVoucher/Confirm",
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

    $('#btn_reconcile').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idreconcile').val(ret.id);
            $("#reconcile_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unreconcile').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unreconcile record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "ReceiptVoucher/UnReconcile",
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
                                $("#delete_reconcile_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#reconcile_btn_submit').click(function () {
        ClearErrorMessage();
        ClickableButton($("#reconcile_btn_submit"), false);
        $.ajax({
            url: base_url + "ReceiptVoucher/Reconcile",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idreconcile').val(), ReconciliationDate: $('#ReconciliationDate').datebox('getValue'),
            }),
            success: function (result) {
                ClickableButton($("#reconcile_btn_submit"), true);
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
                    $("#reconcile_div").dialog('close');
                }
            }
        });
    });

    $('#reconcile_btn_cancel').click(function () {
        $('#reconcile_div').dialog('close');
    });

    $("#AmountPaid, #Rate").blur(function () {
        var total = parseFloat($('#AmountPaid').numberbox('getValue')) / parseFloat($('#Rate').numberbox('getValue'));
        //total = Math.round(total * 100) / 100;
        var pph23 = parseFloat($('#AmountPaid').numberbox('getValue')) * 2.0 / 100.0;
        $('#Amount').numberbox('setValue', total);
        //$('#PPH23').numberbox('setValue', pph23);
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
            url: base_url + "ReceiptVoucher/Delete",
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
        if ($('#ReceiptDate').datebox('getValue') == "") {
            return $($('#ReceiptDate').addClass('errormessage').before('<span class="errormessage">**' + "Receipt Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ReceiptVoucher/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'ReceiptVoucher/Insert';
        }

        var e = document.getElementById("IsGBCH");
        var gbch = e.options[e.selectedIndex].value;
        var f = document.getElementById("Status");
        var status = f.selectedIndex + 1;

        ClickableButton($("#form_btn_save"), false);
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ContactId: $("#ContactId").val(), CashBankId: $("#CashBankId").val(),
                IsGBCH: gbch, RateToIDR: $("#RateToIDR").numberbox('getValue'),
                ReceiptDate: $('#ReceiptDate').datebox('getValue'), DueDate: $('#DueDate').datebox('getValue'),
                NoBukti: $('#NoBukti').val(), BiayaBank: $("#BiayaBank").numberbox('getValue'), Pembulatan: $("#Pembulatan").numberbox('getValue'),
                StatusPembulatan: status, GBCH_No: $('#GBCH_No').val(),
            }),
            //async: false,
            //cache: false,
            //timeout: 30000,
            //error: function () {
            //    return false;
            //},
            success: function (result) {
                ClickableButton($("#form_btn_save"), true);
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
        colNames: ['Id', 'Code', 'Currency', 'Receivable Id', 'Receivable Code', 'Amount Paid','Rate','Actual Amount', 'PPh 23', 'No Surat', 'Description'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false },
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'currency', index: 'currency', width: 80, sortable: false },
                  { name: 'receivableid', index: 'receivableid', width: 130, sortable: false, hidden: true },
                  { name: 'receivable', index: 'receivable', width: 110, sortable: false },
                  { name: 'amountpaid', index: 'amountpaid', width: 180, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
                  { name: 'rate', index: 'rate', width: 80, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 11, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 180, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
                  { name: 'pph23', index: 'pph23', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 100 },
                  { name: 'description', index: 'description', width: 180, sortable: false }
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
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

    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false })
                    .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#Amount').numberbox('clear');
        $('#AmountPaid').numberbox('clear');
        $('#PPH23').numberbox('clear');
        $('#Rate').numberbox('clear');
        $('#Remaining').numberbox('clear');
        $('#Currency').text("");
        $('#ToCurrency').text(' To ' + $('#CurrencyCashBank').val());
        $('#PaidCurrency').text($('#CurrencyCashBank').val());
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "ReceiptVoucher/GetInfoDetail?Id=" + id,
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
                            $('#ReceivableId').val(result.ReceivableId);
                            $('#Receivable').val(result.Receivable);
                            $('#Amount').val(result.Amount);
                            $('#Rate').numberbox('setValue', result.Rate);
                            $('#AmountPaid').numberbox('setValue', result.AmountPaid);
                            $('#PPH23').numberbox('setValue', result.PPH23);
                            $('#Currency').text(result.Currency);
                            $('#ToCurrency').text(' To ' + $('#CurrencyCashBank').val());
                            $('#PaidCurrency').text($('#CurrencyCashBank').val());
                            $('#Remaining').numberbox('setValue', result.Remaining);
                            $('#Description').val(result.Description);
                            $('#ReceiptVoucherDetailId').val(result.ReceiptVoucherDetailId);
                            if (result.Currency == $('#CurrencyCashBank').val()) {
                                $('#Rate').attr('disabled', true);
                                $('#Rate').numberbox('setValue', result.Rate);
                            }
                            else {
                                $('#Rate').removeAttr('disabled');
                                $('#Rate').numberbox('setValue', result.Rate);
                            }
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
                        url: base_url + "ReceiptVoucher/DeleteDetail",
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
                                ReloadGridDetail();
                                $("#TotalAmount").numberbox('setValue', result.totalamount);
                                $("#TotalPPH23").numberbox('setValue', result.totalpph23);
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
            submitURL = base_url + 'ReceiptVoucher/UpdateDetail';
        }
            // Insert 
        else {
            submitURL = base_url + 'ReceiptVoucher/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ReceiptVoucherId: $("#id").val(), ReceivableId: $("#ReceivableId").val(), Description: $("#Description").val(),
                AmountPaid: $("#AmountPaid").numberbox('getValue'), Rate: $("#Rate").numberbox('getValue'),
                Amount: $("#Amount").numberbox('getValue'), PPH23: $("#PPH23").numberbox('getValue'),
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
                    ReloadGridDetail();
                    $("#TotalAmount").numberbox('setValue', result.totalamount);
                    $("#TotalPPH23").numberbox('setValue', result.totalpph23);
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

    // -------------------------------------------------------Look Up contact-------------------------------------------------------
    $('#btnContact').click(function () {
        var lookUpURL = base_url + 'MstContact/GetShortList'; //GetListCustomer
        var lookupGrid = $('#lookup_table_contact');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_contact').dialog('open');
    });

    jQuery("#lookup_table_contact").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'Nama Faktur Pajak','Group ID', 'Contact Group', 'Contact Type'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'name', index: 'name', width: 250 },
                  { name: 'NamaFakturPajak', index: 'NamaFakturPajak', width: 250 },
                  { name: 'contactgroupid', index: 'contactgroupid', width: 60, align: "center", hidden: true },
				  { name: 'contactgroup', index: 'contactgroup', width: 180 },
				  { name: 'contacttype', index: 'contacttype', width: 100 },
        ],
        page: '1',
        pager: $('#lookup_pager_contact'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_contact").width() - 10,
        height: $("#lookup_div_contact").height() - 110,
    });
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_contact').click(function () {
        $('#lookup_div_contact').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_contact').click(function () {
        var id = jQuery("#lookup_table_contact").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_contact").jqGrid('getRowData', id);

            $('#ContactId').val(ret.id).data("kode", id);
            $('#Contact').val(ret.name);
            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up cashbank-------------------------------------------------------
    $('#btnCashBank').click(function () {
        var lookUpURL = base_url + 'MstCashBank/GetList';
        var lookupGrid = $('#lookup_table_cashbank');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_cashbank').dialog('open');
    });

    jQuery("#lookup_table_cashbank").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name','Description','Amount','Currency', 'Is Bank', 'Code'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'name', index: 'name', width: 120 },
                  { name: 'description', index: 'description', width: 200 },
                  { name: 'amount', index: 'amount', width: 100, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } ,hidden : true},
				  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'isbank', index: 'isbank', width: 50 },
                  { name: 'code', index: 'code', width: 60 },
        ],
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
    $("#lookup_table_cashbank").jqGrid('navGrid', '#lookup_toolbar_cashbank', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_cashbank').click(function () {
        $('#lookup_div_cashbank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_cashbank').click(function () {
        var id = jQuery("#lookup_table_cashbank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_cashbank").jqGrid('getRowData', id);
            var codeRV = ret.code.substr(0, 2) + "T" + ret.code.substr(3); //Karakter ke-3 harus T sbg kode Penerimaan, K untuk Pengeluaran

            $('#CashBankId').val(ret.id).data("kode", id);
            $('#CashBank').val(ret.name);
            $('#NoBukti').val(codeRV);
            $('#CurrencyCashBank').val(ret.currency);
            $('#lookup_div_cashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup cashbank----------------------------------------------------------------

    // -------------------------------------------------------Look Up receivable-------------------------------------------------------
    $('#btnReceivable').click(function () {
        var lookUpURL = base_url + 'ReceiptVoucher/GetListReceivableNonDP?contactid=' + $("#ContactId").val();
        var lookupGrid = $('#lookup_table_receivable');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_receivable').dialog('open');
    });

    jQuery("#lookup_table_receivable").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Contact Id', 'Contact', 'Due Date', 'Total',
                    'Remaining', 'PendClearance', 'Currency', 'Rate', 'Receivable Source', 'Id', 'No Surat'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 55, sortable: false },
                  { name: 'contactid', index: 'contactid', width: 100, sortable: false, hidden: true },
                  { name: 'contact', index: 'contact', width: 250, sortable: false },
                  { name: 'duedate', index: 'duedate', search: false, width: 70, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'remainingamount', index: 'remainingamount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', align: 'right', width: 0, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'rate', index: 'rate', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'receivablesource', index: 'receivablesource', width: 180, sortable: false },
                  { name: 'receivablesourceid', index: 'receivablesourceid', width: 40, align: 'right', sortable: false },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 100 },
        ],
        page: '1',
        pager: $('#lookup_pager_receivable'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_receivable").width() - 10,
        height: $("#lookup_div_receivable").height() - 110,
    });
    $("#lookup_table_receivable").jqGrid('navGrid', '#lookup_toolbar_receivable', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_receivable').click(function () {
        $('#lookup_div_receivable').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_receivable').click(function () {
        var id = jQuery("#lookup_table_receivable").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_receivable").jqGrid('getRowData', id);
            $('#ReceivableId').val(id);
            $('#Receivable').val(ret.code);
            $('#Currency').text(ret.currency);
            if (ret.currency == $('#CurrencyCashBank').val()) {
                $('#Rate').attr('disabled', true);
            }
            else {
                $('#Rate').removeAttr('disabled');
            }
            $('#Rate').numberbox('setValue', 1);
            $('#Remaining').numberbox('setValue',ret.remainingamount);
            $('#lookup_div_receivable').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup receivable----------------------------------------------------------------


}); //END DOCUMENT READY