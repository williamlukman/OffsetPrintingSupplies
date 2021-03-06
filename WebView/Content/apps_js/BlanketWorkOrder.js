﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode,
        currentpage = 'last',
		currentpagedetail = 'first' ;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid(setpage) {
        $("#list").setGridParam({ url: base_url + 'BlanketWorkOrder/GetList', postData: { filters: null }, page: setpage }).trigger("reloadGrid");
    }

    function ReloadGridDetail(setpagedetail) {
        $("#listdetail").setGridParam({ url: base_url + 'BlanketWorkOrder/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: setpagedetail }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#form_btn_save').data('kode', '');
        $('#item_btn_submit').data('kode', '');
        $('#copy_btn_submit').data('kode', '');
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

    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#lookup_div_blanket").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#rejected_div").dialog('close');
    $("#finished_div").dialog('close');
    $("#copy_div").dialog('close');
    $("#ContactId").hide();
    $("#WarehouseId").hide();
    $("#BlanketSku").hide();
    $("#RollBlanketSku").hide();
    $("#BlanketLeftBarSku").hide();
    $("#BlanketRightBarSku").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'BlanketWorkOrder/GetList',
        datatype: "json",
        colNames: ['ID', 'Order No.', 'Production No', 'Contact', 'Warehouse', 'QTY', 'QTY Finished',
                    'QTY Rejected', 'Order Date', 'Due Date', 'Confirmation Date', 'Notes', 'Is Completed', 'Created At', 'Updated At'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 75 },
                  { name: 'productionno', index: 'productionno', width: 75 },
	              { name: 'contact', index: 'contact', width: 200 },
                  { name: 'warehouse', index: 'warehouse', width: 100 },
                  { name: 'quantityreceived', index: 'quantityreceived', align: 'right', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'quantityfinal', index: 'quantityfinal', align: 'right', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'quantityrejected', index: 'quantityrejected', align: 'right', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'orderdate', index: 'orderdate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'duedate', index: 'duedate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'notes', index: 'notes', width: 150 },
                  { name: 'iscompleted', index: 'iscompleted', width: 100 },
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
        sortorder: "ASC",
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
        ReloadGrid('first');
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#OrderDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#OrderDateDiv').show();
        $('#OrderDateDiv2').hide();
        $('#HasDueDate').removeAttr('disabled');
        $('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#DueDateDiv').hide();
        $('#DueDateDiv2').show();
        $('#btnContact').removeAttr('disabled');
        $('#btnWarehouse').removeAttr('disabled');
        $('#Code').removeAttr('disabled');
        $('#ProductionNo').removeAttr('disabled');
        $('#Notes').removeAttr('disabled');
        //$('#QuantityReceived').removeAttr('disabled');
        $('#tabledetail_div').hide();
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
                url: base_url + "BlanketWorkOrder/GetInfo?Id=" + id,
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
                            $('#ProductionNo').val(result.ProductionNo);
                            $('#Notes').val(result.Notes);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#QuantityReceived').numberbox('setValue', result.QuantityReceived);
                            $('#form_btn_save').hide();
                            $('#btnContact').attr('disabled', true);
                            $('#btnWarehouse').attr('disabled', true);
                            $('#Code').attr('disabled', true);
                            $('#ProductionNo').attr('disabled', true);
                            $('#Notes').attr('disabled', true);
                            $('#QuantityReceived').attr('disabled', true);
                            document.getElementById("HasDueDate").checked = result.HasDueDate;
                            $('#OrderDate').datebox('setValue', dateEnt(result.OrderDate));
                            $('#OrderDate2').val(dateEnt(result.OrderDate));
                            $('#OrderDateDiv').hide();
                            $('#OrderDateDiv2').show();
                            $('#HasDueDate').attr('disabled', true);
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#DueDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#tabledetail_div').show();
                            ReloadGridDetail('first');
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
                url: base_url + "BlanketWorkOrder/GetInfo?Id=" + id,
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
                            $('#ProductionNo').val(result.ProductionNo);
                            $('#Notes').val(result.Notes);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#QuantityReceived').numberbox('setValue', result.QuantityReceived);
                            $('#Code').removeAttr('disabled');
                            $('#ProductionNo').removeAttr('disabled');
                            $('#Notes').removeAttr('disabled');
                            $('#HasDueDate').removeAttr('disabled');
                            document.getElementById("HasDueDate").checked = result.HasDueDate;
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            if (document.getElementById("HasDueDate").checked) {
                                $('#DueDateDiv').show();
                                $('#DueDateDiv2').hide();
                            }
                            else {
                                $('#DueDateDiv').hide();
                                $('#DueDateDiv2').show();
                            }
                            $('#OrderDate').datebox('setValue', dateEnt(result.OrderDate));
                            $('#OrderDate2').val(dateEnt(result.OrderDate));
                            $('#OrderDateDiv').show();
                            $('#OrderDateDiv2').hide();
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
                            $('#form_div').dialog('open');
                            if (result.IsConfirmed) {
                                $('#btnContact').attr('disabled', true);
                                $('#btnWarehouse').attr('disabled', true);
                                $('#QuantityReceived').attr('disabled', true);
                            }
                            else {
                                $('#btnContact').removeAttr('disabled');
                                $('#btnWarehouse').removeAttr('disabled');
                                //$('#QuantityReceived').removeAttr('disabled');
                            }
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#HasDueDate').click(function () {
        if (document.getElementById("HasDueDate").checked) {
            $('#DueDateDiv').show();
            $('#DueDateDiv2').hide();
        } else {
            $('#DueDateDiv').hide();
            $('#DueDateDiv2').show();
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
                        url: base_url + "BlanketWorkOrder/Unconfirm",
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
                                ReloadGrid($('#list').getGridParam('page'));
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
            url: base_url + "BlanketWorkOrder/Confirm",
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
                    ReloadGrid($('#list').getGridParam('page'));
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
            url: base_url + "BlanketWorkOrder/Delete",
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
                    ReloadGrid($('#list').getGridParam('page'));
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

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'BlanketWorkOrder/Update';
            currentpage = $('#list').getGridParam('page');
        }
            // Insert
        else {
            submitURL = base_url + 'BlanketWorkOrder/Insert';
            currentpage = 'first';
        }

        var duedate = (document.getElementById("HasDueDate").checked) ? $('#DueDate').datebox('getValue') : null;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ContactId: $("#ContactId").val(),
                WarehouseId: $("#WarehouseId").val(), Code: $("#Code").val(),
                ProductionNo: $("#ProductionNo").val(), //QuantityReceived: $('#QuantityReceived').numberbox('getValue'),
                DueDate: duedate, HasDueDate: document.getElementById("HasDueDate").checked,
                OrderDate: $('#OrderDate').datebox('getValue'), Notes: $('#Notes').val(),
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
                    ReloadGrid(currentpage);
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Id', 'Sku', 'Name', 'Sku', 'Nama',
                   'Sku', 'Name', 'Sku', 'Nama',
                   'Rejected Date' ,'Finished Date' 
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'blanketsku', align: 'right', index: 'blanketsku', width: 50, sortable: false },
                  { name: 'blanketname', index: 'blanketname', width: 300, sortable: false },
                  { name: 'rollBlanketsku', align:'right', index: 'rollBlanketsku', width: 50, sortable: false },
                  { name: 'rollBlanketname', index: 'rollBlanketname', width: 200, sortable: false },
                  { name: 'leftbarsku', align: 'right', index: 'leftbarsku', width: 50, sortable: false },
                  { name: 'leftbarname', index: 'leftbarname', width: 200, sortable: false },
                  { name: 'rightbarsku', align: 'right', index: 'rightbarsku', width: 50, sortable: false },
                  { name: 'rightbarname', index: 'rightbarname', width: 200, sortable: false },
                  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'finisheddate', index: 'finisheddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

        ],
        page: currentpagedetail,
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
    $("#listdetail").jqGrid('navGrid', '#pagerdetail', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    jQuery("#listdetail").jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [
          { startColumnName: 'blanketsku', numberOfColumns: 2, titleText: 'Blanket'},
          { startColumnName: 'rollBlanketsku', numberOfColumns: 2, titleText: 'RollBlanket'},
          { startColumnName: 'leftbarsku', numberOfColumns: 2, titleText: 'LeftBar'},
          { startColumnName: 'rightbarsku', numberOfColumns: 2, titleText: 'RightBar'},

        ]
    });


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
                url: base_url + "BlanketWorkOrder/GetInfoDetail?Id=" + id,
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
                            $('#BlanketSku').val(result.BlanketSku);
                            $('#Blanket').val(result.Blanket);
                            $('#RollBlanketSku').val(result.RollBlanketSku);
                            $('#RollBlanket').val(result.RollBlanket);
                            $('#BlanketLeftBarSku').val(result.BlanketLeftBarSku);
                            $('#BlanketLeftBar').val(result.BlanketLeftBar);
                            $('#BlanketRightBarSku').val(result.BlanketRightBarSku);
                            $('#BlanketRightBar').val(result.BlanketRightBar);
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
                        url: base_url + "BlanketWorkOrder/DeleteDetail",
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
                                $('#QuantityReceived').numberbox('setValue', result.QuantityReceived);
                                ReloadGridDetail('first');
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

    $('#btn_copy_detail').click(function () {
        ClearData();
        clearForm("#copy_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "BlanketWorkOrder/GetInfoDetail?Id=" + id,
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
                            $("#copy_btn_submit").data('kode', result.BlanketOrderId);
                            $("#skucopy").val(result.BlanketSku);
                            $('#BlanketSku').val(result.BlanketSku).data('kode', result.BlanketId);
                            $('#Blanket').val(result.Blanket);
                            $('#RollBlanketSku').val(result.RollBlanketSku);
                            $('#RollBlanket').val(result.RollBlanket);
                            $('#BlanketLeftBarSku').val(result.BlanketLeftBarSku);
                            $('#BlanketLeftBar').val(result.BlanketLeftBar);
                            $('#BlanketRightBarSku').val(result.BlanketRightBarSku);
                            $('#BlanketRightBar').val(result.BlanketRightBar);
                            $('#copy_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#copy_btn_cancel').click(function () {
        $('#copy_btn_cancel').val('');
        $("#copy_div").dialog('close');
    });

    $('#copy_btn_submit').click(function () {
        $.ajax({
            url: base_url + "BlanketWorkOrder/CopyDetail",
            contentType: "application/json",
            type: 'POST',
            data: JSON.stringify({
                BlanketId: $("#BlanketSku").data('kode'), BlanketOrderId: $("#copy_btn_submit").data('kode'),
                TotalCopy: $("#TotalCopy").numberbox('getValue')
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
                    $('#QuantityReceived').numberbox('setValue', result.QuantityReceived);
                    ReloadGridDetail('last');
                    $("#copy_div").dialog('close')
                }
            }
        });
    });

    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // coreidentification_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'BlanketWorkOrder/UpdateDetail';
            currentpagedetail = $('#listdetail').getGridParam('page');
        }
            // Insert
        else {
            submitURL = base_url + 'BlanketWorkOrder/InsertDetail';
            currentpagedetail = 'first';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, BlanketId: $("#BlanketSku").data('kode'), BlanketOrderId: $("#id").val(),
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
                    $('#QuantityReceived').numberbox('setValue', result.QuantityReceived);
                    ReloadGridDetail(currentpagedetail);
                    $("#item_div").dialog('close')
                }
            }
        });
    });


    // coreidentification_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up warehouse-------------------------------------------------------
    $('#btnWarehouse').click(function () {
        var lookUpURL = base_url + 'MstWarehouse/GetList';
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
        colNames: ['Id', 'Code','Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                   { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 200 }],
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
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

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
            $('#Warehouse').val(ret.name);

            $('#lookup_div_warehouse').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup warehouse----------------------------------------------------------------


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
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 250 }],
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


    // -------------------------------------------------------Look Up blanket, rollBlanket, bars-------------------------------------------------------
    $('#btnBlanket').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListLookUpItems';
        var lookupGrid = $('#lookup_table_blanket');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_blanket').dialog('open');
    });

    jQuery("#lookup_table_blanket").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Sku', 'Name', 'RollBlanket Sku', 'Name', 'Left Bar Sku', 'Name', 'Right Bar Sku', 'Name'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 50 },
				  { name: 'name', index: 'name', width: 400 },
                  { name: 'rollBlanketsku', index: 'rollBlanketsku', width: 50 },
				  { name: 'rollBlanketname', index: 'rollBlanketname', width: 300 },
                  { name: 'leftbarsku', index: 'leftbarsku', width: 50 },
				  { name: 'leftbarname', index: 'leftbarname', width: 200 },
                  { name: 'rightbarsku', index: 'rightbarsku', width: 50 },
				  { name: 'rightbarname', index: 'rightbarname', width: 200 },

        ],
        page: '1',
        pager: $('#lookup_pager_blanket'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_blanket").width() - 10,
        height: $("#lookup_div_blanket").height() - 110,
    });
    $("#lookup_table_blanket").jqGrid('navGrid', '#lookup_toolbar_blanket', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_blanket').click(function () {
        $('#lookup_div_blanket').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_blanket').click(function () {
        var id = jQuery("#lookup_table_blanket").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_blanket").jqGrid('getRowData', id);

            $('#BlanketSku').val(ret.sku).data("kode", id);
            $('#Blanket').val(ret.name);
            $('#RollBlanketSku').val(ret.rollBlanketsku);
            $('#RollBlanket').val(ret.rollBlanketname);
            $('#BlanketLeftBarSku').val(ret.leftbarsku);
            $('#BlanketLeftBar').val(ret.leftbarname);
            $('#BlanketRightBarSku').val(ret.rightbarsku);
            $('#BlanketRightBar').val(ret.rightbarname);
            $('#lookup_div_blanket').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup blanket, rollBlanket, bars----------------------------------------------------------------

}); //END DOCUMENT READY