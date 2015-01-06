$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'CoreIdentification/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'CoreIdentification/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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

    $("#item_div").dialog('close');
    $("#finished_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#lookup_div_corebuilder").dialog('close');
    $("#lookup_div_rollertype").dialog('close');
    $("#lookup_div_machine").dialog('close');
    $("#lookup_div_warehouse").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#CoreBuilderBaseSku").hide();
    $("#RollerTypeId").hide();
    $("#MachineId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'CoreIdentification/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'No. Diss', 'Warehouse', 'InHouseStock', 'Contact',
                   'QTY', 'Identified Date', 'Incoming Roll Date',
                   'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 70 },
                  { name: 'nodiss', index: 'nodiss', width: 70 },
                  { name: 'warehouse', index: 'warehouse', width: 150 },
                  { name: 'isinhouse', index: 'isinhouse', width: 50, align: 'right' },
                  { name: 'contact', index: 'contact', width: 130 },
                  { name: 'quantity', index: 'quantity', align: 'right', width: 30, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'identifieddate', index: 'identifieddate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'incomingroll', index: 'incomingroll', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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

		          rowIsInHouse = $(this).getRowData(cl).isinhouse;
		          if (rowIsInHouse == 'true') {
		              rowIsInHouse = "YES";
		          } else {
		              rowIsInHouse = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isinhouse: rowIsInHouse });
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
        $('#IdentifiedDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#IncomingRoll').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#Code').removeAttr('disabled');
        $('#NomorDisassembly').removeAttr('disabled');
        $('#btnWarehouse').removeAttr('disabled');
        $('#btnContact').removeAttr('disabled');
        $('#IsInHouse').removeAttr('disabled');
        $('#Quantity').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#IdentifiedDateDiv').show();
        $('#IdentifiedDateDiv2').hide();
        $('#IncomingRollDiv').show();
        $('#IncomingRollDiv2').hide();
        $('#form_btn_save').show();
        $('#form_div').dialog('open');
    });


    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "CoreIdentification/GetInfo?Id=" + id,
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
                            $('#NomorDisassembly').val(result.NomorDisassembly);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#Quantity').numberbox('setValue',result.Quantity);
                            var e = document.getElementById("IsInHouse");
                            if (result.IsInHouse == 1) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            $('#IdentifiedDate').datebox('setValue', dateEnt(result.IdentifiedDate));
                            $('#IdentifiedDate2').val(dateEnt(result.IdentifiedDate));
                            $('#IdentifiedDateDiv').hide();
                            $('#IdentifiedDateDiv2').show();
                            $('#IncomingRoll').datebox('setValue', dateEnt(result.IncomingRoll));
                            $('#IncomingRoll2').val(dateEnt(result.IncomingRoll));
                            $('#IncomingRollDiv').hide();
                            $('#IncomingRollDiv2').show();
                            $('#form_btn_save').hide();
                            $('#Code').attr('disabled', true);
                            $('#NomorDisassembly').attr('disabled', true);
                            $('#Quantity').attr('disabled', true);
                            $('#IsInHouse').attr('disabled', true);
                            $('#btnWarehouse').attr('disabled', true);
                            $('#btnContact').attr('disabled', true);
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



    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "CoreIdentification/GetInfo?Id=" + id,
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
                            $('#id').val(result.Id);
                            $('#Code').val(result.Code);
                            $('#NomorDisassembly').val(result.NomorDisassembly);
                            $('#WarehouseId').val(result.WarehouseId);
                            $('#Warehouse').val(result.Warehouse);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#Quantity').numberbox('setValue',result.Quantity);
                            var e = document.getElementById("IsInHouse");
                            if (result.IsInHouse == 1) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            $('#IdentifiedDate').datebox('setValue', dateEnt(result.IdentifiedDate));
                            $('#IdentifiedDatee2').val(dateEnt(result.IdentifiedDate));
                            $('#IdentifiedDateDiv').show();
                            $('#IdentifiedDateDiv2').hide();
                            $('#IncomingRoll').datebox('setValue', dateEnt(result.IncomingRoll));
                            $('#IncomingRolle2').val(dateEnt(result.IncomingRoll));
                            $('#IncomingRollDiv').show();
                            $('#IncomingRollDiv2').hide();
                            $('#form_btn_save').hide();
                            $('#Code').removeAttr('disabled');
                            $('#NomorDisassembly').removeAttr('disabled');
                            $('#Quantity').removeAttr('disabled');
                            $('#IsInHouse').removeAttr('disabled');
                            $('#btnWareHouse').removeAttr('disabled');
                            $('#btnContact').removeAttr('disabled');
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
                        url: base_url + "CoreIdentification/Unconfirm",
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
        $.ajax({
            url: base_url + "CoreIdentification/Confirm",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
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
            url: base_url + "CoreIdentification/Delete",
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
        if ($('#IdentifiedDate').datebox('getValue') == "") {
            return $($('#IdentifiedDate').addClass('errormessage').before('<span class="errormessage">**' + "Identified Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'CoreIdentification/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'CoreIdentification/Insert';
        }

        var e = document.getElementById("IsInHouse");
        var moving = e.options[e.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Code: $("#Code").val(), WarehouseId: $("#WarehouseId").val(), ContactId: $("#ContactId").val(),
                IsInHouse: moving, Quantity: $("#Quantity").numberbox('getValue'), NomorDisassembly: $('#NomorDisassembly').val(),
                IdentifiedDate: $('#IdentifiedDate').datebox('getValue'), IncomingRoll: $('#IncomingRoll').datebox('getValue')
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

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['RIF Id', 'RollerIdentificationId', 'Roller No', 'Material', 'CoreBuilder Id',
                    'Core Sku', 'Core', 'RollerType Id', 'RollerType',
                    'Machine Id', 'Machine', 'Repair', 'RD', 'CD', 'RL', 'WL', 'TL',
                    'GL', 'Groove Len', 'Qty Grooves', 'Core'
        ],
        colModel: [
                  { name: 'detailid', index: 'detailid', width: 40, sortable: false},
                  { name: 'rolleridentificationid', index: 'rolleridentificationid', width: 130, sortable: false, hidden: true },
                  { name: 'rollerno', index: 'rollerno', width: 60, sortable: false },
                  { name: 'materialcase', index: 'materialcase', width: 60, sortable: false },
                  { name: 'corebuilderid', index: 'corebuilderid', width: 80, sortable: false, hidden: true },
                  { name: 'corebuilderbasesku', index: 'corebuilderbasesku', width: 70, sortable: false },
                  { name: 'corebuildername', index: 'corebuildername', width: 80, sortable: false },
                  { name: 'rollertypeid', index: 'rollertypeid', width: 80, sortable: false, hidden: true },
                  { name: 'rollertypename', index: 'rollertypename', width: 70, sortable: false },
                  { name: 'machineid', index: 'machineid', width: 80, sortable: false, hidden: true},
                  { name: 'machinename', index: 'machinename', width: 90, sortable: false },
                  { name: 'repairrequestcase', index: 'repairrequestcase', width: 90, sortable: false, stype: 'select', editoptions: { value: ':;1:BearingSeat;2:CentreDrill;3:None' }},
                  { name: 'rd', index: 'rd', width: 40, align: 'right', sortable: false },
                  { name: 'cd', index: 'cd', width: 40, align: 'right', sortable: false },
                  { name: 'rl', index: 'rl', width: 40, align: 'right', sortable: false },
                  { name: 'wl', index: 'wl', width: 40, align: 'right', sortable: false },
                  { name: 'tl', index: 'tl', width: 40, align: 'right', sortable: false },
                  { name: 'gl', index: 'tl', width: 40, align: 'right', sortable: false },
                  { name: 'groovelength', index: 'groovelength', width: 65, align: 'right', sortable: false },
                  { name: 'grooveqty', index: 'grooveqty', width: 65, align: 'right', sortable: false },
                  { name: 'coretypecase', index: 'coretypecase', width: 30, align: 'right', sortable: false, stype: 'select', editoptions: { value: ':;R:Hollow;Z:Shaft' } },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'detailid',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsConfirmed = $(this).getRowData(cl).isfinished;
		          if (rowIsConfirmed == 'true') {
		              rowIsConfirmed = "YES";
		          } else {
		              rowIsConfirmed = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isfinished: rowIsConfirmed });

		          rowCoreTypeCase = $(this).getRowData(cl).coretypecase;
		          if (rowCoreTypeCase == 'Hollow') {
		              rowCoreTypeCase = "R";
		          } else if (rowCoreTypeCase == 'Shaft') {
		              rowCoreTypeCase = "Z";
		          } else {
		              rowCoreTypeCase = "-";
		          }
		          $(this).jqGrid('setRowData', ids[i], { coretypecase: rowCoreTypeCase });

		          rowRepairRequestCase = $(this).getRowData(cl).repairrequestcase;
		          if (rowRepairRequestCase == '1') {
		              rowRepairRequestCase = "BearingSeat";
		          } else if (rowRepairRequestCase == '2') {
		              rowRepairRequestCase = "CentreDrill";
		          } else if (rowRepairRequestCase == '3') {
		              rowRepairRequestCase = "None";
		          }
		          $(this).jqGrid('setRowData', ids[i], { repairrequestcase: rowRepairRequestCase });
              }
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "CoreIdentification/GetInfoDetail?Id=" + id,
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
                            $("#item_btn_submit").data('kode', result.Id);
                            $('#DetailId').numberbox('setValue', result.DetailId);
                            var e = document.getElementById("MaterialCase");
                            if (result.MaterialCase == 1) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var f = document.getElementById("RepairRequestCase");
                            if (result.RepairRequestCase == 1) {
                                f.selectedIndex = 0;
                            }
                            else if (result.RepairRequestCase == 2) {
                                f.selectedIndex = 1;
                            }
                            else if (result.RepairRequestCase == 3) {
                                f.selectedIndex = 2;
                            }
                            var g = document.getElementById("CoreTypeCase");
                            if (result.CoreTypeCase == 'Hollow') {
                                g.selectedIndex = 0;
                            }
                            else if (result.CoreTypeCase == 'Shaft') {
                                g.selectedIndex = 1;
                            }
                            else {
                                g.selectedIndex = 2;
                            }

                            $('#CoreIdentificationId').val(result.CoreIdentificationId);
                            $('#RollerNo').val(result.RollerNo);
                            $('#CoreBuilderId').val(result.CoreBuilderId);
                            $('#CoreBuilderBaseSku').val(result.CoreBuilderBaseSku);
                            $('#CoreBuilder').val(result.CoreBuilder);
                            $('#RollerTypeId').val(result.RollerTypeId);
                            $('#RollerType').val(result.RollerType);
                            $('#MachineId').val(result.MachineId);
                            $('#Machine').val(result.MachineId);
                            $('#RD').numberbox('setValue', result.RD);
                            $('#CD').numberbox('setValue', result.CD);
                            $('#RL').numberbox('setValue', result.RL);
                            $('#WL').numberbox('setValue', result.WL);
                            $('#TL').numberbox('setValue', result.TL);
                            $('#GL').numberbox('setValue', result.GL);
                            $('#GrooveLength').numberbox('setValue', result.GrooveLength);
                            $('#GrooveQTY').numberbox('setValue', result.GrooveQTY);
                            $('#item_div').dialog('open');
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
                        url: base_url + "CoreIdentification/DeleteDetail",
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
                                $('#AmountPayable').val(result.AmountPayable);
                                ReloadGridDetail();
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
    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // item_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'CoreIdentification/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'CoreIdentification/InsertDetail';
        }
        var e = document.getElementById("MaterialCase");
        var materialcase = e.options[e.selectedIndex].value;
        var f = document.getElementById("RepairRequestCase");
        var repairrequestcase = f.selectedIndex + 1;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, DetailId: $("#DetailId").numberbox('getValue'), CoreIdentificationId: $("#id").val(), RollerNo: $("#RollerNo").val(),
                MaterialCase: materialcase, CoreBuilderId: $("#CoreBuilderId").val(), CoreBuilderBaseSku: $("#CoreBuilderBaseSku").val(), RollerTypeId: $("#RollerTypeId").val(),
                MachineId: $("#MachineId").val(), RD: $("#RD").numberbox('getValue'), CD: $("#CD").numberbox('getValue'), RL: $("#RL").numberbox('getValue'),
                WL: $("#WL").numberbox('getValue'), TL: $("#TL").numberbox('getValue'), RepairRequestCase: repairrequestcase,
                GL: $("#GL").numberbox('getValue'), GrooveLength: $("#GrooveLength").numberbox('getValue'), GrooveQTY: $("#GrooveQTY").numberbox('getValue')
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
                    $('#AmountPayable').val(result.AmountPayable);
                    ReloadGridDetail();
                    $("#item_div").dialog('close')
                }
            }
        });
    });


    // item_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up warehouse-------------------------------------------------------
    $('#btnWarehouse').click(function () {
        var lookUpURL = base_url + 'MstWarehouse/GetList';
        var lookupGrid = $('#lookup_table_warehouse');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_warehouse').dialog('open');
    });

    jQuery("#lookup_table_warehouse").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Code', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                   { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_warehouse'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_warehouse").width() - 10,
        height: $("#lookup_div_warehouse").height() - 110,
    });
    $("#lookup_table_warehouse").jqGrid('navGrid', '#lookup_toolbar_warehouse', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_warehouse').click(function () {
        $('#lookup_div_warehouse').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_warehouse').click(function () {
        var id = jQuery("#lookup_table_warehouse").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_warehouse").jqGrid('getRowData', id);

            $('#WarehouseId').val(ret.id).data("kode", id);
            $('#Warehouse').val(ret.name);

            $('#lookup_div_warehouse').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup warehouse----------------------------------------------------------------

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
                  { name: 'name', index: 'name', width: 200 }],
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
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
                  { name: 'basesku', index: 'basesku', width: 100 },
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
            $('#CoreBuilderBaseSku').val(ret.sku);
            $('#CoreBuilder').val(ret.name);
            if (ret.corebuildertypecase == "Hollow") { document.getElementById("CoreTypeCase").selectedIndex = 0; }
            else if (ret.corebuildertypecase == "Shaft") { document.getElementById("CoreTypeCase").selectedIndex = 1; }
            else if (ret.corebuildertypecase == "None") { document.getElementById("CoreTypeCase").selectedIndex = 2; }
            $('#lookup_div_corebuilder').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup corebuilder----------------------------------------------------------------


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

  

}); //END DOCUMENT READY