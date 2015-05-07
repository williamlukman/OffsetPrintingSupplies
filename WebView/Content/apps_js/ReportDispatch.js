$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam().trigger("reloadGrid");
    }

    function ClearData() {
        //$('#Description').val('').text('').removeClass('errormessage');
        //$('#Name').val('').text('').removeClass('errormessage');
        //$('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    //$("#lookup_div_itemtype").dialog('close');


    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        //var datas = $("#list").jqGrid('getGridParam', 'data'); //jQuery("list").getGridParam('data'); // get all data from all pages
        //var ids = $('#list').jqGrid('getDataIDs'); // get all id in current page
        //var cols = $("#list").jqGrid('getCol', 'id'); // also get id from current/visible page
        //var idarray = [];
        //for (var i = 0; i < datas.length; i++) {
        //    idarray.push(datas[i].id);
        //}
        window.open(base_url + 'Report/ReportDispatch?AsOfDate=' + $('#EndDate').datebox('getValue')); //JSON.stringify(idarray)
    });

    $("#lookup_div_contact").dialog('close');
    $("#lookup_div_item").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        //url: base_url + 'MstContact/GetListCustomer',
        datatype: "local",
        ignoreCase: true,
        colNames: ['Id', 'Code', 'Item Id', 'Item Sku', 'Name', 'QTY', 'PendRcv', 'Price',
        ],
        colModel: [
                  { name: 'id', index: 'id', width: 40, sortable: false, align: 'center' },
                  { name: 'code', index: 'code', width: 70, sortable: false, align: 'center' },
				  { name: 'itemid', index: 'itemid', width: 100, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 70, sortable: false },
                  { name: 'itemname', index: 'itemname', width: 130, sortable: false },
                  { name: 'quantity', index: 'quantity', width: 50, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'pendingquantity', index: 'pendingquantity', width: 60, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'price', index: 'price', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        //sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        //sortorder: "ASC",
        multiselect: true,
        multiboxonly: true,
        width: window.innerWidth-24,
        height: screen.availHeight-250,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          //rowIsTaxable = $(this).getRowData(cl).istaxable;
		          //if (rowIsTaxable == 'true') {
		          //    rowIsTaxable = "YES"; // + $(this).getRowData(cl).taxcode;
		          //} else {
		          //    rowIsTaxable = "NO";
		          //}
		          //$(this).jqGrid('setRowData', ids[i], { istaxable: rowIsTaxable });

		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

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

    function indexOf(obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i] === obj) {
                return i;
            }
        }
        return -1;
    }

    $('#btn_del').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            ClearErrorMessage();

            var submitURL = '';

            var selectedIDs = jQuery('#list').jqGrid('getGridParam', 'selarrrow');
            //var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to exclude selected row(s)?', function (r) {
                if (r) {
                    for (var i = selectedIDs.length-1; i >= 0; i--)
                    //for(var i in selectedIDs)
                    {
                        jQuery("#list").jqGrid('delRowData', selectedIDs[i]); //
                    }
                    ReloadGrid();
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

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
        colNames: ['ID', 'Name', 'Faktur', 'Address', 'DeliveryAddress', 'Description', 'NPWP', 'Contact No', 'PIC', 'PIC Contact', 'Email', 'Tax Code', 'Taxable', 'Contact Group Id', 'Contact Group', 'Contact Type', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'namafakturpajak', index: 'namafakturpajak', width: 180 },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'deliveryaddress', index: 'deliveryaddress', width: 250 },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'npwp', index: 'npwp', width: 100 },
                  { name: 'contact', index: 'contactno', width: 100 },
                  { name: 'pic', index: 'pic', width: 120 },
                  { name: 'piccontact', index: 'piccontactno', width: 100 },
                  { name: 'email', index: 'email', width: 150 },
                  { name: 'taxcode', index: 'taxcode', width: 50 },
                  { name: 'istaxable', index: 'istaxable', width: 80, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':;true:Yes;false:No' } },
    			  { name: 'contactgroupid', index: 'contactgroupid', width: 60, align: "center", hidden: true },
				  { name: 'contactgroup', index: 'contactgroup', width: 180 },
				  { name: 'contacttype', index: 'contacttype', width: 60, hidden: true },
                  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_contact'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        //multiselect: true,
        //multiboxonly: true,
        width: $("#lookup_div_contact").width() - 10,
        height: $("#lookup_div_contact").height() - 110,
        ondblClickRow: function (rowid) {
            $("#lookup_btn_add_contact").trigger("click");
        },
    });
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: true })
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

            //var ids = $('#list').jqGrid('getDataIDs');
            //var selectedIDs = jQuery('#lookup_table_contact').jqGrid('getGridParam', 'selarrrow');
            //for (var i = 0; i < selectedIDs.length; i++)
            //{
            //    var selrow = jQuery("#lookup_table_contact").jqGrid('getRowData', selectedIDs[i]);
            //    var currowid = indexOf(selrow.id, ids);
            //    if (currowid >= 0) {
            //        //$('#list').jqGrid('setRowData', ids[i], selrow);
            //    }
            //    else {
            //        $('#list').jqGrid('addRowData', selrow.id, selrow);
            //    }
            //}

            $('#lookup_div_contact').dialog('close');
            ReloadGrid();
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up item-------------------------------------------------------
    $('#btnItem').click(function () {
        var lookUpURL = base_url + 'MstItem/GetList';
        var lookupGrid = $('#lookup_table_item');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_item').dialog('open');
    });

    jQuery("#lookup_table_item").jqGrid({
        url: base_url + 'Item/GetList',
        datatype: "json",
        colNames: ['ID', 'Sku', 'Name', 'QTY', 'PendReceival', 'PendDelivery', 'Minimum', 'Virtual', 'Customer QTY', 'UoM', 'Price', 'Avg Price'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 240 },
                  { name: 'quantity', index: 'quantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'minimum', index: 'minimum', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'virtual', index: 'virtual', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'customerquantity', index: 'customerquantity', width: 75, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uom', index: 'uom', width: 40 },
                  { name: 'price', index: 'price', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, hidden: true },
                  { name: 'avgprice', index: 'avgprice', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, hidden: true },
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
    $("#lookup_table_item").jqGrid('navGrid', '#lookup_toolbar_item', { del: false, add: false, edit: false, search: false })
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


}); //END DOCUMENT READY