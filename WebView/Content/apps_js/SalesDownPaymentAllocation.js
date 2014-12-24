$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'SalesDownPaymentAllocation/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'SalesDownPaymentAllocation/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
    $("#lookup_div_salesdownpayment").dialog('close');
    $("#lookup_div_receivable").dialog('close');
    $("#lookup_div_payable").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#ContactId").hide();
    $("#SalesDownPaymentId").hide();
    $("#ReceivableId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'SalesDownPaymentAllocation/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Contact Id', 'Contact Name', 'Payable Id', 'Payable Code', 'Allocation Date',
                   'Total Amount', 'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 70 },
				  { name: 'contactid', index: 'contactid', width: 100, hidden: true },
                  { name: 'contactname', index: 'contactname', width: 150 },
                  { name: 'payableid', index: 'payableid', width: 40 },
                  { name: 'payable', index: 'payable', width: 80 },
                  { name: 'allocationdate', index: 'allocationdate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'totalamount', index: 'totalamount', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
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

		          rowIsGBCH = $(this).getRowData(cl).isgbch;
		          if (rowIsGBCH == 'true') {
		              rowIsGBCH = "YES";
		          } else {
		              rowIsGBCH = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isgbch: rowIsGBCH });
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
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $("#AmountPaid, #Rate").blur(function () {
        var total = parseFloat($('#AmountPaid').numberbox('getValue')) / parseFloat($('#Rate').numberbox('getValue'));
        //total = Math.round(total * 100) / 100;
        $('#Amount').numberbox('setValue', total);
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');

        $('#AllocationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#btnContact').removeAttr('disabled');
        $('#btnSalesDownPayment').removeAttr('disabled');
        $('#btnPayable').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#AllocationDateDiv').show();
        $('#AllocationDateDiv2').hide();
        $('#RateToIDR').removeAttr('disabled');
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
                url: base_url + "SalesDownPaymentAllocation/GetInfo?Id=" + id,
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
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#SalesDownPaymentId').val(result.SalesDownPaymentId);
                            $('#CurrencyCashBank').val(result.currency);
                            $('#PayableId').val(result.PayableId);
                            $('#Payable').val(result.Payable);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            $('#RateToIDR').numberbox('setValue', result.RateToIDR);
                            $('#AllocationDate').datebox('setValue', dateEnt(result.AllocationDate));
                            $('#AllocationDate2').val(dateEnt(result.AllocationDate));
                            $('#AllocationDateDiv2').show();
                            $('#AllocationDateDiv').hide();
                            $('#form_btn_save').hide();
                            $('#btnContact').attr('disabled', true);
                            $('#btnPayable').attr('disabled', true);
                            $('#TotalAmount').attr('disabled', true);
                            $('#RateToIDR').attr('disabled', true);
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
                url: base_url + "SalesDownPaymentAllocation/GetInfo?Id=" + id,
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
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#SalesDownPaymentId').val(result.SalesDownPaymentId);
                            $('#CurrencyCashBank').val(result.currency);
                            $('#PayableId').val(result.PayableId);
                            $('#Payable').val(result.Payable);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            $('#RateToIDR').numberbox('setValue', result.RateToIDR);
                            $('#AllocationDate').datebox('setValue', dateEnt(result.AllocationDate));
                            $('#AllocationDate2').val(dateEnt(result.AllocationDate));
                            $('#AllocationDateDiv2').show();
                            $('#AllocationDateDiv').hide();
                            $('#btnContact').removeAttr('disabled');
                            $('#RateToIDR').removeAttr('disabled');
                            $('#btnPayable').removeAttr('disabled');
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
                        url: base_url + "SalesDownPaymentAllocation/Unconfirm",
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
            url: base_url + "SalesDownPaymentAllocation/Confirm",
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
            url: base_url + "SalesDownPaymentAllocation/Delete",
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
        if ($('#AllocationDate').datebox('getValue') == "") {
            return $($('#AllocationDate').addClass('errormessage').before('<span class="errormessage">**' + "Payment Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'SalesDownPaymentAllocation/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'SalesDownPaymentAllocation/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ContactId: $("#ContactId").val(), SalesDownPaymentId: $("#SalesDownPaymentId").val(),
                AllocationDate: $('#AllocationDate').datebox('getValue'), PayableId: $("#PayableId").val(), RateToIDR: $("#RateToIDR").numberbox('getValue')
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
        colNames: ['Code', 'Receivable Id', 'Receivable Code', 'Amount', 'Description'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'receivableid', index: 'receivableid', width: 130, sortable: false, hidden: true },
                  { name: 'receivable', index: 'receivable', width: 90, sortable: false },
                  { name: 'amount', index: 'amount', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
                  { name: 'description', index: 'description', width: 180, sortable: false }
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
                url: base_url + "SalesDownPaymentAllocation/GetInfoDetail?Id=" + id,
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
                            $('#Currency').text(result.currency);
                            $('#Remaining').numberbox('setValue', result.Remaining);
                            $('#Description').val(result.Description);
                            $('#SalesDownPaymentAllocationDetailId').val(result.SalesDownPaymentAllocationDetailId);
                            if (result.currency == $('#CurrencyCashBank').val()) {
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
                        url: base_url + "SalesDownPaymentAllocation/DeleteDetail",
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
                                $("#TotalAmount").numberbox('setValue', result.totalAmount)
                                ReloadGrid();
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
            submitURL = base_url + 'SalesDownPaymentAllocation/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'SalesDownPaymentAllocation/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, SalesDownPaymentAllocationId: $("#id").val(), ReceivableId: $("#ReceivableId").val(), Description: $("#Description").val(),
                AmountPaid: $("#AmountPaid").numberbox('getValue'), Rate: $("#Rate").numberbox('getValue'),
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
                    ReloadGridDetail();
                    ReloadGrid();
                    $("#TotalAmount").numberbox('setValue', result.totalAmount)
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
        var lookUpURL = base_url + 'MstContact/GetListCustomer';
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
        colNames: ['ID', 'Name'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'name', index: 'name', width: 150 },
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
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
            $('#Contact').val(ret.code);
            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up payable-------------------------------------------------------
    $('#btnPayable').click(function () {
        var lookUpURL = base_url + 'Payable/GetListSalesDownPayment';
        var lookupGrid = $('#lookup_table_payable');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_payable').dialog('open');
    });

    jQuery("#lookup_table_payable").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Contact Id', 'Contact', 'Payable Source', 'Id',
                   'Due Date', 'Total', 'Currency', 'Remaining', 'PendClearance'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 55, sortable: false },
                  { name: 'contactid', index: 'contactid', width: 100, sortable: false, hidden: true },
                  { name: 'contactname', index: 'contactname', width: 150, sortable: false, hidden: true },
                  { name: 'payablesource', index: 'payablesource', width: 100, sortable: false },
                  { name: 'payablesourceid', index: 'payablesourceid', width: 40, align: 'right', sortable: false },
                  { name: 'duedate', index: 'duedate', search: false, width: 70, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'remainingamount', index: 'remainingamount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', align: 'right', width: 0, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
        ],
        page: '1',
        pager: $('#lookup_pager_payable'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_payable").width() - 10,
        height: $("#lookup_div_payable").height() - 110,
    });
    $("#lookup_table_payable").jqGrid('navGrid', '#lookup_toolbar_payable', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_payable').click(function () {
        $('#lookup_div_payable').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_payable').click(function () {
        var id = jQuery("#lookup_table_payable").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_payable").jqGrid('getRowData', id);
            $('#CurrencyCashBank').val(ret.currency);

            $('#PayableId').val(id).data("kode", id);
            $('#Payable').val(ret.code);
            $('#lookup_div_payable').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup payable----------------------------------------------------------------

    // -------------------------------------------------------Look Up receivable-------------------------------------------------------
    $('#btnReceivable').click(function () {
        var lookUpURL = base_url + 'Receivable/GetListSalesInvoice';
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
        colNames: ['Code', 'Contact Id', 'Contact', 'Receivable Source', 'Id',
                    'Due Date', 'Total', 'Currency', 'Remaining', 'PendClearance'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 55, sortable: false },
                  { name: 'contactid', index: 'contactid', width: 100, sortable: false, hidden: true },
                  { name: 'contactname', index: 'contactname', width: 150, sortable: false, hidden: true },
                  { name: 'receivablesource', index: 'receivablesource', width: 100, sortable: false },
                  { name: 'receivablesourceid', index: 'receivablesourceid', width: 40, align: 'right', sortable: false },
                  { name: 'duedate', index: 'duedate', search: false, width: 70, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, sortable: false },
                  { name: 'amount', index: 'amount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'currency', index: 'currency', width: 100 },
                  { name: 'remainingamount', index: 'remainingamount', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', align: 'right', width: 0, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
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
    $("#lookup_table_receivable").jqGrid('navGrid', '#lookup_toolbar_receivable', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
                $('#Rate').numberbox('setValue', 1);
            }
            else {
                $('#Rate').removeAttr('disabled');
                $('#Rate').numberbox('setValue', 1);
            }
            $('#Remaining').numberbox('setValue', ret.remainingamount);
            $('#lookup_div_receivable').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup receivable----------------------------------------------------------------


}); //END DOCUMENT READY