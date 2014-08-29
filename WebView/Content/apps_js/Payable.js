$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Payable/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'Payable/GetListByDate', postData: { filters: null, startdate: StartDate, enddate: EndDate }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Payable/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Contact Id', 'Contact', 'Group',
                   'Source', 'Id', 'Amount', 'Remaining', 'PendingClearance',
                   'Due Date', 'Completion Date', 'Created At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center", hidden: true },
				  { name: 'code', index: 'code', width: 70 },
                  { name: 'contactid', index: 'contactid', width: 40, hidden: true},
                  { name: 'contact', index: 'contact', width: 150 },
                  { name: 'group', index: 'group', width: 150 },
				  { name: 'source', index: 'source', width: 120, align: 'right' },
				  { name: 'sourceid', index: 'sourceid', width: 55 },
                  { name: 'amount', index: 'amount', width: 80, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'remainingamount', index: 'remainingamount', width: 80, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', width: 100, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'duedate', index: 'duedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'completiondate', index: 'completiondate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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

    $('#btn_search').click(function () {
        $('#StartDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EndDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $("#search_div").dialog("open");
    });

    $('#search_btn_submit').click(function () {
        ClearErrorMessage();
        ReloadGridByDate($('#StartDate').datebox('getValue'), $('#EndDate').datebox('getValue'));
        $("#search_div").dialog('close');
    });

    $('#search_btn_cancel').click(function () {
        $('#search_div').dialog('close');
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
}); //END DOCUMENT READY