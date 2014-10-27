$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstRollerBuilder/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_machine").dialog('close');
    $("#lookup_div_rollertype").dialog('close');
    $("#lookup_div_compound").dialog('close');
    $("#lookup_div_adhesive").dialog('close');
    $("#lookup_div_corebuilder").dialog('close');
    $("#lookup_div_uom").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $('#SkuRollerUsedCore').attr('disabled', true);
    $('#SkuRollerNewCore').attr('disabled', true);
    $("#MachineId").hide();
    $("#RollerTypeId").hide();
    $("#CompoundId").hide();
    $("#AdhesiveId").hide();
    $("#UoMId").hide();
    $("#CoreBuilderId").hide();


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstRollerBuilder/GetList',
        datatype: "json",
        colNames: ['ID', 'Base Sku', 'Name', 'RollerType', 'RD', 'CD', 'RL', 'WL', 'TL',
                   'Used Sku', 'QTY', 'UoM', 'New Sku', 'QTY', 'UoM', 
                   'Machine', 'Compound', 'Adhesive','Core Sku', 'Core', 'Description',
                   'Crown', 'Size', 'Groove', 'W', 'D', 'P', 'Chamfer',
                   'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
                  { name: 'basesku', index: 'basesku', width: 60 },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'rollertypename', index: 'rollertypename', width: 75 },
                  { name: 'rd', index: 'rd', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'cd', index: 'cd', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'rl', index: 'rl', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'wt', index: 'wt', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'tl', index: 'tl', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'skurollerusedcore', index: 'skurollerusedcore', width: 70 },
                  { name: 'rollerusedcorequantity', index: 'rollerusedcorequantity', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 30 },
                  { name: 'skurollernewcore', index: 'skurollernewcore', width: 70 },
                  { name: 'rollernewcorequantity', index: 'rollernewcorequantity', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'uom', index: 'uom', width: 30 },
                  { name: 'machinename', index: 'machinename', width: 100 },
                  { name: 'compoundname', index: 'compoundname', width: 100 },
                  { name: 'adhesivename', index: 'adhesivename', width: 100 },
                  { name: 'coresku', index: 'coresku', width: 60 },
                  { name: 'corebuildername', index: 'corebuildername', width: 80 },
                  { name: 'description', index: 'description', width: 80, hidden: true },
                  { name: 'iscrowning', index: 'iscrowning', width: 48, align: 'right' },
                  { name: 'crowningsize', index: 'crowningsize', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'isgrooving', index: 'isgrooving', width: 48, align: 'right' },
                  { name: 'groovingwidth', index: 'groovingwidth', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'groovingdepth', index: 'groovingdepth', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'groovingposition', index: 'groovingposition', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'ischamfer', index: 'ischamfer', width: 48, align: 'right' },
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
        sortorder: "ASC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowCrowning = $(this).getRowData(cl).iscrowning;
		          if (rowCrowning == 'true') {
		              rowCrowning = "Y";
		          } else {
		              rowCrowning = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iscrowning: rowCrowning });

		          rowGrooving = $(this).getRowData(cl).isgrooving;
		          if (rowGrooving == 'true') {
		              rowGrooving = "Y";
		          } else {
		              rowGrooving = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isgrooving: rowGrooving });

		          rowChamfer = $(this).getRowData(cl).ischamfer;
		          if (rowChamfer == 'true') {
		              rowChamfer = "Y";
		          } else {
		              rowChamfer = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { ischamfer: rowChamfer });
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
                url: base_url + "MstRollerBuilder/GetInfo?Id=" + id,
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
                            $('#BaseSku').val(result.BaseSku);
                            $('#SkuRollerUsedCore').val(result.SkuRollerUsedCore);
                            $('#SkuRollerNewCore').val(result.SkuRollerNewCore);
                            $('#UoMId').val(result.UoMId);
                            $('#UoM').val(result.UoM);
                            $('#Name').val(result.Name);
                            $('#Description').val(result.Description);
                            $('#MachineId').val(result.MachineId);
                            $('#Machine').val(result.Machine);
                            $('#RollerTypeId').val(result.RollerTypeId);
                            $('#RollerType').val(result.RollerType);
                            $('#CompoundId').val(result.CompoundId);
                            $('#Compound').val(result.Compound);
                            $('#AdhesiveId').val(result.AdhesiveId);
                            $('#Adhesive').val(result.Adhesive);
                            $('#CoreBuilderId').val(result.CoreBuilderId);
                            $('#CoreBuilder').val(result.CoreBuilder);
                            $('#RD').numberbox('setValue', (result.RD));
                            $('#CD').numberbox('setValue', (result.CD));
                            $('#RL').numberbox('setValue', (result.RL));
                            $('#WL').numberbox('setValue', (result.WL));
                            $('#TL').numberbox('setValue', (result.TL));
                            $('#RollerUsedCoreQuantity').numberbox('setValue', (result.RollerUsedCoreQuantity));
                            $('#RollerNewCoreQuantity').numberbox('setValue', (result.RollerNewCoreQuantity));
                            $('#CrowningSize').numberbox('setValue', (result.CrowningSize));
                            $('#GroovingDepth').numberbox('setValue', (result.GroovingDepth));
                            $('#GroovingWidth').numberbox('setValue', (result.GroovingWidth));
                            $('#GroovingPosition').numberbox('setValue', (result.GroovingPosition));
                            document.getElementById("iscrowning").checked = result.IsCrowning;
                            if (result.IsCrowning) { $('#CrowningSize').removeAttr('disabled'); } else { $('#CrowningSize').attr('disabled', true); }
                            document.getElementById("isgrooving").checked = result.IsGrooving;
                            if (result.IsGrooving) { $('#GroovingWidth').removeAttr('disabled'); } else { $('#GroovingWidth').attr('disabled', true); }
                            if (result.IsGrooving) { $('#GroovingDepth').removeAttr('disabled'); } else { $('#GroovingDepth').attr('disabled', true); }
                            if (result.IsGrooving) { $('#GroovingPosition').removeAttr('disabled'); } else { $('#GroovingPosition').attr('disabled', true); }
                            document.getElementById("ischamfer").checked = result.IsChamfer;
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#iscrowning').click(function () {
        if (document.getElementById("iscrowning").checked) { $('#CrowningSize').removeAttr('disabled'); } else { $('#CrowningSize').attr('disabled', true); }
    });

    $('#isgrooving').click(function () {
        if (document.getElementById("isgrooving").checked) {
            $('#GroovingWidth').removeAttr('disabled');
            $('#GroovingDepth').removeAttr('disabled');
            $('#GroovingPosition').removeAttr('disabled');
        }
        else {
            $('#GroovingWidth').attr('disabled', true);
            $('#GroovingDepth').attr('disabled', true);
            $('#GroovingPosition').attr('disabled', true);
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
            url: base_url + "MstRollerBuilder/Delete",
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
            submitURL = base_url + 'MstRollerBuilder/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstRollerBuilder/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, MachineId: $("#MachineId").val(), RollerTypeId: $("#RollerTypeId").val(),
                CompoundId: $("#CompoundId").val(), AdhesiveId: $("#AdhesiveId").val(), UoMId: $("#UoMId").val(), CoreBuilderId: $("#CoreBuilderId").val(),
                BaseSku: $("#BaseSku").val(), SkuRollerUsedCore: $("#BaseSku").val() + "U", SkuRollerNewCore: $("#BaseSku").val() + "N",
                Name: $("#Name").val(), Description: $("#Description").val(), RD: $("#RD").numberbox('getValue'),
                CD: $("#CD").numberbox('getValue'), RL: $("#RL").numberbox('getValue'), WL: $("#WL").numberbox('getValue'),
                TL: $("#TL").numberbox('getValue'), IsCrowning: document.getElementById("iscrowning").checked,
                CrowningSize: $("#CrowningSize").numberbox('getValue'), IsGrooving: document.getElementById("isgrooving").checked,
                GroovingWidth: $("#GroovingWidth").numberbox('getValue'), GroovingDepth: $("#GroovingDepth").numberbox('getValue'),
                GroovingPosition: $("#GroovingPosition").numberbox('getValue'), IsChamfer: document.getElementById("ischamfer").checked,
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
    $("#lookup_table_uom").jqGrid('navGrid', '#lookup_toolbar_uom', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
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
    $("#lookup_table_machine").jqGrid('navGrid', '#lookup_toolbar_machine', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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

    // -------------------------------------------------------Look Up rollertype-------------------------------------------------------
    $('#btnRollerType').click(function () {
        var lookUpURL = base_url + 'MstRollerType/GetList';
        var lookupGrid = $('#lookup_table_rollertype');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_rollertype').dialog('open');
    });

    jQuery("#lookup_table_rollertype").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_rollertype'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_rollertype").width() - 10,
        height: $("#lookup_div_rollertype").height() - 110,
    });
    $("#lookup_table_rollertype").jqGrid('navGrid', '#lookup_toolbar_rollertype', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_rollertype').click(function () {
        $('#lookup_div_rollertype').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_rollertype').click(function () {
        var id = jQuery("#lookup_table_rollertype").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_rollertype").jqGrid('getRowData', id);

            $('#RollerTypeId').val(ret.id).data("kode", id);
            $('#RollerType').val(ret.name);

            $('#lookup_div_rollertype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup rollertype----------------------------------------------------------------

    // -------------------------------------------------------Look Up compound-------------------------------------------------------
    $('#btnCompound').click(function () {
        var lookUpURL = base_url + 'MstRollerBuilder/GetListCompound';
        var lookupGrid = $('#lookup_table_compound');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_compound').dialog('open');
    });

    jQuery("#lookup_table_compound").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'SKU', 'Name',
                     'Description', 'Quantity', 'Pending Receival', 'Pending Delivery',
                     'UoM Id', 'UoM', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'sku', index: 'sku', width: 70 },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'description', index: 'description', width: 100, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 60 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updateat', index: 'updateat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_compound'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_compound").width() - 10,
        height: $("#lookup_div_compound").height() - 110,
    });
    $("#lookup_table_compound").jqGrid('navGrid', '#lookup_toolbar_compound', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_compound').click(function () {
        $('#lookup_div_compound').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_compound').click(function () {
        var id = jQuery("#lookup_table_compound").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_compound").jqGrid('getRowData', id);

            $('#CompoundId').val(ret.id).data("kode", id);
            $('#Compound').val(ret.name);

            $('#lookup_div_compound').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup compound----------------------------------------------------------------

    // -------------------------------------------------------Look Up adhesive-------------------------------------------------------
    $('#btnAdhesive').click(function () {
        var lookUpURL = base_url + 'MstRollerBuilder/GetListAdhesiveRoller';
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
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'description', index: 'description', width: 100, hidden: true },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 60 },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
				  { name: 'updateat', index: 'updateat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, hidden: true },
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
    $("#lookup_table_adhesive").jqGrid('navGrid', '#lookup_toolbar_adhesive', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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

    // -------------------------------------------------------Look Up corebuilder-------------------------------------------------------
    $('#btnCoreBuilder').click(function () {
        var lookUpURL = base_url + 'MstCoreBuilder/GetList';
        var lookupGrid = $('#lookup_table_corebuilder');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_corebuilder').dialog('open');
    });

    jQuery("#lookup_table_corebuilder").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',

        colNames: ['Id', 'Sku', 'Name', 'Description', 'Machine', 'Type'],
        colModel: [
                  { name: 'id', index: 'id', width: 40, align: 'right' },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 70, hidden: true },
                  { name: 'machine', index: 'machine', width: 100 },
                  { name: 'corebuildertypecase', index: 'corebuildertypecase', width: 60 }
        ],
        page: '1',
        pager: $('#lookup_pager_corebuilder'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_corebuilder").width() - 10,
        height: $("#lookup_div_corebuilder").height() - 110,
    });
    $("#lookup_table_corebuilder").jqGrid('navGrid', '#lookup_toolbar_corebuilder', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_corebuilder').click(function () {
        $('#lookup_div_corebuilder').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_corebuilder').click(function () {
        var id = jQuery("#lookup_table_corebuilder").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_corebuilder").jqGrid('getRowData', id);

            $('#CoreBuilderId').val(ret.id).data("kode", id);
            $('#CoreBuilder').val(ret.name);

            $('#lookup_div_corebuilder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup corebuilder----------------------------------------------------------------

}); //END DOCUMENT READY