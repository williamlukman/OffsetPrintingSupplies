﻿$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstBlanket/GetList', postData: { filters: null } }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#CroppingType").change(function (e) {
        if ($("#CroppingType").val() == "NO") {
            $("#LeftOverAC").numberbox('setValue', 0);
            $("#LeftOverAR").numberbox('setValue', 0);
            $("#Special").numberbox('setValue', 0);
        }
    });

    $("#form_div").dialog('close');
    $("#lookup_div_machine").dialog('close');
    $("#lookup_div_adhesive").dialog('close');
    $("#lookup_div_adhesive2").dialog('close');
    $("#lookup_div_rollBlanket").dialog('close');
    $("#lookup_div_leftbaritem").dialog('close');
    $("#lookup_div_rightbaritem").dialog('close');
    $("#lookup_div_uom").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#ItemTypeRow").hide();
    $("#KSRow").hide();
    $("#UoMId").hide();
    $("#ContactId").hide();
    $("#MachineId").hide();
    $("#AdhesiveId").hide();
    $("#Adhesive2Id").hide();
    $("#RollBlanketItemId").hide();
    $("#ItemTypeId").hide();
    $("#LeftBarItemId").hide();
    $("#RightBarItemId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstBlanket/GetList',
        datatype: "json",
        colNames: ['ID', 'Sku', 'Name', 'Roll No', 'QTY', 'UoM',
                   'AC', 'AR', 'Thickness', 'KS', 'Description',
                   'ItemType', 'Machine', 'Adhesive', 'Adhesive2', 'Roll Blanket', 
                   'Bar1', 'Bar2', 'Customer',
                   'Application', 'Cropping', 'AC-', 'AR-',
                   'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'sku', index: 'sku', width: 50 },
				  { name: 'name', index: 'name', width: 400 },
                  { name: 'rollno', index: 'rollno', width: 70, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 50, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 30 },
                  { name: 'ac', index: 'ac', width: 30, formatter: 'integer', align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'ar', index: 'ar', width: 30, formatter: 'integer', align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'thickness', index: 'thickness', width: 30, formatter: 'integer', align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'ks', index: 'ks', width: 30, formatter: 'integer', align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'description', index: 'description', width: 90, align: 'right', hidden: true },
                  { name: 'itemtypename', index: 'itemtypename', width: 60, hidden: true },
                  { name: 'machinename', index: 'machinename', width: 90 },
                  { name: 'adhesivename', index: 'adhesivename', width: 90 },
                  { name: 'adhesive2name', index: 'adhesive2name', width: 90 },
                  { name: 'rollBlanketname', index: 'rollBlanketname', width: 90 },
                  { name: 'leftbaritemname', index: 'leftbaritemname', width: 90 },
                  { name: 'rightbaritemname', index: 'rightbaritemname', width: 90 },
                  { name: 'contact', index: 'contact', width: 130 },
                  { name: 'applicationcase', index: 'applicationcase', width: 65 },
                  { name: 'croppingtype', index: 'croppingtype', width: 60 },
                  { name: 'leftoverac', index: 'leftoverac', width: 40, align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'leftoverar', index: 'leftoverar', width: 40, align: 'right', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
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
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
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
                url: base_url + "MstBlanket/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    //else if (result.ConfirmationDate == null) {
                    //    $.messager.alert('Information', 'Data belum dikonfirmasi...!!', 'info');
                    //}
                    else {
                        window.open(base_url + "Report/PrintoutIdentifyBlanket?Id=" + id);
                    }
                }
            });
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode

        $('#form_div').dialog('open');
        $.ajax({
            dataType: "json",
            url: base_url + "MstItemType/GetInfoByName?itemType=Blanket",
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
                        $('#ItemTypeId').val(result.Id);
                        $('#ItemType').val(result.Name);
                        $('#Sku').val(result.SKU);
                    }
                }
            }
        });
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");        
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "MstBlanket/GetInfo?Id=" + id,
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
                            $('#Description').val(result.Description);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemType').val(result.ItemType);
                            $('#UoMId').val(result.UoMId);
                            $('#UoM').val(result.UoM);
                            $('#Sku').val(result.Sku);
                            $('#RollNo').val(result.RollNo);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#MachineId').val(result.MachineId);
                            $('#Machine').val(result.Machine);
                            $('#AdhesiveId').val(result.AdhesiveId);
                            $('#Adhesive').val(result.Adhesive);
                            $('#Adhesive2Id').val(result.Adhesive2Id);
                            $('#Adhesive2').val(result.Adhesive2);
                            document.getElementById("IsBarRequired").value = result.IsBarRequired == true ? true : false;
                            $('#RollBlanketItemId').val(result.RollBlanketItemId);
                            $('#RollBlanketItem').val(result.RollBlanketItem);
                            $('#LeftBarItemId').val(result.LeftBarItemId);
                            $('#LeftBarItem').val(result.LeftBarItem);
                            $('#RightBarItemId').val(result.RightBarItemId);
                            $('#RightBarItem').val(result.RightBarItem);
                            $('#AC').numberbox('setValue', (result.AC));
                            $('#AR').numberbox('setValue', (result.AR));
                            $('#thickness').numberbox('setValue', (result.thickness));
                            $('#KS').numberbox('setValue', (result.KS));
                            $('#Quantity').numberbox('setValue', (result.Quantity));
                            document.getElementById("ApplicationCase").value = result.ApplicationCase;
                            document.getElementById("CroppingType").value = result.CroppingType;
                            $('#LeftOverAC').numberbox('setValue', (result.LeftOverAC));
                            $('#LeftOverAR').numberbox('setValue', (result.LeftOverAR));
                            $('#Special').numberbox('setValue', (result.Special));
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
            url: base_url + "MstBlanket/Delete",
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
            submitURL = base_url + 'MstBlanket/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstBlanket/Insert';
        }

        var e = document.getElementById("IsBarRequired");
        var isbar = e.options[e.selectedIndex].value;
        var f = document.getElementById("CroppingType");
        var croppingtype = f.options[f.selectedIndex].value;
        var hasleftbar = $('#LeftBarItemId').val() == '' ? false : true;
        var hasrightbar = $('#RightBarItemId').val() == '' ? false : true;
        var g = document.getElementById("ApplicationCase");
        var applicationcase = g.options[g.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ItemTypeId: $("#ItemTypeId").val(), Sku: $("#Sku").val(), Name: $("#Name").val(),
                Description: $("#Description").val(), UoMId: $("#UoMId").val(), RollNo: $("#RollNo").val(),
                ContactId: $("#ContactId").val(), MachineId: $("#MachineId").val(), AdhesiveId: $("#AdhesiveId").val(),
                Adhesive2Id: $("#Adhesive2Id").val(), RollBlanketItemId: $("#RollBlanketItemId").val(), IsBarRequired: isbar,
                HasLeftBar: hasleftbar, HasRightBar: hasrightbar, LeftBarItemId: $("#LeftBarItemId").val(),
                RightBarItemId: $("#RightBarItemId").val(), AC: $("#AC").val(), AR: $("#AR").val(),
                thickness: $("#thickness").val(), KS: $("#KS").val(), ApplicationCase: applicationcase,
                CroppingType: croppingtype, LeftOverAC: $("#LeftOverAC").numberbox('getValue'), LeftOverAR: $("#LeftOverAR").numberbox('getValue'),
                Special: $("#Special").numberbox('getValue')
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
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
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
            $('#UoM').val(ret.name);

            $('#lookup_div_uom').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup uom----------------------------------------------------------------

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
        colNames: ['Id', 'Name', 'KodeUoM', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'kodeuom', index: 'kodeuom', width: 60 },
                  { name: 'description', index: 'description', width: 150 },
        ],
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


    // -------------------------------------------------------Look Up machine-------------------------------------------------------
    $('#btnMachine').click(function () {
        var lookUpURL = base_url + 'MstMachine/GetList';
        var lookupGrid = $('#lookup_table_machine');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_machine').dialog('open');
    });

    jQuery("#lookup_table_machine").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Code', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_machine'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_machine").width() - 10,
        height: $("#lookup_div_machine").height() - 110,
    });
    $("#lookup_table_machine").jqGrid('navGrid', '#lookup_toolbar_machine', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_machine').click(function () {
        $('#lookup_div_machine').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_machine').click(function () {
        var id = jQuery("#lookup_table_machine").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_machine").jqGrid('getRowData', id);

            $('#MachineId').val(ret.id).data("kode", id);
            $('#Machine').val(ret.name);

            $('#lookup_div_machine').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup machine----------------------------------------------------------------

    // -------------------------------------------------------Look Up adhesive-------------------------------------------------------
    $('#btnAdhesive').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListAdhesiveBlanket';
        var lookupGrid = $('#lookup_table_adhesive');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_adhesive').dialog('open');
    });

    jQuery("#lookup_table_adhesive").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 100, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 60 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true},
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_adhesive'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_adhesive").width() - 10,
        height: $("#lookup_div_adhesive").height() - 110,
    });
    $("#lookup_table_adhesive").jqGrid('navGrid', '#lookup_toolbar_adhesive', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_adhesive').click(function () {
        $('#lookup_div_adhesive').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_adhesive').click(function () {
        var id = jQuery("#lookup_table_adhesive").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_adhesive").jqGrid('getRowData', id);

            $('#AdhesiveId').val(ret.id).data("kode", id);
            $('#Adhesive').val(ret.name);

            $('#lookup_div_adhesive').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup adhesive----------------------------------------------------------------

    // -------------------------------------------------------Look Up adhesive2-------------------------------------------------------
    $('#btnAdhesive2').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListAdhesiveBlanket';
        var lookupGrid = $('#lookup_table_adhesive2');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_adhesive2').dialog('open');
    });

    jQuery("#lookup_table_adhesive2").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 200 },
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
        pager: $('#lookup_pager_adhesive2'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_adhesive2").width() - 10,
        height: $("#lookup_div_adhesive2").height() - 110,
    });
    $("#lookup_table_adhesive2").jqGrid('navGrid', '#lookup_toolbar_adhesive2', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_adhesive2').click(function () {
        $('#lookup_div_adhesive2').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_adhesive2').click(function () {
        var id = jQuery("#lookup_table_adhesive2").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_adhesive2").jqGrid('getRowData', id);

            $('#Adhesive2Id').val(ret.id).data("kode", id);
            $('#Adhesive2').val(ret.name);

            $('#lookup_div_adhesive2').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup adhesive2----------------------------------------------------------------

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

            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up rollBlanket-------------------------------------------------------
    $('#btnRollBlanketItem').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListRollBlanket';
        var lookupGrid = $('#lookup_table_rollBlanket');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_rollBlanket').dialog('open');
    });

    jQuery("#lookup_table_rollBlanket").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 100, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 60 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true},
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_rollBlanket'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_rollBlanket").width() - 10,
        height: $("#lookup_div_rollBlanket").height() - 110,
    });
    $("#lookup_table_rollBlanket").jqGrid('navGrid', '#lookup_toolbar_rollBlanket', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_rollBlanket').click(function () {
        $('#lookup_div_rollBlanket').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_rollBlanket').click(function () {
        var id = jQuery("#lookup_table_rollBlanket").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_rollBlanket").jqGrid('getRowData', id);

            $('#RollBlanketItemId').val(ret.id).data("kode", id);
            $('#RollBlanketItem').val(ret.name);

            $('#lookup_div_rollBlanket').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup rollBlanket----------------------------------------------------------------

    // -------------------------------------------------------Look Up leftbaritem-------------------------------------------------------
    $('#btnLeftBarItem').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListBar';
        var lookupGrid = $('#lookup_table_leftbaritem');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_leftbaritem').dialog('open');
    });

    jQuery("#lookup_table_leftbaritem").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 200 },
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
        pager: $('#lookup_pager_leftbaritem'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_leftbaritem").width() - 10,
        height: $("#lookup_div_leftbaritem").height() - 110,
    });
    $("#lookup_table_leftbaritem").jqGrid('navGrid', '#lookup_toolbar_leftbaritem', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_leftbaritem').click(function () {
        $('#lookup_div_leftbaritem').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_leftbaritem').click(function () {
        var id = jQuery("#lookup_table_leftbaritem").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_leftbaritem").jqGrid('getRowData', id);

            $('#LeftBarItemId').val(ret.id).data("kode", id);
            $('#LeftBarItem').val(ret.name);
            $('#lookup_div_leftbaritem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup leftbaritem----------------------------------------------------------------

    // -------------------------------------------------------Look Up rightbaritem-------------------------------------------------------
    $('#btnRightBarItem').click(function () {
        var lookUpURL = base_url + 'MstBlanket/GetListBar';
        var lookupGrid = $('#lookup_table_rightbaritem');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_rightbaritem').dialog('open');
    });

    jQuery("#lookup_table_rightbaritem").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 200 },
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
        pager: $('#lookup_pager_rightbaritem'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_rightbaritem").width() - 10,
        height: $("#lookup_div_rightbaritem").height() - 110,
    });
    $("#lookup_table_rightbaritem").jqGrid('navGrid', '#lookup_toolbar_rightbaritem', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_rightbaritem').click(function () {
        $('#lookup_div_rightbaritem').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_rightbaritem').click(function () {
        var id = jQuery("#lookup_table_rightbaritem").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_rightbaritem").jqGrid('getRowData', id);

            $('#RightBarItemId').val(ret.id).data("kode", id);
            $('#RightBarItem').val(ret.name);
            $('#lookup_div_rightbaritem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup rightbaritem----------------------------------------------------------------

    // --------------------------------------------- Start OnChange IsBarRequired ----------------------------------------------------------------

    $('#IsBarRequired').change(function(event) {
        var e = document.getElementById("IsBarRequired");
        var isbar = e.options[e.selectedIndex].value;

        if (e.value == "False") {
            $('#RightBarItemId').val('');
            $('#RightBarItem').val('');
            $('#LeftBarItemId').val('');
            $('#LeftBarItem').val('');

            $("#leftBarRow").hide();
            $("#rightBarRow").hide();
        }
        else {
            $("#leftBarRow").show();
            $("#rightBarRow").show();
            var id = $('#id').val();
            if (id) {
                $.ajax({
                    dataType: "json",
                    url: base_url + "MstBlanket/GetInfo?Id=" + id,
                    success: function (result) {
                        $('#LeftBarItemId').val(result.LeftBarItemId);
                        $('#LeftBarItem').val(result.LeftBarItem);
                        $('#RightBarItemId').val(result.RightBarItemId);
                        $('#RightBarItem').val(result.RightBarItem);
                    }
                });
            }
        }
    });

}); //END DOCUMENT READY