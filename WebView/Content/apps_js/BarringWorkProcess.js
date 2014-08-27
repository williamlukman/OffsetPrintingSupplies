$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'BarringWorkProcess/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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

    $("#finished_div").dialog('close');
    $("#rejected_div").dialog('close');
    $("#form_div").dialog('close');
    $("#item_div").dialog('close');
  

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'BarringWorkProcess/GetList',
        datatype: "json",
        colNames: ['ID','Barring Order ID', 'Barring Id', 'Sku','Barring','Blanket Sku','BlanketName', 'LeftBar Sku', 'LeftBarName',
                     'RightBar Sku', 'RightBarName','IsCut','IsSideSealed',  'IsBarPrepared',
                    'IsAdhesiveTapeApplied','IsBarMounted',    'IsBarHeatPressed',    'IsBarPullOffTested',
                    'IsQCAndMarked',   'IsPackaged',  'IsRejected',     'Rejected Date', 'Is Finished' ,'Finished Date'
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: "center" },
    			  { name: 'barringorderid', index: 'barringorderid', width: 80, align: "center" },
                  { name: 'barringid', index: 'barringid', width: 100, sortable: false },
                  { name: 'sku', index: 'sku', width: 100, sortable: false },
                  { name: 'barringname', index: 'barringname', width: 100, sortable: false },
                  { name: 'blanketsku', index: 'blanketsku', width: 100, sortable: false },
                  { name: 'blanketname', index: 'blanketname', width: 100, sortable: false },
                  { name: 'leftbarsku', index: 'leftbarsku', width: 100, sortable: false },
                  { name: 'lefbarname', index: 'lefbarname', width: 100, sortable: false },
                  { name: 'rightbarsku', index: 'rightbarsku', width: 100, sortable: false },
                  { name: 'rightbarname', index: 'rightbarname', width: 100, sortable: false },
                  { name: 'iscut', index: 'iscut', width: 100, sortable: false },
                  { name: 'issidesealed', index: 'issidesealed', width: 100, sortable: false },
                  { name: 'isbarprepared', index: 'isbarprepared', width: 100, sortable: false },
                  { name: 'isadhesivetapeapplied', index: 'isadhesivetapeapplied', width: 100, sortable: false },
                  { name: 'isbarmounted', index: 'isbarmounted', width: 100, sortable: false },
                  { name: 'isbarheatpressed', index: 'isbarheatpressed', width: 100, sortable: false },
                  { name: 'isbarpullofftested', index: 'isbarpullofftested', width: 100, sortable: false },
                  { name: 'isqcandmarked', index: 'isqcandmarked', width: 100, sortable: false },
                  { name: 'ispackaged', index: 'ispackaged', width: 100, sortable: false },
                  { name: 'isrejected', index: 'isrejected', width: 100, sortable: false },
                  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isfinished', index: 'isfinished', width: 100, sortable: false },
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



  



    $('#btn_process').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "BarringWorkProcess/GetInfo?Id=" + id,
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
                            $('#BarringOrderId').val(result.BarringOrderId);
                            $('#BarringSku').val(result.BarringSku);
                            $('#Barring').val(result.Barring);
                            $('#BlanketSku').val(result.BlanketSku);
                            $('#Blanket').val(result.Blanket);
                            $('#LeftBarSku').val(result.LeftBarSku);
                            $('#LeftBar').val(result.LeftBar);
                            $('#RightBarSku').val(result.RightBarSku);
                            $('#RightBar').val(result.RightBar);
                            document.getElementById("iscut").checked = result.IsCut;
                            document.getElementById("issidesealed").checked = result.IsSideSealed;
                            document.getElementById("isbarprepared").checked = result.IsBarPrepared;
                            document.getElementById("isadhesivetapeapplied").checked = result.IsAdhesiveTapeApplied;
                            document.getElementById("isbarmounted").checked = result.IsBarMounted;
                            document.getElementById("isbarheatpressed").checked = result.IsBarHeatPressed;
                            document.getElementById("isbarpullofftested").checked = result.IsBarPullOffTested;
                            document.getElementById("isqcandmarked").checked = result.IsQCAndMarked;
                            document.getElementById("ispackaged").checked = result.IsPackaged;
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

 

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#id").val();
        submitURL = base_url + 'BarringWorkProcess/ProgressDetail';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, 
                IsCut: document.getElementById("iscut").checked, IsSideSealed: document.getElementById("issidesealed").checked,
                IsBarPrepared: document.getElementById("isbarprepared").checked, IsAdhesiveTapeApplied: document.getElementById("isadhesivetapeapplied").checked,
                IsBarMounted: document.getElementById("isbarmounted").checked, IsBarHeatPressed: document.getElementById("isbarheatpressed").checked,
                IsBarPullOffTested: document.getElementById("isbarpullofftested").checked, IsQCAndMarked: document.getElementById("isqcandmarked").checked,
                IsPackaged: document.getElementById("ispackaged").checked,
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
                        url: base_url + "BarringWorkProcess/Unfinish",
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
        $.ajax({
            url: base_url + "BarringWorkProcess/Finish",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idfinished').val(), FinishedDate: $('#FinishedDate').datebox('getValue'),
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
                        url: base_url + "BarringWorkProcess/Unreject",
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
            url: base_url + "BarringWorkProcess/Reject",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idrejected').val(), RejectedDate: $('#FinishedDate').datebox('getValue'),
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
            submitURL = base_url + 'BarringWorkProcess/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'BarringWorkProcess/InsertDetail';
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
                Id: id, BarringId: $("#BarringId").val(), IsBarRequired: isbar, HasLeftBar: hasleft,
                HasRightBar: hasright, BarringOrderId: $("#id").val()
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