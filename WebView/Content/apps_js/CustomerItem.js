﻿$(document).ready(function () {
    var a;
    $("#detail_panel").hide();
    $("#lookup_div_contact").dialog('close');

    function ReloadGridListWarehouse() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'MstWarehouse/GetList',
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Code', 'Name'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'code', index: 'code', width: 80 },
                      { name: 'name', index: 'name', width: 200},
            ],
            page: '1',
            pager: jQuery('#pager'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#toolbar").width(),
            height: $(window).height() - 190,
            onSelectRow: function (id) {
                var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
                if (id) {
                    $("#list_detail").jqGrid('GridUnload');
                    $("#detail_panel").show();
                    ReloadGridViewItem(id);

                } else {
                    $.messager.alert('Information', 'Data Not Found...!!', 'info');
                };
            },
            gridComplete:
              function () {
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    //    $("#list").setGridParam({ url: base_url + 'MstWarehouse/GetList', page: '1' }).trigger("reloadGrid");

         }

    function ReloadGridListItem() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'MstItem/GetList',
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
            pager: jQuery('#pager'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: true,
            sortorder: "asc",
            width: $("#toolbar").width(),
            height: $(window).height() - 190,
            onSelectRow: function (id) {
                var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
                if (id) {
                    $("#list_detail").jqGrid('GridUnload');
                    $("#detail_panel").show();
                    ReloadGridViewWarehouse(id);
                } else {
                    $.messager.alert('Information', 'Data Not Found...!!', 'info');
                };
            },
            gridComplete:
              function () {
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    }

    // customeritem
    function ReloadGridViewItem(id) {
        
        $("#list_detail").jqGrid({
            url: base_url + 'CustomerItem/GetListItem?Id=' + id + '&cid=' + $("#ContactId").val(),
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Sku', 'Item Name', 'QTY', 'UoM'
            ],
            colModel: [
                      { name: 'itemid', index: 'itemid', width: 80, align: "center"},
                      { name: 'sku', index: 'sku', width: 80 },
                      { name: 'item', index: 'item', width: 240},
                      { name: 'quantity', index: 'quantity', width: 100, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'uomname', index: 'uomname', width: 100 },
            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'itemid',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 190,
            gridComplete:
              function () {
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }

    // itemwarehouse
    function ReloadGridViewWarehouse(id) {
        
        $("#list_detail").jqGrid({
            url: base_url + 'CustomerItem/GetListWarehouse?Id=' + id + '&cid=' + $("#ContactId").val(),
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Code', 'Name', 'QTY', 'UoM'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center"},
                      { name: 'code', index: 'code', width: 80 },
                      { name: 'name', index: 'name', width: 240 },
                      { name: 'quantity', index: 'quantity', width: 100, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'uomname', index: 'uomname', width: 100 },
            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 190,
            gridComplete:
              function () {
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    }


    ReloadGridListWarehouse();

    $('#btn_warehouse').click(function () {
        a = 'warehouse';
        $("#list").jqGrid('GridUnload');
        ReloadGridListWarehouse();
    });
 
    $('#btn_item').click(function () {
        a = 'item';
        $("#list").jqGrid('GridUnload');
        ReloadGridListItem();
    });


    ////GRID +++++++++++++++
    //$("#list").jqGrid({
    //    url: base_url + 'PaymentVoucher/GetList',
    //    datatype: "json",
    //    colNames: ['ID', 'Code', 'Name', 'Description', 'Created At', 'Updated At'],
    //    colModel: [
    //              { name: 'id', index: 'id', width: 80, align: "center" },
    //              { name: 'code', index: 'code', width: 80 },
    //              { name: 'name', index: 'name', width: 80 },
    //              { name: 'description', index: 'description', width: 250 },
    //              { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
    //              { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
    //    ],
    //    page: '1',
    //    pager: jQuery('#pager'),
    //    rowNum: 20,
    //    rowList: [20, 30, 60],
    //    sortname: 'id',
    //    viewrecords: true,
    //    scrollrows: true,
    //    shrinkToFit: false,
    //    sortorder: "DESC",
    //    width: $("#toolbar").width(),
    //    height: $(window).height() - 200,
    //    onSelectRow: function (id) {
    //        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
    //        if (id) {
    //            $("#list_detail").setGridParam({ url: base_url + 'CustomerItem/GetListItem?Id=' + id, postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    //        } else {
    //            $.messager.alert('Information', 'Data Not Found...!!', 'info');
    //        };
    //    },
    //    gridComplete:
    //      function () {
    //      }

    //});//END GRID
    //$("#list").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: true });
    //// .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        if (a == 'item') {
            $("#list").jqGrid('GridUnload');
            ReloadGridListItem();
        }
        else {
            $("#list").jqGrid('GridUnload');
            ReloadGridListWarehouse();
        }
        
    });

    $('#detail_btn_reload').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (a == 'item') {
            $("#list_detail").jqGrid('GridUnload');
            ReloadGridViewWarehouse(id);
        }
        else {
            $("#list_detail").jqGrid('GridUnload');
            ReloadGridViewItem(id);

        }

    });
    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    //GRID Detail+++++++++++++++
   
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

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
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 250 }],
        page: '1',
        pager: $('#lookup_pager_contact'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_contact").width() - 10,
        height: $("#lookup_div_contact").height() - 110,
    });
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: false })
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

            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

}); //END DOCUMENT READY