$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function onIsTaxable() {
        if (document.getElementById('IsTaxable').checked) {
            $('#TaxCode').removeAttr('disabled');
        } else {
            $('#TaxCode').attr('disabled', true);
        }
    };

    document.getElementById('IsTaxable').onchange = function () {
        onIsTaxable();
    };

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstContact/GetListSupplier', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_contactgroup").dialog('close');
    $("#ContactGroupId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstContact/GetListSupplier',
        datatype: "json",
        colNames: ['ID', 'Name', 'Faktur', 'Address', 'DeliveryAddress', 'Description', 'NPWP', 'Contact No', 'PIC', 'PIC Contact', 'Email', 'Tax Code', 'Taxable', 'Contact Group Id', 'Contact Group', 'Contact Type', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'namafakturpajak', index: 'namafakturpajak', width: 180, hidden: true },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'deliveryaddress', index: 'deliveryaddress', width: 250, hidden: true },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'npwp', index: 'npwp', width: 100, hidden: true },
                  { name: 'contactno', index: 'contactno', width: 150 },
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
		          rowIsTaxable = $(this).getRowData(cl).istaxable;
		          if (rowIsTaxable == 'true') {
		              rowIsTaxable = "YES"; // + $(this).getRowData(cl).taxcode;
		          } else {
		              rowIsTaxable = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { istaxable: rowIsTaxable });

		      }
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
        document.getElementById("ContactType").selectedIndex = 0;
        document.getElementById("IsTaxable").checked = true;
        $('#btnContactGroup').removeAttr('disabled');
        onIsTaxable();
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
                url: base_url + "MstContact/GetInfo?Id=" + id,
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
                            $("#form_btn_save").data('kode', id);
                            $('#id').val(result.Id);
                            $('#Name').val(result.Name);
                            $('#Address').val(result.Address);
                            $('#ContactNo').val(result.ContactNo);
                            $('#PIC').val(result.PIC);
                            $('#PICContactNo').val(result.PICContactNo);
                            $('#Email').val(result.Email);
                            $('#Description').val(result.Description);
                            $('#ContactGroupId').val(result.ContactGroupId);
                            $('#ContactGroup').val(result.ContactGroup);
                            //$('#btnContactGroup').attr('disabled', true);
                            document.getElementById("ContactType").selectedIndex = 0;
                            document.getElementById("IsTaxable").checked = result.IsTaxable;
                            onIsTaxable();
                            var e = document.getElementById("TaxCode");
                            var taxcode = result.TaxCode;
                            if (result.TaxCode == "01"){ e.selectedIndex = 0; }
                            else if (result.TaxCode == "02") { e.selectedIndex = 1; }
                            else if (result.TaxCode == "03") { e.selectedIndex = 2; }
                            else if (result.TaxCode == "04") { e.selectedIndex = 3; }
                            else if (result.TaxCode == "05") { e.selectedIndex = 4; }
                            else if (result.TaxCode == "06") { e.selectedIndex = 5; }
                            else if (result.TaxCode == "07") { e.selectedIndex = 6; }
                            else if (result.TaxCode == "08") { e.selectedIndex = 7; }
                            else if (result.TaxCode == "09") { e.selectedIndex = 8; }
                            $("#form_div").dialog("open");
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
            url: base_url + "MstContact/Delete",
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
            submitURL = base_url + 'MstContact/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstContact/Insert';
        }

        var e = document.getElementById("TaxCode");
        var taxcode = e.options[e.selectedIndex].value;
        var f = document.getElementById("ContactType");
        var contacttype = f.options[f.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(), Address: $("#Address").val(),
                ContactNo: $("#ContactNo").val(), PIC: $("#PIC").val(), PICContactNo: $("#PICContactNo").val(),
                Email: $("#Email").val(), TaxCode: taxcode, IsTaxable: document.getElementById("IsTaxable").checked ? 'true' : 'false',
                ContactType: contacttype, Description: $("#Description").val(), ContactGroupId: $("#ContactGroupId").val()
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

    // -------------------------------------------------------Look Up contactgroup-------------------------------------------------------
    $('#btnContactGroup').click(function () {
        var lookUpURL = base_url + 'ContactGroup/GetList';
        var lookupGrid = $('#lookup_table_contactgroup');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_contactgroup').dialog('open');
    });

    jQuery("#lookup_table_contactgroup").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'Description', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
				  { name: 'name', index: 'name', width: 180 },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_contactgroup'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_contactgroup").width() - 10,
        height: $("#lookup_div_contactgroup").height() - 110,
    });
    $("#lookup_table_contactgroup").jqGrid('navGrid', '#lookup_toolbar_contactgroup', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_contactgroup').click(function () {
        $('#lookup_div_contactgroup').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_contactgroup').click(function () {
        var id = jQuery("#lookup_table_contactgroup").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_contactgroup").jqGrid('getRowData', id);

            $('#ContactGroupId').val(ret.id).data("kode", id);
            $('#ContactGroup').val(ret.name);

            $('#lookup_div_contactgroup').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // ---------------------------------------------End Lookup contactgroup----------------------------------------------------------------


}); //END DOCUMENT READY