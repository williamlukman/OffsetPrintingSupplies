$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
                    'Ready', 'PendReceival', 'PendDelivery', 'MIN', 'Virtual',
                    'UoM', 'Selling Price', 'AvgPrice',
                    'Description', 'Item Type', 'Sub Type', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 240 },
                  { name: 'quantity', index: 'quantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'minimumquantity', index: 'minimumquantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'virtual', index: 'virtual', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uom', index: 'uom', width: 40 },
                  { name: 'sellingprice', index: 'sellingprice', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'avgprice', index: 'avgprice', width: 80, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'description', index: 'description', width: 70 },
                  { name: 'itemtype', index: 'itemtype', width: 70 },
                  { name: 'subtype', index: 'subtype', width: 70 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
                var ids = $(this).jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    rowQuantity = $(this).getRowData(cl).quantity;
                    rowMinimumQuantity = $(this).getRowData(cl).minimumquantity;
                    if (rowMinimumQuantity - rowQuantity >= 0) {
                        $(this).jqGrid('setRowData', ids[i], false, 'fontred');
                    }
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
                            $('#SKU').val(result.Sku);
                            $('#Description').val(result.Description);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemTypeName').val(result.ItemType);
                            $('#UoMId').val(result.UoMId);
                            $('#UoMName').val(result.UoM);
                            $('#MinimumQuantity').numberbox('setValue', (result.MinimumQuantity));
                            $('#SellingPrice').numberbox('setValue', (result.SellingPrice));
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
        vStatusSaving = 0;
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = base_url + 'MstItem/UpdatePrice';
        var id = $("#form_btn_save").data('kode');

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, SellingPrice: $("#SellingPrice").numberbox('getValue'),
                MinimumQuantity: $("#MinimumQuantity").numberbox('getValue')
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

    // -------------------------------------------------------Look Up PriceMutation -------------------------------------------------------
    $('#btnPriceMutation').click(function () {

        var lookUpURL = base_url + 'MstPriceMutation/GetList';
        var lookupGrid = $('#lookup_table_pricemutation');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_pricemutation').dialog('open');
    });

    $("#lookup_table_pricemutation").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'address', index: 'address', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_pricemutation'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_pricemutation").width() - 10,
        height: $("#lookup_div_pricemutation").height() - 110,
    });
    $("#lookup_table_pricemutation").jqGrid('navGrid', '#lookup_toolbar_pricemutation', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_pricemutation').click(function () {
        $('#lookup_div_pricemutation').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_pricemutation').click(function () {
        var id = jQuery("#lookup_table_pricemutation").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_pricemutation").jqGrid('getRowData', id);

            $('#PriceMutationId').val(ret.id).data("kode", id);
            $('#PriceMutationName').val(ret.name);

            $('#lookup_div_pricemutation').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    
    // ---------------------------------------------End Lookup PriceMutation----------------------------------------------------------------

}); //END DOCUMENT READY