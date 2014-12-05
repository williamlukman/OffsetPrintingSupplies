$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'TemporaryDeliveryOrder/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'TemporaryDeliveryOrder/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
    $("#push_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_previousorder").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#PreviousOrderId").hide();
    $("#WarehouseId").hide();
    $("#ItemId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'TemporaryDeliveryOrder/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Nomor Surat', 'Type', 'Order Id', 'Order Code', 'Warehouse Id', 'Warehouse', 'Delivery Date',
                    'Is Confirmed', 'Confirmation Date', 'Reconciled', 'Is Pushed', 'Push Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 60 },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 120 },
                  { name: 'ordertype', index: 'ordertype', width: 70 },
				  { name: 'orderid', index: 'orderid', width: 100, hidden: true },
                  { name: 'ordercode', index: 'ordercode', width: 70 },
                  { name: 'warehouseid', index: 'warehouseid', width: 100, hidden: true },
                  { name: 'warehousename', index: 'warehousename', width: 130 },
                  { name: 'deliverydate', index: 'deliverydate', width: 100, search: false, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isreconciled', index: 'isreconciled', width: 60 },
                  { name: 'ispushed', index: 'ispushed', width: 100, hidden: true },
                  { name: 'pushdate', index: 'pushdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		              rowIsConfirmed = "Y";
		          } else {
		              rowIsConfirmed = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isconfirmed: rowIsConfirmed });

		          rowIsReconciled = $(this).getRowData(cl).isreconciled;
		          if (rowIsReconciled == 'true') {
		              rowIsReconciled = "Y";
		          } else {
		              rowIsReconciled = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isreconciled: rowIsReconciled });

		          rowOrderType = $(this).getRowData(cl).ordertype;
		          if (rowOrderType == '0') {
		              rowOrderType = "Trial";
		          } else if (rowOrderType == '1') {
		              rowOrderType = "Sample";
		          } else {
		              rowOrderType = "Part Delivery";
		          }
		          $(this).jqGrid('setRowData', ids[i], { ordertype: rowOrderType });
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
                url: base_url + "TemporaryDeliveryOrder/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else if (result.ConfirmationDate == null) {
                        $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    }
                    else {
                        window.open(base_url + "Report/ReportTemporaryDeliveryOrder?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#DeliveryDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#btnPreviousOrder').removeAttr('disabled');
        $('#btnWarehouse').removeAttr('disabled');
        $('#OrderType').removeAttr('disabled');
        $('#NomorSurat').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#DeliveryDateDiv').show();
        $('#DeliveryDateDiv2').hide();
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
                url: base_url + "TemporaryDeliveryOrder/GetInfo?Id=" + id,
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
                            document.getElementById("OrderType").selectedIndex = result.OrderType;
                            $('#PreviousOrderId').val(result.OrderId);
                            $('#PreviousOrder').val(result.OrderCode);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#DeliveryDate').datebox('setValue', dateEnt(result.DeliveryDate));
                            $('#DeliveryDate2').val(dateEnt(result.DeliveryDate));
                            $('#DeliveryDateDiv2').show();
                            $('#DeliveryDateDiv').hide();
                            $('#form_btn_save').hide();
                            $('#OrderType').attr('disabled', true);
                            $('#btnPreviousOrder').attr('disabled', true);
                            $('#btnWarehouse').attr('disabled', true);
                            $('#NomorSurat').attr('disabled', true);
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
                url: base_url + "TemporaryDeliveryOrder/GetInfo?Id=" + id,
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
                            $('#PreviousOrderId').val(result.OrderId);
                            $('#PreviousOrder').val(result.OrderCode);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#DeliveryDate').datebox('setValue', dateEnt(result.DeliveryDate));
                            $('#DeliveryDate2').val(dateEnt(result.DeliveryDate));
                            $('#btnPreviousOrder').removeAttr('disabled');
                            $('#btnWarehouse').removeAttr('disabled');
                            $('#NomorSurat').removeAttr('disabled');
                            $('#tabledetail_div').hide();
                            $('#DeliveryDateDiv2').show();
                            $('#DeliveryDateDiv').hide();
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
                        url: base_url + "TemporaryDeliveryOrder/Unconfirm",
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
            url: base_url + "TemporaryDeliveryOrder/Confirm",
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

    $('#btn_forward').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#PushDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idpush').val(ret.id);
            $("#push_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#push_btn_submit').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to create delivery order?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "TemporaryDeliveryOrder/Push",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: $('#idpush').val(), ConfirmationDate: $('#PushDate').datebox('getValue'),
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
                                $("#push_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#push_btn_cancel').click(function () {
        $('#push_div').dialog('close');
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
            url: base_url + "TemporaryDeliveryOrder/Delete",
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
        if ($('#DeliveryDate').datebox('getValue') == "") {
            return $($('#DeliveryDate').addClass('errormessage').before('<span class="errormessage">**' + "Adjustment Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'TemporaryDeliveryOrder/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'TemporaryDeliveryOrder/Insert';
        }

        var ordertype = document.getElementById("OrderType").selectedIndex;

        var virtualorderid;
        var deliveryorderid;

        if (ordertype == 0 || ordertype == 1) {
            virtualorderid = $("#PreviousOrderId").val();
            deliveryorderid = null;
        }
        else {
            virtualorderid = null;
            deliveryorderid = $("#PreviousOrderId").val();
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, WarehouseId: $("#WarehouseId").val(),
                DeliveryDate: $('#DeliveryDate').datebox('getValue'), OrderType: ordertype,
                VirtualOrderId: virtualorderid, DeliveryOrderId: deliveryorderid,
                NomorSurat: $('#NomorSurat').val()
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
        colNames: ['Id', 'Code', 'Detail Id', 'Detail Code', 'Item Id', 'Item Sku', 'Name', 'QTY', 'Restock', 'Waste', 'Price'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false },
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'orderdetailid', index: 'orderdetailid', width: 100, sortable: false, hidden: true },
                  { name: 'orderdetailcode', index: 'orderdetailcode', width: 70, sortable: false },
                  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 40, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'restockquantity', index: 'restockquantity', width: 50, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'wastequantity', index: 'wastequantity', width: 40, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'price', index: 'price', width: 100, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
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

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#Price').removeAttr('disabled');
        $('#Quantity').removeAttr('disabled');
        $('#btnItem').removeAttr('disabled');
        $('#row-restock').hide();
        $('#row-waste').hide();
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "TemporaryDeliveryOrder/GetInfoDetail?Id=" + id,
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
                            $('#ItemId').val(result.ItemId).data('process', '');
                            $('#Item').val(result.Item);
                            $('#Quantity').numberbox('setValue',result.Quantity);
                            $('#WasteQuantity').val(result.WasteQuantity);
                            $('#RestockQuantity').val(result.RestockQuantity);
                            $('#Price').numberbox('setValue',result.Price);
                            $('#PreviousOrderDetailId').val(result.OrderDetailId);
                            $('#Price').removeAttr('disabled');
                            $('#Quantity').removeAttr('disabled');
                            $('#btnItem').removeAttr('disabled');
                            $('#row-waste').hide();
                            $('#row-restock').hide();
                            $('#item_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_process_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "TemporaryDeliveryOrder/GetInfoDetail?Id=" + id,
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
                            $('#ItemId').val(result.ItemId).data('process', '1');
                            $('#Item').val(result.Item);
                            $('#Quantity').numberbox('setValue',result.Quantity);
                            $('#WasteQuantity').val(result.WasteQuantity);
                            $('#RestockQuantity').val(result.RestockQuantity);
                            $('#Price').numberbox('setValue',result.Price);
                            $('#PreviousOrderDetailId').val(result.OrderDetailId);
                            $('#Price').attr('disabled', true);
                            $('#Quantity').attr('disabled', true);
                            $('#btnItem').attr('disabled', true);
                            $('#row-waste').show();
                            $('#row-restock').show();
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
                        url: base_url + "TemporaryDeliveryOrder/DeleteDetail",
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
    // item_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');
        var process = $('#ItemId').data('process');
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            if (process == '1') {
                submitURL = base_url + 'TemporaryDeliveryOrder/ProcessDetail';
            }
            else {
                submitURL = base_url + 'TemporaryDeliveryOrder/UpdateDetail';
            }
        }
        else {
            submitURL = base_url + 'TemporaryDeliveryOrder/InsertDetail';
        }
        var ordertype = document.getElementById("OrderType").selectedIndex;

        var virtualorderdetailid;
        var salesorderdetailid;

        if (ordertype == 0 || ordertype == 1) {
            virtualorderdetailid = $("#PreviousOrderDetailId").val();
            salesorderdetailid = null;
        }
        else {
            virtualorderdetailid = null;
            salesorderdetailid = $("#PreviousOrderDetailId").val();
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, TemporaryDeliveryOrderId: $("#id").val(), ItemId: $("#ItemId").val(),
                Quantity: $("#Quantity").numberbox('getValue'), SalesOrderDetailId: salesorderdetailid, VirtualOrderDetailId: virtualorderdetailid,
                WasteQuantity: $("#WasteQuantity").numberbox('getValue'), RestockQuantity: $("#RestockQuantity").numberbox('getValue')
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


    // item_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up previousorder-------------------------------------------------------
    $('#btnPreviousOrder').click(function () {
        var lookUpURL;
        if (document.getElementById("OrderType").selectedIndex == 0) {
            lookUpURL = base_url + 'VirtualOrder/GetTrialListConfirmedNotCompleted';
        }
        else if (document.getElementById("OrderType").selectedIndex == 1) {
            lookUpURL = base_url + 'VirtualOrder/GetSampleListConfirmedNotCompleted';
        }
        else if (document.getElementById("OrderType").selectedIndex == 2) {
            lookUpURL = base_url + 'DeliveryOrder/GetListNotConfirmed';
        }
        var lookupGrid = $('#lookup_table_previousorder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_previousorder').dialog('open');
    });

    jQuery("#lookup_table_previousorder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Code', 'Nomor Surat', 'Contact Id', 'Contact', 'Order Date',
                  'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At', 'Warehouse Id', 'Warehouse'],
        colModel: [
    			  { name: 'id', index: 'id', width: 40, align: "center" },
                  { name: 'code', index: 'code', width: 60 },
                  { name: 'nomorsurat', index: 'nomorsurat', width: 120 },
				  { name: 'contactid', index: 'contactid', width: 100, hidden: true },
                  { name: 'contact', index: 'contact', width: 130 },
                  { name: 'orderdate', index: 'orderdate', width: 90, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden: true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'warehouseid', index: 'warehouseid', width: 100, hidden: true },
                  { name: 'warehouse', index: 'warehouse', width: 130, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_previousorder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_previousorder").width() - 10,
        height: $("#lookup_div_previousorder").height() - 110,
    });
    $("#lookup_table_previousorder").jqGrid('navGrid', '#lookup_toolbar_previousorder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_previousorder').click(function () {
        $('#lookup_div_previousorder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_previousorder').click(function () {
        var id = jQuery("#lookup_table_previousorder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_previousorder").jqGrid('getRowData', id);

            $('#PreviousOrderId').val(ret.id).data("kode", id);
            $('#PreviousOrder').val(ret.code);
            $('#lookup_div_previousorder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up item-------------------------------------------------------
    $('#btnItem').click(function () {
        var lookUpURL;
        if (document.getElementById("OrderType").selectedIndex == 0 || document.getElementById("OrderType").selectedIndex == 1) {
            lookUpURL = base_url + 'VirtualOrder/GetListDetail?Id=' + $("#PreviousOrderId").val();
        }
        else {
            lookUpURL = base_url + 'SalesOrder/GetListDetail?Id=' + $("#PreviousOrderId").val();
        }
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
        colNames: ['Id', 'Code', 'Item Id', 'Item Sku', 'Name', 'QTY', 'PendDlv', 'Price',
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false, align: 'center' },
                  { name: 'code', index: 'code', width: 70, sortable: false, align: 'center' },
				  { name: 'itemid', index: 'itemid', width: 100, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 70, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 50, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false, hidden: true },
                  { name: 'pendingquantity', index: 'pendingquantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'price', index: 'price', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
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
        gridComplete:
          function () {
              var ids = $(this).jqGrid('getDataIDs');
              for (var i = 0; i < ids.length; i++) {
                  var cl = ids[i];
                  rowType = $(this).getRowData(cl).type;
                  if (rowType == 'true') {
                      rowType = "Service";
                  } else {
                      rowType = "Trading";
                  }
                  $(this).jqGrid('setRowData', ids[i], { type: rowType });
              }
          }

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
            $('#PreviousOrderDetailId').val(id);
            $('#ItemId').val(ret.itemid);
            $('#Item').val(ret.itemname);
            $('#Price').numberbox('setValue',ret.price);
            $('#lookup_div_item').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup item----------------------------------------------------------------

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

}); //END DOCUMENT READY