$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'RollerWarehouseMutation/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'RollerWarehouseMutation/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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

    $("#recoveryworkorder_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
    $("#lookup_div_recoveryworkorder").dialog('close');
    $("#lookup_div_recoveryworkorderdetail").dialog('close');
    $("#lookup_div_warehouseto").dialog('close');
    $("#lookup_div_warehousefrom").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#RecoveryWorkOrderId").hide();
    $("#WarehouseFromId").hide();
    $("#WarehouseToId").hide();
    $("#RecoveryWorkOrderDetailId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'RollerWarehouseMutation/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'RollerIdentification Id', 'RIF', 'WarehouseFrom Id',
                    'WarehouseFrom Code', 'From', 'WarehouseTo Id', 'WarehouseTo Code',
                    'To','QTY','Is Confirmed','Confirmation Date','Created At','Updated At'
        ],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 60 },
				  { name: 'rolleridentificationid', index: 'rolleridentificationid', width: 100, hidden: true },
                  { name: 'rolleridentificationcode', index: 'rolleridentificationcode', width: 60 },
                  { name: 'warehousefromid', index: 'warehousefromid', width: 100, hidden: true },
                  { name: 'warehousefromcode', index: 'warehousefromcode', width: 100, hidden: true },
                  { name: 'warehousefrom', index: 'warehousefrom', width: 180 },
                  { name: 'warehousetoid', index: 'warehousetoid', width: 100, hidden: true },
                  { name: 'warehousetocode', index: 'warehousetocode', width: 100, hidden: true },
                  { name: 'warehouseto', index: 'warehouseto', width: 180 },
                  { name: 'quantity', index: 'quantity', width: 40, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
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
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#btnWarehouseFrom').removeAttr('disabled');
        $('#btnWarehouseTo').removeAttr('disabled');
        $('#btnRecoveryWorkOrder').removeAttr('disabled');
        $('#Quantity').removeAttr('disabled');
        $('#MutationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#MutationDateDiv').show();
        $('#MutationDateDiv2').hide();
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
                url: base_url + "RollerWarehouseMutation/GetInfo?Id=" + id,
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
                            $('#RecoveryWorkOrderId').val(result.RecoveryOrderId);
                            $('#RecoveryWorkOrder').val(result.RecoveryOrder);
                            $('#WarehouseFromId').val(result.WarehouseFromId);
                            $('#WarehouseFrom').val(result.WarehouseFrom);
                            $('#WarehouseToId').val(result.WarehouseToId);
                            $('#WarehouseTo').val(result.WarehouseTo);
                            $('#Quantity').val(result.Quantity);
                            $('#form_btn_save').hide();
                            $('#btnRecoveryWorkOrder').attr('disabled', true);
                            $('#btnWarehouseFrom').attr('disabled', true);
                            $('#btnWarehouseTo').attr('disabled', true);
                            $('#Quantity').attr('disabled', true);
                            $('#MutationDate').datebox('setValue', dateEnt(result.MutationDate));
                            $('#MutationDate2').val(dateEnt(result.MutationDate));
                            $('#MutationDateDiv2').show();
                            $('#MutationDateDiv').hide();
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
                url: base_url + "RollerWarehouseMutation/GetInfo?Id=" + id,
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
                            $('#RecoveryWorkOrderId').val(result.RecoveryOrderId);
                            $('#RecoveryWorkOrder').val(result.RecoveryOrder);
                            $('#WarehouseFromId').val(result.WarehouseFromId);
                            $('#WarehouseFrom').val(result.WarehouseFrom);
                            $('#WarehouseToId').val(result.WarehouseToId);
                            $('#WarehouseTo').val(result.WarehouseTo);
                            $('#Quantity').val(result.Quantity);
                            $('#MutationDate').datebox('setValue', dateEnt(result.MutationDate));
                            $('#MutationDate2').val(dateEnt(result.MutationDate));
                            $('#MutationDateDiv2').hide();
                            $('#MutationDateDiv').show();
                            $('#btnWarehouseFrom').removeAttr('disabled');
                            $('#btnWarehouseTo').removeAttr('disabled');
                            $('#btnRecoveryWorkOrder').removeAttr('disabled');
                            $('#Quantity').removeAttr('disabled');
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
                        url: base_url + "RollerWarehouseMutation/Unconfirm",
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
            url: base_url + "RollerWarehouseMutation/Confirm",
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
            url: base_url + "RollerWarehouseMutation/Delete",
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
            submitURL = base_url + 'RollerWarehouseMutation/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'RollerWarehouseMutation/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, RecoveryOrderId: $("#RecoveryWorkOrderId").val(),
                WarehouseToId: $("#WarehouseToId").val(), WarehouseFromId: $("#WarehouseFromId").val(),
                Quantity: $('#Quantity').numberbox('getValue'), MutationDate: $('#MutationDate').datebox('getValue'),
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
        colNames: ['RIF ID','Code', 'RollerWarehouseMutation Id','RWOD Id', 'Item Id', 'Sku', 'Item'
        ],
        colModel: [
                  { name: 'rifid', index: 'rifid', width: 70, sortable: false, align: 'right' },
                  { name: 'code', index: 'code', width: 70, sortable: false, align: 'right' },
                  { name: 'rollerwarehousemutationid', index: 'rollerwarehousemutationid', width: 100, sortable: false, hidden: true },
                  { name: 'recoveryworkorderdetailid', index: 'recoveryworkorderdetailid', width: 60, align: 'right', sortable: false },
				  { name: 'itemid', index: 'itemid', width: 100, sortable: false, hidden: true },
				  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false},
                  { name: 'itemname', index: 'itemname', width: 150, sortable: false },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'rifid',
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
                url: base_url + "RollerWarehouseMutation/GetInfoDetail?Id=" + id,
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
                            $('#RecoveryWorkOrderDetailId').val(result.RIFID).data("kode", result.RecoveryOrderDetailId);
                            $('#RecoveryWorkOrderDetail').val(result.Item);
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
                        url: base_url + "RollerWarehouseMutation/DeleteDetail",
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
    // recoveryworkorder_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');
    
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'RollerWarehouseMutation/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'RollerWarehouseMutation/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, RecoveryOrderDetailId: $("#RecoveryWorkOrderDetailId").data('kode'), RollerWarehouseMutationId: $("#id").val()
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


    // recoveryworkorder_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up warehouseto-------------------------------------------------------
    $('#btnWarehouseTo').click(function () {
        var lookUpURL = base_url + 'MstWarehouse/GetList';
        var lookupGrid = $('#lookup_table_warehouseto');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_warehouseto').dialog('open');
    });

    jQuery("#lookup_table_warehouseto").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Code', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                   { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_warehouseto'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_warehouseto").width() - 10,
        height: $("#lookup_div_warehouseto").height() - 110,
    });
    $("#lookup_table_warehouseto").jqGrid('navGrid', '#lookup_toolbar_warehouseto', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_warehouseto').click(function () {
        $('#lookup_div_warehouseto').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_warehouseto').click(function () {
        var id = jQuery("#lookup_table_warehouseto").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_warehouseto").jqGrid('getRowData', id);

            $('#WarehouseToId').val(ret.id).data("kode", id);
            $('#WarehouseTo').val(ret.name);

            $('#lookup_div_warehouseto').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup warehouseto----------------------------------------------------------------


    // -------------------------------------------------------Look Up warehousefrom-------------------------------------------------------
    $('#btnWarehouseFrom').click(function () {
        var lookUpURL = base_url + 'MstWarehouse/GetList';
        var lookupGrid = $('#lookup_table_warehousefrom');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_warehousefrom').dialog('open');
    });

    jQuery("#lookup_table_warehousefrom").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Code', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                   { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_warehousefrom'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_warehousefrom").width() - 10,
        height: $("#lookup_div_warehousefrom").height() - 110,
    });
    $("#lookup_table_warehousefrom").jqGrid('navGrid', '#lookup_toolbar_warehousefrom', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_warehousefrom').click(function () {
        $('#lookup_div_warehousefrom').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_warehousefrom').click(function () {
        var id = jQuery("#lookup_table_warehousefrom").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_warehousefrom").jqGrid('getRowData', id);
           
            $('#WarehouseFromId').val(ret.id).data("kode", id);
            $('#WarehouseFrom').val(ret.name);

            $('#lookup_div_warehousefrom').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup warehousefrom----------------------------------------------------------------


    // -------------------------------------------------------Look Up recoveryworkorder-------------------------------------------------------
    $('#btnRecoveryWorkOrder').click(function () {
        var lookUpURL = base_url + 'RecoveryWorkOrder/GetList';
        var lookupGrid = $('#lookup_table_recoveryworkorder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_recoveryworkorder').dialog('open');
    });

    jQuery("#lookup_table_recoveryworkorder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_recoveryworkorder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_recoveryworkorder").width() - 10,
        height: $("#lookup_div_recoveryworkorder").height() - 110,
    });
    $("#lookup_table_recoveryworkorder").jqGrid('navGrid', '#lookup_toolbar_recoveryworkorder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_recoveryworkorder').click(function () {
        $('#lookup_div_recoveryworkorder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_recoveryworkorder').click(function () {
        var id = jQuery("#lookup_table_recoveryworkorder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_recoveryworkorder").jqGrid('getRowData', id);
        
            $('#RecoveryWorkOrderId').val(ret.id).data("kode", id);
            $('#RecoveryWorkOrder').val(ret.name);

            $('#lookup_div_recoveryworkorder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup recoveryworkorder----------------------------------------------------------------

    // -------------------------------------------------------Look Up recoveryworkorderdetail-------------------------------------------------------
    $('#btnRecoveryWorkOrderDetail').click(function () {
        var lookUpURL = base_url + 'RecoveryWorkOrder/GetListDetailFinishedNotDelivered?id=' + $('#RecoveryWorkOrderId').val();
        var lookupGrid = $('#lookup_table_recoveryworkorderdetail');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_recoveryworkorderdetail').dialog('open');
    });

    jQuery("#lookup_table_recoveryworkorderdetail").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['RIF Id', 'Detail Id', 'Material', 'RollerBuilder Id', 'Roller Sku', 'Roller'
        ],
        colModel: [
                  { name: 'detailid', index: 'detailid', width: 40, sortable: false, align: 'right' },
                  { name: 'rolleridentificationdetailid', index: 'rolleridentificationdetailid', width: 130, sortable: false, hidden: true },
                  { name: 'materialcase', index: 'materialcase', width: 50, sortable: false },
                  { name: 'rollerbuilderid', index: 'rollerbuilderid', width: 80, sortable: false, hidden: true },
                  { name: 'rollerbuildersku', index: 'rollerbuildersku', width: 60, sortable: false },
                  { name: 'rollerbuildername', index: 'rollerbuildername', width: 100, sortable: false },
        ],
        page: '1',
        pager: $('#lookup_pager_recoveryworkorderdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_recoveryworkorderdetail").width() - 10,
        height: $("#lookup_div_recoveryworkorderdetail").height() - 110,
    });
    $("#lookup_table_recoveryworkorderdetail").jqGrid('navGrid', '#lookup_toolbar_recoveryworkorderdetail', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_recoveryworkorderdetail').click(function () {
        $('#lookup_div_recoveryworkorderdetail').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_recoveryworkorderdetail').click(function () {
        var id = jQuery("#lookup_table_recoveryworkorderdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_recoveryworkorderdetail").jqGrid('getRowData', id);

            $('#RecoveryWorkOrderDetailId').val(ret.detailid).data("kode", id);
            $('#RecoveryWorkOrderDetail').val(ret.rollerbuildername);

            $('#lookup_div_recoveryworkorderdetail').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup recoveryworkorderdetail----------------------------------------------------------------

}); //END DOCUMENT READY