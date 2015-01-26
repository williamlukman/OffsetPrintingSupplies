$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'BlanketWorkProcess/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
    $("#rejected_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
    $("#BlanketId").hide();
    $("#BlanketLeftBarId").hide();
    $("#BlanketRightBarId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'BlanketWorkProcess/GetList',
        datatype: "json",
        colNames: ['ID','Blanket Order ID', 'Code', 'Production No', 'Blanket Id', 'Sku','Blanket Name','Sku','Roll Name', 'Sku', 'Bar 1 Name',
                   'Sku', 'Bar 2 Name','C', 'SS', 'BP',
                   'ATA','Roll Blanket QTY', 'BM', 'BHP', 'BPOT',
                   'QC&M', 'P', 'Rej', 'Rejected Date', 'Fin' ,'Finished Date'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 50, align: "center" },
    			  { name: 'blanketorderid', index: 'blanketorderid', width: 80, align: 'center', hidden: true },
                  { name: 'blanketordercode', index: 'blanketordercode', width: 50, align: 'center', hidden: true },
                  { name: 'blanketorderproductionno', index: 'blanketorderproductionno', width: 80, align: 'center' },
                  { name: 'blanketid', index: 'blanketid', width: 100, sortable: false, hidden: true },
                  { name: 'sku', index: 'sku', width: 50, sortable: false, align: 'right' },
                  { name: 'blanketname', index: 'blanketname', width: 400, sortable: false },
                  { name: 'rollBlanketsku', index: 'rollBlanketsku', width: 50, align: 'right', sortable: false },
                  { name: 'rollBlanketname', index: 'rollBlanketname', width: 200, sortable: false },
                  { name: 'leftbarsku', index: 'leftbarsku', width: 50, align: 'right', sortable: false },
                  { name: 'lefbarname', index: 'lefbarname', width: 200, sortable: false },
                  { name: 'rightbarsku', index: 'rightbarsku', width: 50, align: 'right', sortable: false },
                  { name: 'rightbarname', index: 'rightbarname', width: 200, sortable: false },
                  { name: 'iscut', index: 'iscut', width: 30, sortable: false },
                  { name: 'issidesealed', index: 'issidesealed', width: 30, sortable: false },
                  { name: 'isbarprepared', index: 'isbarprepared', width: 30, sortable: false },
                  { name: 'isadhesivetapeapplied', index: 'isadhesivetapeapplied', width: 30, sortable: false },
                  { name: 'rollblanketusage', index: 'rollblanketusage', align: 'right', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'isbarmounted', index: 'isbarmounted', width: 30, sortable: false },
                  { name: 'isbarheatpressed', index: 'isbarheatpressed', width: 30, sortable: false },
                  { name: 'isbarpullofftested', index: 'isbarpullofftested', width: 30, sortable: false },
                  { name: 'isqcandmarked', index: 'isqcandmarked', width: 35, sortable: false },
                  { name: 'ispackaged', index: 'ispackaged', width: 30, sortable: false },
                  { name: 'isrejected', index: 'isrejected', width: 30, sortable: false },
                  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isfinished', index: 'isfinished', width: 30, sortable: false },
                  { name: 'finisheddate', index: 'finisheddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		          rowCut = $(this).getRowData(cl).iscut;
		          if (rowCut == 'true') {
		              rowCut = "Y";
		          } else {
		              rowCut = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iscut: rowCut });

		          rowSideSealed = $(this).getRowData(cl).issidesealed;
		          if (rowSideSealed == 'true') {
		              rowSideSealed = "Y";
		          } else {
		              rowSideSealed = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { issidesealed: rowSideSealed });

		          rowBarPrepared = $(this).getRowData(cl).isbarprepared;
		          if (rowBarPrepared == 'true') {
		              rowBarPrepared = "Y";
		          } else {
		              rowBarPrepared = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbarprepared: rowBarPrepared });

		          rowAdhesiveTapeApplied = $(this).getRowData(cl).isadhesivetapeapplied;
		          if (rowAdhesiveTapeApplied == 'true') {
		              rowAdhesiveTapeApplied = "Y";
		          } else {
		              rowAdhesiveTapeApplied = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isadhesivetapeapplied: rowAdhesiveTapeApplied });

		          rowBarMounted = $(this).getRowData(cl).isbarmounted;
		          if (rowBarMounted == 'true') {
		              rowBarMounted = "Y";
		          } else {
		              rowBarMounted = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbarmounted: rowBarMounted });

		          rowBarHeatPressed = $(this).getRowData(cl).isbarheatpressed;
		          if (rowBarHeatPressed == 'true') {
		              rowBarHeatPressed = "Y";
		          } else {
		              rowBarHeatPressed = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbarheatpressed: rowBarHeatPressed });
                 
		          rowBarPullOffTested = $(this).getRowData(cl).isbarpullofftested;
		          if (rowBarPullOffTested == 'true') {
		              rowBarPullOffTested = "Y";
		          } else {
		              rowBarPullOffTested = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbarpullofftested: rowBarPullOffTested });

		          rowQCAndMarked = $(this).getRowData(cl).isqcandmarked;
		          if (rowQCAndMarked == 'true') {
		              rowQCAndMarked = "Y";
		          } else {
		              rowQCAndMarked = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isqcandmarked: rowQCAndMarked });

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
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_process').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "BlanketWorkProcess/GetInfo?Id=" + id,
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
                            $('#BlanketOrderId').val(result.BlanketOrderId);
                            $('#BlanketSku').val(result.BlanketSku);
                            $('#BlanketName').val(result.Blanket);
                            $('#RollBlanketSku').val(result.RollBlanketSku);
                            $('#RollBlanket').val(result.RollBlanket);
                            $('#LeftBarSku').val(result.LeftBarSku);
                            $('#LeftBar').val(result.LeftBar);
                            $('#RightBarSku').val(result.RightBarSku);
                            $('#RightBar').val(result.RightBar);
                            $('#ViewRollBlanketUsage').val(result.RollBlanketUsage);
                            document.getElementById("iscut").checked = result.IsCut;
                            document.getElementById("issidesealed").checked = result.IsSideSealed;
                            document.getElementById("isbarprepared").checked = result.IsBarPrepared;
                            document.getElementById("isadhesivetapeapplied").checked = result.IsAdhesiveTapeApplied;
                            document.getElementById("isbarmounted").checked = result.IsBarMounted;
                            document.getElementById("isbarheatpressed").checked = result.IsBarHeatPressed;
                            document.getElementById("isbarpullofftested").checked = result.IsBarPullOffTested;
                            document.getElementById("isqcandmarked").checked = result.IsQCAndMarked;
                            document.getElementById("ispackaged").checked = result.IsPackaged;
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
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
                        url: base_url + "BlanketWorkProcess/Unfinish",
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
            url: base_url + "BlanketWorkProcess/Finish",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idfinished').val(), FinishedDate: $('#FinishedDate').datebox('getValue'), RollBlanketUsage: $('#RollBlanketUsageFinish').numberbox('getValue')
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
                        url: base_url + "BlanketWorkProcess/Unreject",
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
            url: base_url + "BlanketWorkProcess/Reject",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idrejected').val(), RejectedDate: $('#rejectedDate').datebox('getValue'), RollBlanketUsage: $('#RollBlanketUsageReject').numberbox('getValue')
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

 
    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // coreidentification_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'BlanketWorkProcess/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'BlanketWorkProcess/InsertDetail';
        }
        var e = document.getElementById("IsBarRequired");
        var isbar = e.options[e.selectedIndex].value;
        var f = document.getElementById("HasLeftBar");
        var hasleft = f.options[f.selectedIndex].value;
        var g = document.getElementById("HasRightBar");
        var hasright = g.options[g.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, BlanketId: $("#BlanketId").val(), IsBarRequired: isbar, HasLeftBar: hasleft,
                HasRightBar: hasright, BlanketOrderId: $("#id").val()
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


}); //END DOCUMENT READY