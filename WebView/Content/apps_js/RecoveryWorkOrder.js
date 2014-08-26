$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'RecoveryWorkOrder/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'RecoveryWorkOrder/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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

    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
    $("#lookup_div_coreidentification").dialog('close');
    $("#lookup_div_coreidentificationdetail").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_rollerbuilder").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#wrap_div").dialog('close');
    $("#delete_confirm_div").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'RecoveryWorkOrder/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'CoreIdentification Id', 'Warehouse Id',
                    'Warehouse Code', 'Warehouse Name', 'QTY Received', 'QTY Final',
                    'QTY Rejected', 'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'coreidentificationid', index: 'coreidentificationid', width: 100 },
                  { name: 'warehouseid', index: 'warehouseid', width: 100 },
                  { name: 'warehousecode', index: 'warehousecode', width: 100 },
                  { name: 'warehouse', index: 'warehouse', width: 100 },
                  { name: 'quantityreceived', index: 'quantityreceived', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'quantityfinal', index: 'quantityfinal', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'quantityrejected', index: 'quantityrejected', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100 },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#btnCoreIdentification').removeAttr('disabled');
        $('#btnWarehouse').removeAttr('disabled');
        $('#Code').removeAttr('disabled');
        $('#QuantityReceived').removeAttr('disabled');
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
                url: base_url + "RecoveryWorkOrder/GetInfo?Id=" + id,
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
                            $('#CoreIdentificationId').val(result.CoreIdentificationId);
                            $('#CoreIdentification').val(result.CoreIdentification);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#QuantityReceived').val(result.QuantityReceived);
                            $('#form_btn_save').hide();
                            $('#btnCoreIdentification').attr('disabled', true);
                            $('#btnWarehouse').attr('disabled', true);
                            $('#Code').attr('disabled', true);
                            $('#QuantityReceived').attr('disabled', true);
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
                url: base_url + "RecoveryWorkOrder/GetInfo?Id=" + id,
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
                            $('#CoreIdentificationId').val(result.CoreIdentificationId);
                            $('#CoreIdentification').val(result.CoreIdentification);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#QuantityReceived').val(result.QuantityReceived);
                            $('#btnCoreIdentification').removeAttr('disabled');
                            $('#btnWarehouse').removeAttr('disabled');
                            $('#Code').removeAttr('disabled');
                            $('#QuantityReceived').removeAttr('disabled');
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
                        url: base_url + "RecoveryWorkOrder/Unconfirm",
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
            url: base_url + "RecoveryWorkOrder/Confirm",
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
            url: base_url + "RecoveryWorkOrder/Delete",
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
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'RecoveryWorkOrder/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'RecoveryWorkOrder/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, CoreIdentificationId: $("#CoreIdentificationId").val(),
                WarehouseId: $("#WarehouseId").val(), Code: $("#Code").val(),
                QuantityReceived: $('#QuantityReceived').numberbox('getValue')
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

   
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['CoreIdentificationDetailId', 'RollerBuilder Id', 'RollerBuilder Name', 'CoreTypeCase', 'Acc', 'RepairRequestCase',
                   'CompoundUsage', 'IsRejected', 'Rejected Date', 'Is Finished', 'Finished Date'
        ],
        colModel: [
                  { name: 'coreidentificationdetailid', index: 'coreidentificationdetailid', width: 100, sortable: false },
                  { name: 'rollerbuilderid', index: 'rollerbuilderid', width: 100, sortable: false },
                  { name: 'rollerbuildername', index: 'rollerbuildername', width: 100, sortable: false },
				  { name: 'coretypecase', index: 'coretypecase', width: 100, sortable: false },
                  { name: 'acc', index: 'acc', width: 100, sortable: false },
                  { name: 'repairrequestcase', index: 'repairrequestcase', width: 100, sortable: false },
                  { name: 'compoundusage', index: 'compoundusage', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'isrejected', index: 'isrejected', width: 100, sortable: false },
                  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isfinished', index: 'isfinished', width: 100, sortable: false },
                  { name: 'finisheddate', index: 'finisheddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

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
        width: $(window).width() - 600,
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
                url: base_url + "RecoveryWorkOrder/GetInfoDetail?Id=" + id,
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
                            var e = document.getElementById("RepairRequestCase");
                            if (result.RepairRequestCase == 1) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var f = document.getElementById("CoreTypeCase");
                            if (result.CoreTypeCase == "R") {
                                f.selectedIndex = 0;
                            }
                            else {
                                f.selectedIndex = 1;
                            }
                            $('#CoreIdentificationDetailId').val(result.CoreIdentificationDetailId);
                            $('#CoreIdentificationDetail').val(result.DetailId);
                            $('#CoreBuilderName').val(result.CoreBuilderName);
                            $('#RollerBuilderId').val(result.RollerBuilderId);
                            $('#RollerBuilder').val(result.RollerBuilder);
                            $('#Acc').val(result.Acc);
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
                        url: base_url + "RecoveryWorkOrder/DeleteDetail",
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
    // coreidentification_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'RecoveryWorkOrder/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'RecoveryWorkOrder/InsertDetail';
        }
        var e = document.getElementById("CoreTypeCase");
        var coretypecase = e.options[e.selectedIndex].value;
        var f = document.getElementById("RepairRequestCase");
        var repairrequestcase = f.options[f.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, CoreIdentificationDetailId: $("#CoreIdentificationDetailId").val(), CoreTypeCase: coretypecase, RepairRequestCase: repairrequestcase,
                Acc: $("#Acc").val(), RollerBuilderId: $("#RollerBuilderId").val(), RecoveryOrderId: $("#id").val()
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
        colNames: ['Id', 'Code', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                   { name: 'code', index: 'code', width: 200 },
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
            $('#Warehouse').val(ret.name);

            $('#lookup_div_warehouse').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup warehouse----------------------------------------------------------------


    // -------------------------------------------------------Look Up coreidentification-------------------------------------------------------
    $('#btnCoreIdentification').click(function () {
        var lookUpURL = base_url + 'CoreIdentification/GetList';
        var lookupGrid = $('#lookup_table_coreidentification');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_coreidentification').dialog('open');
    });

    jQuery("#lookup_table_coreidentification").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Warehouse Id', 'Warehouse Code', 'Warehouse Name', 'Contact Id', 'Contact Name',
                    'IsInHouse', 'Quantity', 'Identified Date',
               'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'warehouseid', index: 'coreidentificationreceiptid', width: 100 },
                  { name: 'warehousecode', index: 'coreidentificationreceipt', width: 100 },
                  { name: 'warehouse', index: 'description', width: 100 },
                  { name: 'contactid', index: 'description', width: 100 },
                  { name: 'contact', index: 'description', width: 100 },
                  { name: 'isinhouse', index: 'istax', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'identifieddate', index: 'identifieddate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100 },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_coreidentification'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_coreidentification").width() - 10,
        height: $("#lookup_div_coreidentification").height() - 110,
    });
    $("#lookup_table_coreidentification").jqGrid('navGrid', '#lookup_toolbar_coreidentification', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_coreidentification').click(function () {
        $('#lookup_div_coreidentification').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_coreidentification').click(function () {
        var id = jQuery("#lookup_table_coreidentification").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_coreidentification").jqGrid('getRowData', id);

            $('#CoreIdentificationId').val(ret.id).data("kode", id);
            $('#CoreIdentification').val(ret.code);

            $('#lookup_div_coreidentification').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup coreidentification----------------------------------------------------------------

    // -------------------------------------------------------Look Up coreidentificationdetail-------------------------------------------------------
    $('#btnCoreIdentificationDetail').click(function () {
        var lookUpURL = base_url + 'CoreIdentification/GetListDetail?Id=' + $("#CoreIdentificationId").val();
        var lookupGrid = $('#lookup_table_coreidentificationdetail');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_coreidentificationdetail').dialog('open');
    });

    jQuery("#lookup_table_coreidentificationdetail").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Detail Id', 'RollerIdentificationId', 'Material Case', 'CoreBuilder Id', 'CoreBuilder Name', 'RollerType Id', 'RollerType Name'
                      , 'Machine Id', 'Machine Name', 'RD', 'CD', 'RL', 'WL', 'TL', 'Is Finished', 'Finished Date'
        ],
        colModel: [
                  { name: 'detailid', index: 'detailid', width: 130, sortable: false },
                  { name: 'rolleridentificationid', index: 'rolleridentificationid', width: 130, sortable: false, hidden: true },
                  { name: 'materialcase', index: 'materialcase', width: 130, sortable: false },
                  { name: 'corebuilderid', index: 'corebuilderid', width: 80, sortable: false },
                  { name: 'corebuildername', index: 'corebuildername', width: 80, sortable: false },
                  { name: 'rollertypeid', index: 'rollertypeid', width: 80, sortable: false },
                  { name: 'rollertypename', index: 'rollertypename', width: 80, sortable: false },
                  { name: 'machineid', index: 'machineid', width: 80, sortable: false },
                  { name: 'machinename', index: 'machinename', width: 80, sortable: false },
                  { name: 'rd', index: 'rd', width: 80, sortable: false },
                  { name: 'cd', index: 'cd', width: 80, sortable: false },
                  { name: 'rl', index: 'rl', width: 80, sortable: false },
                  { name: 'wl', index: 'wl', width: 80, sortable: false },
                  { name: 'tl', index: 'tl', width: 80, sortable: false },
                  { name: 'isfinished', index: 'isfinished', width: 80, sortable: false },
                  { name: 'finisheddate', index: 'finisheddate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_coreidentificationdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_coreidentificationdetail").width() - 10,
        height: $("#lookup_div_coreidentificationdetail").height() - 110,
    });
    $("#lookup_table_coreidentificationdetail").jqGrid('navGrid', '#lookup_toolbar_coreidentificationdetail', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_coreidentificationdetail').click(function () {
        $('#lookup_div_coreidentificationdetail').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_coreidentificationdetail').click(function () {
        var id = jQuery("#lookup_table_coreidentificationdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_coreidentificationdetail").jqGrid('getRowData', id);

            $('#CoreIdentificationDetailId').val(id).data("kode", id);
            $('#CoreIdentificationDetail').val(ret.detailid);
            $('#CoreBuilderName').val(ret.corebuildername);

            $('#lookup_div_coreidentificationdetail').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup coreidentificationdetail----------------------------------------------------------------



    // -------------------------------------------------------Look Up rollerbuilder-------------------------------------------------------
    $('#btnRollerBuilder').click(function () {
        var lookUpURL = base_url + 'MstRollerBuilder/GetList';
        var lookupGrid = $('#lookup_table_rollerbuilder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_rollerbuilder').dialog('open');
    });

    jQuery("#lookup_table_rollerbuilder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }
        ],

        page: '1',
        pager: $('#lookup_pager_rollerbuilder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_rollerbuilder").width() - 10,
        height: $("#lookup_div_rollerbuilder").height() - 110,
    });
    $("#lookup_table_rollerbuilder").jqGrid('navGrid', '#lookup_toolbar_rollerbuilder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_rollerbuilder').click(function () {
        $('#lookup_div_rollerbuilder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_rollerbuilder').click(function () {
        var id = jQuery("#lookup_table_rollerbuilder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_rollerbuilder").jqGrid('getRowData', id);

            $('#RollerBuilderId').val(ret.id).data("kode", id);
            $('#RollerBuilder').val(ret.name);

            $('#lookup_div_rollerbuilder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup rollerbuilder----------------------------------------------------------------

}); //END DOCUMENT READY