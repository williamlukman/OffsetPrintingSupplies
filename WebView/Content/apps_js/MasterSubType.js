﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstSubType/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#ItemTypeId').hide();
    $("#lookup_div_itemtype").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstSubType/GetList',
        datatype: "json",
        colNames: ['ID', 'Name', 'Item Type Id', 'Item Type', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 120 },
                  { name: 'itemtypeid', index: 'itemtypeid', width: 80, align: "center", hidden: true },
				  { name: 'itemtype', index: 'itemtype', width: 200 },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
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
		      //var ids = $(this).jqGrid('getDataIDs');
		      //for (var i = 0; i < ids.length; i++) {
		      //    var cl = ids[i];
		      //    rowDel = $(this).getRowData(cl).deletedimg;
		      //    if (rowDel == 'true') {
		      //        img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";

		      //    } else {
		      //        img = "";
		      //    }
		      //    $(this).jqGrid('setRowData', ids[i], { deletedimg: img });
		      //}
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

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "MstSubType/GetInfo?Id=" + id,
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
                            $('#Name').val(result.Name);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemType').val(result.ItemType);
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            //if (ret.deletedimg != '') {
            //    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
            //    return;
            //}
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
            url: base_url + "MstSubType/Delete",
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
            submitURL = base_url + 'MstSubType/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstSubType/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name :$("#Name").val(), ItemTypeId: $("#ItemTypeId").val()
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}')
                {
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

    // -------------------------------------------------------Look Up Itemtype-------------------------------------------------------
    $('#btnItemType').click(function () {

        var lookUpURL = base_url + 'MstItemType/GetList';
        var lookupGrid = $('#lookup_table_itemtype');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_itemtype').dialog('open');
    });

    $("#lookup_table_itemtype").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_itemtype'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_itemtype").width() - 10,
        height: $("#lookup_div_itemtype").height() - 110,
    });
    $("#lookup_table_itemtype").jqGrid('navGrid', '#lookup_toolbar_itemtype', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_itemtype').click(function () {
        $('#lookup_div_itemtype').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_itemtype').click(function () {
        var id = jQuery("#lookup_table_itemtype").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_itemtype").jqGrid('getRowData', id);

            $('#ItemTypeId').val(ret.id).data("kode", id);
            $('#ItemType').val(ret.name);
            $('#lookup_div_itemtype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup ItemType----------------------------------------------------------------

}); //END DOCUMENT READY