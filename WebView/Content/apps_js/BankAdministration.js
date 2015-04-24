$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'BankAdministration/GetList', postData: { filters: null } }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'BankAdministration/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#form_btn_save').data('kode', '');
        ClearErrorMessage();
    }

    $("#lookup_div_account").dialog('close');
    $("#lookup_div_cashbank").dialog('close');
    $("#form_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#item_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#CashBankId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'BankAdministration/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'No Bukti', 'CashBank', 'Exchange Rate',
                   'Total Amount', //'Biaya Admin', 'Biaya Bunga', 'Pendapatan Jasa', 'Pendapatan Bunga', 'Pengembalian Piutang', 
                   'Catatan', 'Administration Date', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
				  { name: 'code', index: 'code', width: 80 },
                  { name: 'nobukti', index: 'nobukti', width: 80 },
                  { name: 'cashbank', index: 'cashbank', width: 150 },
                  { name: 'ExchangeRateAmount', index: 'ExchangeRateAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'Amount', index: 'Amount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  //{ name: 'BiayaAdminAmount', index: 'BiayaAdminAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  //{ name: 'BiayaBungaAmount', index: 'BiayaBungaAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  //{ name: 'PendapatanJasaAmount', index: 'PendapatanJasaAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  //{ name: 'PendapatanBungaAmount', index: 'PendapatanBungaAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  //{ name: 'PengembalianPiutangAmount', index: 'PengembalianPiutangAmount', width: 100, formatter: 'currency', align: "right", formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'administrationdate', index: 'administrationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'confirmationdate', index: 'confirmationdate', search:false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
                url: base_url + "BankAdministration/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/PrintoutBankAdministration?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#AdministrationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#NoBukti').removeAttr('disabled');
        $('#btnCashBank').removeAttr('disabled');
        $('#Description').removeAttr('disabled');
        $('#ExchangeRateAmount').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#AdministrationDateDiv').show();
        $('#AdministrationDateDiv2').hide();
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
                url: base_url + "BankAdministration/GetInfo?Id=" + id,
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
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#Description').val(result.Description);
                            $('#ExchangeRateAmount').numberbox('setValue', (result.ExchangeRateAmount));
                            $('#TotalAmount').numberbox('setValue', (result.Amount));
                            //$('#BiayaAdminAmount').numberbox('setValue', (result.BiayaAdminAmount));
                            //$('#BiayaBungaAmount').numberbox('setValue', (result.BiayaBungaAmount));
                            //$('#PendapatanJasaAmount').numberbox('setValue', (result.PendapatanJasaAmount));
                            //$('#PendapatanBungaAmount').numberbox('setValue', (result.PendapatanBungaAmount));
                            //$('#PengembalianPiutangAmount').numberbox('setValue', (result.PengembalianPiutangAmount));
                            $('#AdministrationDate').datebox('setValue', dateEnt(result.AdministrationDate));
                            $('#AdministrationDate2').val(dateEnt(result.AdministrationDate));
                            //var e = document.getElementById("IsExpense");
                            //if (result.IsExpense == true) {
                            //    e.selectedIndex = 1;
                            //}
                            //else {
                            //    e.selectedIndex = 0;
                            //}
                            $('#AdministrationDateDiv').hide();
                            $('#AdministrationDateDiv2').show();
                            $('#NoBukti').attr('disabled', true);
                            $('#btnCashBank').attr('disabled', true);
                            $('#Description').attr('disabled', true);
                            $('#ExchangeRateAmount').attr('disabled', true);

                            $('#form_btn_save').hide();
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
                url: base_url + "BankAdministration/GetInfo?Id=" + id,
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
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#Description').val(result.Description);
                            $('#ExchangeRateAmount').numberbox('setValue', (result.ExchangeRateAmount));
                            $('#TotalAmount').numberbox('setValue', (result.Amount));
                            //$('#BiayaAdminAmount').numberbox('setValue', (result.BiayaAdminAmount));
                            //$('#BiayaBungaAmount').numberbox('setValue', (result.BiayaBungaAmount));
                            //$('#PendapatanJasaAmount').numberbox('setValue', (result.PendapatanJasaAmount));
                            //$('#PendapatanBungaAmount').numberbox('setValue', (result.PendapatanBungaAmount));
                            //$('#PengembalianPiutangAmount').numberbox('setValue', (result.PengembalianPiutangAmount));
                            $('#AdministrationDate').datebox('setValue', dateEnt(result.AdministrationDate));
                            //var e = document.getElementById("IsExpense");
                            //if (result.IsExpense == true) {
                            //    e.selectedIndex = 1;
                            //}
                            //else {
                            //    e.selectedIndex = 0;
                            //}
                            $('#NoBukti').removeAttr('disabled');
                            $('#btnCashBank').removeAttr('disabled');
                            $('#Description').removeAttr('disabled');
                            $('#ExchangeRateAmount').removeAttr('disabled');
                            $('#AdministrationDateDiv').show();
                            $('#AdministrationDateDiv2').hide();

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
                        url: base_url + "BankAdministration/Unconfirm",
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
            url: base_url + "BankAdministration/Confirm",
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
            url: base_url + "BankAdministration/Delete",
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
            submitURL = base_url + 'BankAdministration/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'BankAdministration/Insert';
        }

        //var e = document.getElementById("IsExpense");
        //var IsExp = e.options[e.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, CashBankId: $("#CashBankId").val(), AdministrationDate: $('#AdministrationDate').datebox('getValue'), Description: $("#Description").val(),
                ExchangeRateAmount: $("#ExchangeRateAmount").numberbox('getValue'), NoBukti: $("#NoBukti").val(), //IsExpense: IsExp,
                Amount: $("#TotalAmount").numberbox('getValue'),
                //BiayaAdminAmount: $("#BiayaAdminAmount").numberbox('getValue'), BiayaBungaAmount: $("#BiayaBungaAmount").numberbox('getValue'),
                //PendapatanJasaAmount: $("#PendapatanJasaAmount").numberbox('getValue'), PendapatanBungaAmount: $("#PendapatanBungaAmount").numberbox('getValue'),
                //PengembalianPiutangAmount: $("#PengembalianPiutangAmount").numberbox('getValue'), 
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
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Code', 'Account Id', 'Account Code', 'Account', 'Status', 'Amount'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 70 },
                  { name: 'accountid', index: 'accountid', width: 130, hidden: true },
                  { name: 'accountcode', index: 'accountcode', width: 80 },
                  { name: 'account', index: 'account', width: 250 },
                  { name: 'status', index: 'status', width: 50 },
                  { name: 'amount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowStatus = $(this).getRowData(cl).status;
		          if (rowStatus == 1) {
		              rowStatus = "Debit";
		          } else {
		              rowStatus = "Credit";
		          }
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });
		      }
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#item_div').dialog('open');
        $('#Amount').numberbox('clear');

    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "BankAdministration/GetInfoDetail?Id=" + id,
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
                            $('#AccountId').val(result.AccountId);
                            $('#Account').val(result.Account);
                            $('#Amount').numberbox('setValue', result.Amount);
                            var e = document.getElementById("Status");
                            if (result.Status == 1) {
                                e.selectedIndex = 0;
                            }
                            else if (result.Status == 2) {
                                e.selectedIndex = 1;
                            }
                            $('#Description').val(result.Description);
                            $('#BankAdministrationDetailId').val(result.Id);
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
                        url: base_url + "BankAdministration/DeleteDetail",
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
                                $('#TotalAmount').numberbox('setValue', result.totalAmount);
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
            submitURL = base_url + 'BankAdministration/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'BankAdministration/InsertDetail';
        }

        var e = document.getElementById("Status");
        var status = e.selectedIndex + 1;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, BankAdministrationId: $("#id").val(), AccountId: $("#AccountId").val(), Status: status,
                Amount: $("#Amount").numberbox('getValue'),
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
                    ReloadGridDetail();
                    $('#TotalAmount').numberbox('setValue', result.totalAmount);
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

    // -------------------------------------------------------Look Up CashBank-------------------------------------------------------
    $('#btnCashBank').click(function () {

        var lookUpURL = base_url + 'MstCashBank/GetList';
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
                  { name: 'address', index: 'address', width: 200 }],
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

            $('#CashBankId').val(ret.id).data("kode", id);
            $('#CashBank').val(ret.name);

            $('#lookup_div_cashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup CashBank----------------------------------------------------------------


    // -------------------------------------------------------Look Up account-------------------------------------------------------
    $('#btnAccount').click(function () {
        var lookUpURL = base_url + 'ChartOfAccount/GetLeaves';
        var lookupGrid = $('#lookup_table_account');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_account').dialog('open');
    });

    jQuery("#lookup_table_account").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Account Code', 'Account Code', 'Account Name', 'Group', 'Level', 'Parent Code', 'Parent Name', 'Legacy', 'CashBank', 'Legacy Code'],
        colModel: [
				  { name: 'Id', index: 'Id', width: 40, hidden: true },
				  { name: 'Code', index: 'Code', width: 80, classes: "grid-col" }, 
                  { name: 'parsecode', index: 'parsecode', hidden:true, width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: "", defaultValue: '0' } },
				  { name: 'name', index: 'name', width: 250 },
                  { name: 'group', index: 'group', width: 90 },
                  { name: 'level', index: 'level', width: 50 },
                  { name: 'parentcode', index: 'parentid', width: 80, classes: "grid-col" },
                  { name: 'parent', index: 'parent', width: 80 },
                  { name: 'islegacy', index: 'islegacy', width: 40, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'iscashbank', index: 'iscashbank', width: 60, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'legacycode', index: 'legacycode', width: 80, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_account'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_account").width() - 10,
        height: $("#lookup_div_account").height() - 110,
    });
    $("#lookup_table_account").jqGrid('navGrid', '#lookup_toolbar_account', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_account').click(function () {
        $('#lookup_div_account').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_account').click(function () {
        var id = jQuery("#lookup_table_account").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_account").jqGrid('getRowData', id);

            $('#AccountId').val(ret.Id).data("kode", id);
            $('#Account').val(ret.name);
            $('#lookup_div_account').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // ---------------------------------------------End Lookup account----------------------------------------------------------------


}); //END DOCUMENT READY