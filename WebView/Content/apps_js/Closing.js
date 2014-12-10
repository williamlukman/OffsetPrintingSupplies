$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Closing/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        //$('#Description').val('').text('').removeClass('errormessage');
       // $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#close_confirm_div").dialog('close');
    $("#open_confirm_div").dialog('close');
    $("#search_div").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Closing/GetList',
        datatype: "json",
        colNames: ['ID', 'Period', 'Year', 'Beginning', 'End Date','Is Year', 'Status', 'Closing'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center", hidden: true },
				  { name: 'period', index: 'period', width: 60 },
				  { name: 'year', index: 'yearperiod', width: 60 },
                  { name: 'beginning', index: 'beginningperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'enddate', index: 'enddateperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
				  { name: 'isyear', index: 'isyear', width: 60 },
                  { name: 'isclosed', index: 'isclosed', width: 80 },
                  { name: 'closing', index: 'closing', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } }
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
		          rowIsBank = $(this).getRowData(cl).isyear;
		          if (rowIsBank == 'true') {
		              rowIsBank = "YES";
		          } else {
		              rowIsBank = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isyear: rowIsBank });
		          rowIsBank = $(this).getRowData(cl).isclosed;
		          if (rowIsBank == 'true') {
		              rowIsBank = "Close";
		          } else {
		              rowIsBank = "Open";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isclosed: rowIsBank });
		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
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
        $('#BaginningPeriodDiv').show();
        $('#BaginningPeriodDiv2').hide();
        $('#EndDatePeriodDiv').show();
        $('#EndDatePeriodDiv2').hide();
        $('#Period').removeAttr('disabled');
        $('#YearPeriod').removeAttr('disabled');
        $('#IsYear').removeAttr('disabled');
        $('#form_btn_save').show();

        $.ajax({
            dataType: "json",
            url: base_url + "Currency/GetListNonBase?",
            success: function (result) {
                if (result.query != null) {
                    createContainerTable();
                    var tbody = $('#list_containerSE');
                    for (var i = 1; i <= result.query.length; i++) {
                        var trow = $("<tr>").addClass("tableRow").addClass('ui-widget-content');
                        $("<td>").addClass("tableCell").text(result.query[i-1].Id).appendTo(trow);
                        $("<td>").addClass("tableCell").text(result.query[i - 1].Name).appendTo(trow);
                        $("<td>").addClass("tableCell")
                            .append('<input id="TotalAmount" name="TotalAmount" type="text" size="15" maxlength="20" class="textright easyui-numberbox" data-options="groupSeparator:\',\'" value="0""/>')
                            .appendTo(trow);
                        trow.appendTo(tbody);
                        $('#list_containerSE tr:last td:eq(2)').html('<input id="TotalAmount" name="TotalAmount" type="text" size="15" maxlength="20" class="textright easyui-numberbox" data-options="groupSeparator:\',\'" value="0""/>');
                        $('#list_containerSE tr:last td:eq(2)').find('#TotalAmount').numberbox();
                    }
                }
            }
        });
        $('#form_div').dialog('open');
    });

    $('#btn_view').click(function () {
        ClearData();
        clearForm("#frm");
        $('#form_btn_save').hide();
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "Closing/GetInfo?Id=" + id,
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
                            $('#Id').val(result.Id);
                            $('#Period').val(result.Period);
                            $('#YearPeriod').val(result.YearPeriod);
                            $('#BeginningPeriod2').val(dateEnt(result.BeginningPeriod));
                            $('#EndDatePeriod2').val(dateEnt(result.EndDatePeriod));
                            $('#BaginningPeriodDiv').hide();
                            $('#BaginningPeriodDiv2').show();
                            $('#EndDatePeriodDiv').hide();
                            $('#EndDatePeriodDiv2').show();
                            var f = document.getElementById("IsYear");
                            if (result.IsYear == true) {
                                f.selectedIndex = 1;
                            }
                            else {
                                f.selectedIndex = 0;
                            }
                            $('#Period').attr('disabled', true);
                            $('#YearPeriod').attr('disabled', true);
                            $('#IsYear').attr('disabled', true);
                            $('#form_div').dialog('open');
                            createContainerTable();
                            var tbody = $('#list_containerSE');
                            for (var i = 1; i <= result.exchangeRateClosings.length; i++) {
                                var trow = $("<tr>").addClass("tableRow").addClass('ui-widget-content');
                                $("<td>").addClass("tableCell").text(result.exchangeRateClosings[i - 1].CurrencyId).appendTo(trow);
                                $("<td>").addClass("tableCell").text(result.exchangeRateClosings[i - 1].Name).appendTo(trow);
                                $("<td>").addClass("tableCell")
                                .append('<input id="TotalAmount" name="TotalAmount" type="text" size="15" maxlength="20" class="textright easyui-numberbox" data-options="groupSeparator:\',\'" value=' + result.exchangeRateClosings[i - 1].Rate+ ' disabled="disabled""/>')
                                .appendTo(trow);
                                $('#list_containerSE tr:last td:eq(2)').html('<input id="TotalAmount" name="TotalAmount" type="text" size="15" maxlength="20" class="textright easyui-numberbox" data-options="groupSeparator:\',\'" value=' + result.exchangeRateClosings[i - 1].Rate + ' disabled="disabled""/>');
                                $('#list_containerSE tr:last td:eq(2)').find('#TotalAmount').numberbox();
                                trow.appendTo(tbody);
                            }
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    function createContainerTable() {
        var tbody = $('#list_containerSE');
        if (tbody == null || tbody.length < 1) return;
        // Clear 
        $("#list_containerSE tr.tableRow").each(function () {
            $(this).remove();
        });

        var trow = $("<tr>").addClass("tableRow").addClass('ui-jqgrid-labels ui-widget-content');
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Id').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Currency').appendTo(trow);
        $("<th>").addClass("tableCell").addClass("ui-state-default ui-th-column ui-th-ltr").text('Rate To IDR').appendTo(trow);
        trow.appendTo(tbody);
    }

    $('#btn_close').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ClosingDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idclose').val(ret.id);
            $("#close_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_open').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#open_confirm_btn_submit').data('Id', ret.id);
            $("#open_confirm_div").dialog("open");
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

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');
        var exchangerateclosingContainer = [];
        var exchangerateclosingContainerIdx = 0;
        $('#list_exchangerateclosing').each(function () {
            if (exchangerateclosingContainerIdx > 0) {
                obj = {};
                obj['CurrencyId'] = $.trim($(this).find('td:eq(0)').text());
                obj['Rate'] = $.trim($(this).find('#TotalAmount').numberbox('getValue'));
                exchangerateclosingContainer.push(obj);
            }
            exchangerateclosingContainerIdx++;
        });
        var f = document.getElementById("IsYear");
        var isyear = f.options[f.selectedIndex].value;
        submitURL = base_url + 'Closing/Insert';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Period: $("#Period").val(), YearPeriod: $("#YearPeriod").val(),
                BeginningPeriod: $('#BeginningPeriod').datebox('getValue'), IsYear: isyear,
                EndDatePeriod: $('#EndDatePeriod').datebox('getValue'), exchangeRateClosing: exchangerateclosingContainer
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

    $('#close_confirm_btn_cancel').click(function () {
        $('#close_confirm_btn_submit').val('');
        $("#close_confirm_div").dialog('close');
    });

    $("#close_confirm_btn_submit").click(function () {
        ClearErrorMessage();

        var submitURL = '';
        var id = $("#idclose").val();

        submitURL = base_url + 'Closing/Close';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ClosedAt: $('#ClosedAt').datebox('getValue')
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
                    $("#close_confirm_div").dialog('close')
                }
            }
        });
    });

    $('#open_confirm_btn_cancel').click(function () {
        $('#open_confirm_btn_submit').val('');
        $("#open_confirm_div").dialog('close');
    });

    $("#open_confirm_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#open_confirm_btn_submit").data('Id');

        submitURL = base_url + 'Closing/Open';

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id
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
                    $("#open_confirm_div").dialog('close')
                }
            }
        });
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "Closing/Delete",
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
                }
                else {
                    ReloadGrid();
                    $("#delete_confirm_div").dialog('close');
                }
            }
        });
    });

    function clearForm(form) {

        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea') {
                this.value = "";
            }
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
            if ($(this).hasClass('easyui-numberbox'))
                $(this).numberbox('clear');

        });
    }
}); //END DOCUMENT READY