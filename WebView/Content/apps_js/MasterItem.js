$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode,
		currentpage = '1';
   
    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { }, page: currentpage }).trigger("reloadGrid");
    }
    
    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_itemtype").dialog('close');
    $("#lookup_div_uom").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#ItemTypeId").hide();
    $("#UoMId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstItem/GetList',
        datatype: "json",
        colNames: ['ID', 'Sku', 'Name', 
                    'Ready', 'PendReceival', 'PendDelivery', 'MIN', 'Virtual', "Cust's QTY",
                    'UoM', 'Selling Price', 'Currency Id', 'Currency', 'Price List', 'AvgPrice', "Cust's AvgPrice",
                    'Description', 'Item Type', 'Tradeable', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 480 },
                  { name: 'quantity', index: 'quantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'minimumquantity', index: 'minimumquantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'virtual', index: 'virtual', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'customerquantity', index: 'customerquantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 70 },
                  { name: 'sellingprice', index: 'sellingprice', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
    			  { name: 'currencyid', index: 'currencyid', width: 35, align: "center", hidden: true },
                  { name: 'currency', index: 'currency', width: 70 },
                  { name: 'pricelist', index: 'pricelist', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'avgprice', index: 'avgprice', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'customeravgprice', index: 'customeravgprice', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'description', index: 'description', width: 100 },
                  { name: 'itemtype', index: 'itemtype', width: 70 },
				  { name: 'istradeable', index: 'istradeable', width: 40, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':;true:Yes;false:No' } },
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
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
            function () {
                var ids = $(this).jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    rowQuantity = $(this).getRowData(cl).quantity;
                    rowMinimumQuantity = $(this).getRowData(cl).minimumquantity;
                    if (rowMinimumQuantity - rowQuantity >= 0) {
                        $(this).jqGrid('setRowData', ids[i], false, 'fontred');
                    }

                    rowIsTradeable = $(this).getRowData(cl).istradeable;
                    if (rowIsTradeable == 'true') {
                        rowIsTradeable = "Yes";
                    } else {
                        rowIsTradeable = "No";
                    }
                    $(this).jqGrid('setRowData', ids[i], { istradeable: rowIsTradeable });
                }
            }
    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        $('input[id*="gs_"]').val("");
        $('select[id*="gs_"]').val("ALL");
        currentpage = '1';
        $("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { "filters": "" }, page: currentpage }).trigger("reloadGrid");
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Report/ReportItem');
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
            $.ajax({
                dataType: "json",
                url: base_url + "MstItem/GetInfo?Id=" + id,
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
                            $('#Sku').val(result.Sku);
                            $('#Description').val(result.Description);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemTypeName').val(result.ItemType);
                            var f = document.getElementById("IsTradeable");
                            if (result.IsTradeable == true) {
                                f.selectedIndex = 0;
                            }
                            else {
                                f.selectedIndex = 1;
                            }
                            $('#UoMId').val(result.UoMId);
                            $('#UoMName').val(result.UoM);
                            $('#Quantity').numberbox('setValue', (result.Quantity));
                            $('#MinimumQuantity').numberbox('setValue', (result.MinimumQuantity));
                            $('#SellingPrice').numberbox('setValue', (result.SellingPrice));
                            $('#PriceList').numberbox('setValue', (result.PriceList));
                            $('#PendingDelivery').numberbox('setValue', (result.PendingDelivery));
                            $('#PendingReceival').numberbox('setValue', (result.PendingReceival));
                            $('#form_div').dialog('open');
                            $('#CurrencyId').val(result.CurrencyId);
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
            url: base_url + "MstItem/Delete",
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
                    currentpage = $('#list').getGridParam('page');
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
            currentpage = $('#list').getGridParam('page')
            if ($("#ItemTypeName").val() == 'Blanket' || $("#ItemTypeName").val() == 'Roller' || $("#ItemTypeName").val() == 'Core') {
                submitURL = base_url + 'MstItem/UpdatePrice';
            }
            else {
                submitURL = base_url + 'MstItem/Update';
            }
        }
            // Insert
        else {
            currentpage = '1';
            submitURL = base_url + 'MstItem/Insert';
        }

        var f = document.getElementById("IsTradeable");
        var tradeable = f.options[f.selectedIndex].value;

        var g = document.getElementById("CurrencyId");
        var currency = g.options[g.selectedIndex].value;
        
        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(), ItemTypeId: $("#ItemTypeId").val(), SellingPrice: $("#SellingPrice").numberbox('getValue'),
                PriceList: $("#PriceList").numberbox('getValue'), CurrencyId: currency,
                Sku: $("#Sku").val(), Description: $("#Description").val(), UoMId: $("#UoMId").val(),
                MinimumQuantity: $("#MinimumQuantity").numberbox('getValue'), IsTradeable: tradeable
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
            $('#ItemTypeName').val(ret.name);
            $('#lookup_div_itemtype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
        $.ajax({
            dataType: "json",
            url: base_url + "MstItemType/GetInfoByName?itemType=" + $('#ItemTypeName').val(),
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
                        $('#Sku').val(result.SKU);
                    }
                }
            }
        });

    });

    
    // ---------------------------------------------End Lookup ItemType----------------------------------------------------------------

    // -------------------------------------------------------Look Up uom-------------------------------------------------------
    $('#btnUoM').click(function () {
        var lookUpURL = base_url + 'MstUoM/GetList';
        var lookupGrid = $('#lookup_table_uom');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_uom').dialog('open');
    });

    jQuery("#lookup_table_uom").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'contactcode', width: 80, align: 'right' },
                  { name: 'name', index: 'contactname', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_uom'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_uom").width() - 10,
        height: $("#lookup_div_uom").height() - 110,
    });
    $("#lookup_table_uom").jqGrid('navGrid', '#lookup_toolbar_uom', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_uom').click(function () {
        $('#lookup_div_uom').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_uom').click(function () {
        var id = jQuery("#lookup_table_uom").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_uom").jqGrid('getRowData', id);

            $('#UoMId').val(ret.id).data("kode", id);
            $('#UoMName').val(ret.name);

            $('#lookup_div_uom').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup uom----------------------------------------------------------------

    // -------------------------------------------------------Look Up subtype-------------------------------------------------------
    $('#btnSubType').click(function () {
        var lookUpURL = base_url + 'MstSubType/GetList';
        var lookupGrid = $('#lookup_table_subtype');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_subtype').dialog('open');
    });

    jQuery("#lookup_table_subtype").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_subtype'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_subtype").width() - 10,
        height: $("#lookup_div_subtype").height() - 110,
    });
    $("#lookup_table_subtype").jqGrid('navGrid', '#lookup_toolbar_subtype', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_subtype').click(function () {
        $('#lookup_div_subtype').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_subtype').click(function () {
        var id = jQuery("#lookup_table_subtype").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_subtype").jqGrid('getRowData', id);

            $('#SubTypeId').val(ret.id).data("kode", id);
            $('#SubTypeName').val(ret.name);

            $('#lookup_div_subtype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup subtype----------------------------------------------------------------

}); //END DOCUMENT READY