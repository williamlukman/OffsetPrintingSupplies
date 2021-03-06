﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'RecoveryWorkProcess/GetList', /*postData: { filters: null }, page: 'first'*/ }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'RecoveryWorkProcess/GetListAccessory?Id=' + $("#id").val(), /*postData: { filters: null }, page: 'first'*/ }).trigger("reloadGrid");
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

    $("#finished_div").dialog('close');
    $("#accessory_div").dialog('close'); 
    $("#rejected_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_compoundunderlayer").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#CompoundUnderLayerId").hide();
    $("#ItemId").hide();
    $("#ItemSku").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'RecoveryWorkProcess/GetList',
        datatype: "json",
        colNames: ['RO Id', 'RCN No', 'CoreIdentificationDetailId', 'RIFD Id', 'Diss No', 'Material',
                    'RollerBuilder Id', 'Roller Sku', 'Roller Name', 'Type', 'Core', 'Compound', 'Compound QTY',
                    'Under Layer', 'Under Layer QTY',
                   'D', 'S&G', 'W',
                   'V', 'FO', 'CG', 'CNCG',
                   'P&QC','P',
                   'Rejected Date', 'Finished Date'
        ],
        colModel: [
                  { name: 'recoveryorderid', index: 'recoveryorderid', width: 40 },
                  { name: 'recoveryordercode', index: 'recoveryordercode', width: 80 },
                  { name: 'coreidentificationdetailid', index: 'coreidentificationdetailid', width: 100, hidden: true },
                  { name: 'rifdid', index: 'rollerbuilderid', width: 40 },
                  { name: 'NomorDisassembly', index: 'NomorDisassembly', width: 80 },
                  { name: 'materialcase', index: 'materialcase', width: 50 },
                  { name: 'rollerbuilderid', index: 'rollerbuilderid', width: 40, hidden: true },
                  { name: 'rollerbuilderbasesku', index: 'rollerbuilderbasesku', width: 60 },
                  { name: 'rollerbuilder', index: 'rollerbuilder', width: 450 },
				  { name: 'coretypecase', index: 'coretypecase', width: 40 },
				  { name: 'corebuilder', index: 'corebuilder', width: 200 },
				  { name: 'compound', index: 'compound', width: 200 },
                  { name: 'compoundusage', index: 'compoundusage', width: 90, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'compoundunderlayer', index: 'compoundunderlayer', width: 200 },
				  { name: 'compoundunderlayerusage', index: 'compoundunderlayerusage', width: 90, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'isdisassembled', index: 'isdisassembled', width: 30 },
                  { name: 'isstrippedandglued', index: 'isstrippedandglued', width: 30 },
                  { name: 'iswrapped', index: 'iswrapped', width: 30 },
                  { name: 'isvulcanized', index: 'isvulcanized', width: 30 },
                  { name: 'isfacedoff', index: 'isfacedoff', width: 30 },
                  { name: 'isconventionalgrinded', index: 'isconventionalgrinded', width: 30 },
                  { name: 'iscncgrinded', index: 'iscncgrinded', width: 35 },
                  { name: 'ispolishedandqc', index: 'ispolishedandqc', width: 35 },
                  { name: 'ispackaged', index: 'ispackaged', width: 30 },
                  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 90, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'finisheddate', index: 'finisheddate', sortable: false, search: false, width: 90, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

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
		          rowDisassembled = $(this).getRowData(cl).isdisassembled;
		          if (rowDisassembled == 'true') {
		              rowDisassembled = "Y";
		          } else {
		              rowDisassembled = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isdisassembled: rowDisassembled });

		          rowStrippedAndGlued = $(this).getRowData(cl).isstrippedandglued;
		          if (rowStrippedAndGlued == 'true') {
		              rowStrippedAndGlued = "Y";
		          } else {
		              rowStrippedAndGlued = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isstrippedandglued: rowStrippedAndGlued });

		          rowWrapped = $(this).getRowData(cl).iswrapped;
		          if (rowWrapped == 'true') {
		              rowWrapped = "Y";
		          } else {
		              rowWrapped = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iswrapped: rowWrapped });

		          rowVulcanized = $(this).getRowData(cl).isvulcanized;
		          if (rowVulcanized == 'true') {
		              rowVulcanized = "Y";
		          } else {
		              rowVulcanized = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isvulcanized: rowVulcanized });

		          rowFacedOff = $(this).getRowData(cl).isfacedoff;
		          if (rowFacedOff == 'true') {
		              rowFacedOff = "Y";
		          } else {
		              rowFacedOff = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isfacedoff: rowFacedOff });

		          rowConventionalGrinded = $(this).getRowData(cl).isconventionalgrinded;
		          if (rowConventionalGrinded == 'true') {
		              rowConventionalGrinded = "Y";
		          } else {
		              rowConventionalGrinded = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isconventionalgrinded: rowConventionalGrinded });

		          rowCNCGrinded = $(this).getRowData(cl).iscncgrinded;
		          if (rowCNCGrinded == 'true') {
		              rowCNCGrinded = "Y";
		          } else {
		              rowCNCGrinded = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iscncgrinded: rowCNCGrinded });

		          rowPolishedAndQC = $(this).getRowData(cl).ispolishedandqc;
		          if (rowPolishedAndQC == 'true') {
		              rowPolishedAndQC = "Y";
		          } else {
		              rowPolishedAndQC = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { ispolishedandqc: rowPolishedAndQC });

		          rowPackaged = $(this).getRowData(cl).ispackaged;
		          if (rowPackaged == 'true') {
		              rowPackaged = "Y";
		          } else {
		              rowPackaged = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { ispackaged: rowPackaged });

		          rowRejected = $(this).getRowData(cl).isrejected;
		          if (rowRejected == 'true') {
		              rowRejected = "Y";
		          } else {
		              rowRejected = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isrejected: rowRejected });

		          rowFinished = $(this).getRowData(cl).isfinished;
		          if (rowFinished == 'true') {
		              rowFinished = "Y";
		          } else {
		              rowFinished = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isfinished: rowFinished });
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
                url: base_url + "RecoveryWorkProcess/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    //else if (result.ConfirmationDate == null) {
                    //    $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    //}
                    else {
                        window.open(base_url + "Report/PrintoutRecoveringWorkChart?Id=" + id);
                    }
                }
            });
        }
    });



    $('#btn_process').click(function () {
        ClearData();
        clearForm('#frm');
        $('#form_btn_save').show();
        $('#process_div').show();
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "RecoveryWorkProcess/GetInfo?Id=" + id,
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
                            $('#RecoveryOrderId').val(result.RecoveryOrderId);
                            $('#RecoveryOrderCode').val(result.RecoveryOrderCode);
                            $('#RollerBuilderBaseSku').val(result.RollerBuilderBaseSku);
                            $('#RollerBuilder').val(result.RollerBuilder);
                            $('#CoreTypeCase').val(result.CoreTypeCase);
                            $('#NomorDisassembly').val(result.NomorDisassembly);
                            $('#Compound').val(result.Compound);
                            $('#CompoundUsage').numberbox('setValue', result.CompoundUsage);
                            $('#CompoundUnderLayerId').val(result.CompoundUnderLayerId);
                            $('#CompoundUnderLayer').val(result.CompoundUnderLayer);
                            $('#CompoundUnderLayerUsage').numberbox('setValue', result.CompoundUnderLayerUsage);
                            document.getElementById("IsDisassembled").checked = result.IsDisassembled;
                            document.getElementById("IsStrippedAndGlued").checked = result.IsStrippedAndGlued;
                            document.getElementById("IsWrapped").checked = result.IsWrapped;
                            document.getElementById("IsVulcanized").checked = result.IsVulcanized;
                            document.getElementById("IsFacedOff").checked = result.IsFacedOff;
                            document.getElementById("IsConventionalGrinded").checked = result.IsConventionalGrinded;
                            document.getElementById("IsCNCGrinded").checked = result.IsCNCGrinded;
                            document.getElementById("IsPolishedAndQC").checked = result.IsPolishedAndQC;
                            document.getElementById("IsPackaged").checked = result.IsPackaged;
                            if (result.IsDisassembled) { $('#IsDisassembled').attr('disabled', true); } else { $('#IsDisassembled').removeAttr('disabled'); }
                            if (result.IsStrippedAndGlued) { $('#IsStrippedAndGlued').attr('disabled', true); } else { $('#IsStrippedAndGlued').removeAttr('disabled'); }
                            if (result.IsWrapped) { $('#IsWrapped').attr('disabled', true); } else { $('#IsWrapped').removeAttr('disabled'); }
                            if (result.IsVulcanized) { $('#IsVulcanized').attr('disabled', true); } else { $('#IsVulcanized').removeAttr('disabled'); }
                            if (result.IsFacedOff) { $('#IsFacedOff').attr('disabled', true); } else { $('#IsFacedOff').removeAttr('disabled'); }
                            if (result.IsConventionalGrinded) { $('#IsConventionalGrinded').attr('disabled', true); } else { $('#IsConventionalGrinded').removeAttr('disabled'); }
                            if (result.IsCNCGrinded) { $('#IsCNCGrinded').attr('disabled', true); } else { $('#IsCNCGrinded').removeAttr('disabled'); }
                            if (result.IsPolishedAndQC) { $('#IsPolishedAndQC').attr('disabled', true); } else { $('#IsPolishedAndQC').removeAttr('disabled'); }
                            if (result.IsPackaged) { $('#IsPackaged').attr('disabled', true); } else { $('#IsPackaged').removeAttr('disabled'); }
                            $('#process_div').show();
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


    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "RecoveryWorkProcess/GetInfo?Id=" + id,
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
                            $('#tableaccessory_div').hide();
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
                        url: base_url + "RecoveryWorkProcess/Unconfirm",
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
            url: base_url + "RecoveryWorkProcess/Confirm",
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
            url: base_url + "RecoveryWorkProcess/Delete",
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
        //var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        // Update
      
            submitURL = base_url + 'RecoveryWorkProcess/ProgressDetail';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, CompoundUsage: $("#CompoundUsage").numberbox('getValue'),
                CompoundUnderLayerId: $("#CompoundUnderLayerId").val(),
                CompoundUnderLayerUsage: $("#CompoundUnderLayerUsage").numberbox('getValue'),
                IsDisassembled: document.getElementById("IsDisassembled").checked,
                IsStrippedAndGlued: document.getElementById("IsStrippedAndGlued").checked,
                IsWrapped: document.getElementById("IsWrapped").checked,
                IsVulcanized: document.getElementById("IsVulcanized").checked,
                IsFacedOff: document.getElementById("IsFacedOff").checked,
                IsConventionalGrinded: document.getElementById("IsConventionalGrinded").checked,
                IsCNCGrinded: document.getElementById("IsCNCGrinded").checked,
                IsPolishedAndQC: document.getElementById("IsPolishedAndQC").checked,
                IsPackaged: document.getElementById("IsPackaged").checked,
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
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    //GRID Accessory+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Item ID', 'Sku', 'Name', 'QTY', 'UoM'],
        colModel: [
    			  //{ name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'itemid', index: 'itemid', width: 60 },
                  { name: 'itemsku', index: 'itemsku', width: 80 },
				  { name: 'item', index: 'item', width: 200 },
                  { name: 'quantity', index: 'quantity', width: 80, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 60 },
        ],
        //page: '1',
        //pager: $('#pageraccessory'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'itemid',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		  }
    });//END GRID Acessory
    $("#listdetail").jqGrid('navGrid', '#pageraccessory', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#accessory_div');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "RecoveryWorkProcess/GetInfo?Id=" + id,
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
                            $('#RecoveryOrderId').val(result.RecoveryOrderId);
                            $('#RollerBuilderBaseSku').val(result.RollerBuilderBaseSku);
                            $('#RollerBuilder').val(result.RollerBuilder);
                            $('#CoreTypeCase').val(result.CoreTypeCase);
                            $('#Compound').val(result.Compound);
                            $('#CompoundUsage').numberbox('setValue', result.CompoundUsage);
                            $('#CompoundUnderLayerId').val(result.CompoundUnderLayerId);
                            $('#CompoundUnderLayer').val(result.CompoundUnderLayer);
                            $('#CompoundUnderLayerUsage').numberbox('setValue', result.CompoundUnderLayerUsage);
                            document.getElementById("IsDisassembled").checked = result.IsDisassembled;
                            document.getElementById("IsStrippedAndGlued").checked = result.IsStrippedAndGlued;
                            document.getElementById("IsWrapped").checked = result.IsWrapped;
                            document.getElementById("IsVulcanized").checked = result.IsVulcanized;
                            document.getElementById("IsFacedOff").checked = result.IsFacedOff;
                            document.getElementById("IsConventionalGrinded").checked = result.IsConventionalGrinded;
                            document.getElementById("IsCNCGrinded").checked = result.IsCNCGrinded;
                            document.getElementById("IsPolishedAndQC").checked = result.IsPolishedAndQC;
                            document.getElementById("IsPackaged").checked = result.IsPackaged;
                            if (result.IsDisassembled) { $('#IsDisassembled').attr('disabled', true); } else { $('#IsDisassembled').removeAttr('disabled'); }
                            if (result.IsStrippedAndGlued) { $('#IsStrippedAndGlued').attr('disabled', true); } else { $('#IsStrippedAndGlued').removeAttr('disabled'); }
                            if (result.IsWrapped) { $('#IsWrapped').attr('disabled', true); } else { $('#IsWrapped').removeAttr('disabled'); }
                            if (result.IsVulcanized) { $('#IsVulcanized').attr('disabled', true); } else { $('#IsVulcanized').removeAttr('disabled'); }
                            if (result.IsFacedOff) { $('#IsFacedOff').attr('disabled', true); } else { $('#IsFacedOff').removeAttr('disabled'); }
                            if (result.IsConventionalGrinded) { $('#IsConventionalGrinded').attr('disabled', true); } else { $('#IsConventionalGrinded').removeAttr('disabled'); }
                            if (result.IsCNCGrinded) { $('#IsCNCGrinded').attr('disabled', true); } else { $('#IsCNCGrinded').removeAttr('disabled'); }
                            if (result.IsPolishedAndQC) { $('#IsPolishedAndQC').attr('disabled', true); } else { $('#IsPolishedAndQC').removeAttr('disabled'); }
                            if (result.IsPackaged) { $('#IsPackaged').attr('disabled', true); } else { $('#IsPackaged').removeAttr('disabled'); }
                            $('#form_btn_save').hide();
                            $('#process_div').hide();
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

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#accessory_div');
        $("#accessory_btn_submit").data('kode', '');
        $('#accessory_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#accessory_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "RecoveryWorkProcess/GetInfoAccessory?Id=" + id, //$("#id").val(),
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
                            $("#accessory_btn_submit").data('kode', result.Id);
                            $('#ItemId').val(result.ItemId);
                            $('#Item').val(result.Item);
                            $('#Quantity').numberbox('setValue', (result.Quantity));
                            $('#accessory_div').dialog('open');
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
                        url: base_url + "RecoveryWorkProcess/DeleteAccessory",
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
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    //--------------------------------------------------------Dialog @Accessory-------------------------------------------------------------
    // accessory_btn_submit

    $("#accessory_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#accessory_btn_submit").data('kode');
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'RecoveryWorkProcess/UpdateAccessory';
        }
            // Insert
        else {
            submitURL = base_url + 'RecoveryWorkProcess/InsertAccessory';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ItemId: $("#ItemId").val(), Quantity: $("#Quantity").numberbox('getValue'), RecoveryOrderDetailId: jQuery("#list").jqGrid('getGridParam', 'selrow') //$("#id").val()
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
                    $("#accessory_div").dialog('close')
                }
            }
        });
    });


    // accessory_btn_cancel
    $('#accessory_btn_cancel').click(function () {
        clearForm('#accessory_div');
        $("#accessory_div").dialog('close');
    });

    $('#btn_finish').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#FinishedDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idfinished').val(id);
            $("#finished_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unfinish').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unfinish record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "RecoveryWorkProcess/Unfinish",
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
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#finished_btn_submit').click(function () {
        ClearErrorMessage();
        ClickableButton($("#finished_btn_submit"), false);
        $.ajax({
            url: base_url + "RecoveryWorkProcess/Finish",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idfinished').val(), FinishedDate: $('#FinishedDate').datebox('getValue'),
            }),
            success: function (result) {
                ClickableButton($("#finished_btn_submit"), true);
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
                    $("#finished_div").dialog('close');
                }
            }
        });
    });

    $('#finished_btn_cancel').click(function () {
        $('#finished_div').dialog('close');
    });

    $('#btn_reject').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#FinishedDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idrejected').val(id);
            $("#rejected_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unreject').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unreject record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "RecoveryWorkProcess/Unreject",
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
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#rejected_btn_submit').click(function () {
        ClearErrorMessage();
        $.ajax({
            url: base_url + "RecoveryWorkProcess/Reject",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idrejected').val(), RejectedDate: $('#RejectedDate').datebox('getValue'),
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
                    $("#rejected_div").dialog('close');
                }
            }
        });
    });

    $('#rejected_btn_cancel').click(function () {
        $('#rejected_div').dialog('close');
    });

    
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up item-------------------------------------------------------
    $('#btnItem').click(function () {
        var lookUpURL = base_url + 'MstItem/GetListAccessory';
        var lookupGrid = $('#lookup_table_item');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_btn_add_item').show();
        $('#lookup_div_item').dialog('open');
    });

    jQuery("#lookup_table_item").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Sku', 'Name', 'QTY', 'PendReceival', 'PendDelivery', 'Minimum', 'Virtual', 'UoM'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 240 },
                  { name: 'quantity', index: 'quantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'minimum', index: 'minimum', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'virtual', index: 'virtual', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uom', index: 'uom', width: 40 },
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
    $("#lookup_table_item").jqGrid('navGrid', '#lookup_toolbar_item', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_item').click(function () {
        $('#lookup_div_item').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_item').click(function () {
        var id = jQuery("#lookup_table_item").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_item").jqGrid('getRowData', id);

            $('#ItemId').val(ret.id).data("kode", id);
            $('#Item').val(ret.name);

            $('#lookup_div_item').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // ---------------------------------------------End Lookup item----------------------------------------------------------------
   
    // -------------------------------------------------------Look Up compoundunderlayer-------------------------------------------------------
    $('#btnCompoundUnderLayer').click(function () {
        var lookUpURL = base_url + 'MstRollerBuilder/GetListCompound';
        var lookupGrid = $('#lookup_table_compoundunderlayer');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_compoundunderlayer').dialog('open');
    });

    $('#btn_removeUnderLayer').click(function () {
        $('#CompoundUnderLayerId').val('');
        $('#CompoundUnderLayer').val('');
        $('#CompoundUnderLayerUsage').numberbox('setValue', 0);
    });

    jQuery("#lookup_table_compoundunderlayer").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 300 },
                  { name: 'description', index: 'description', width: 100, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 60 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_compoundunderlayer'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_compoundunderlayer").width() - 10,
        height: $("#lookup_div_compoundunderlayer").height() - 110,
    });
    $("#lookup_table_compoundunderlayer").jqGrid('navGrid', '#lookup_toolbar_compoundunderlayer', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_compoundunderlayer').click(function () {
        $('#lookup_div_compoundunderlayer').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_compoundunderlayer').click(function () {
        var id = jQuery("#lookup_table_compoundunderlayer").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_compoundunderlayer").jqGrid('getRowData', id);

            $('#CompoundUnderLayerId').val(ret.id).data("kode", id);
            $('#CompoundUnderLayer').val(ret.name);

            $('#lookup_div_compoundunderlayer').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // ---------------------------------------------End Lookup compoundunderlayer----------------------------------------------------------------

}); //END DOCUMENT READY