$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'GeneralLedger/GetList', postData: { filters: null }, page: '1' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'GeneralLedger/GetListByDate', postData: { startdate: StartDate, enddate: EndDate } }).trigger("reloadGrid");
    }

    function ReloadGridByAccountAndDate(AccountId, StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'GeneralLedger/GetListByAccountAndDate', postData: { accountid: AccountId, startdate: StartDate, enddate: EndDate } }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#search_div").dialog('close');
    $("#AccountId").hide();
    $("#lookup_div_coa").dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'GeneralLedger/GetList',
        datatype: "json",
        colNames: ['ID', 'Transaction Date', 'Status', 'Account Code', 'Account Name',
                   'Debit', 'Credit', 'Source Document', 'Id', 'Nomor Surat', ],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
				  { name: 'transactiondate', index: 'transactiondate', search: false, width: 120, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'status', index: 'status', width: 70, stype: 'select', editoptions: { value: ':;1:Debit;2:Credit' } },
				  { name: 'accountcode', index: 'accountcode', width: 120 },
				  { name: 'account', index: 'account', width: 250 },
                  { name: 'debitamount', index: 'debitamount', width: 120, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'creditamount', index: 'creditamount', width: 120, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'sourcedocument', index: 'sourcedocument', width: 120 },
                  { name: 'sourcedocumentid', index: 'sourcedocumentid', width: 70, align: 'right', formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'nomorsurat', index: 'nomorsurat', width: 120 },
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
		          rowStatus = $(this).getRowData(cl).status;
		          if (rowStatus == 1) {
		              rowStatus = "Debit";
		          } else {
		              rowStatus = "Credit";
		          }
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });
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

    $('#btn_search').click(function () {
        $('#StartDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EndDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $("#search_div").dialog("open");
    });

    $('#search_btn_submit').click(function () {
        ClearErrorMessage();
        ReloadGridByAccountAndDate($('#AccountId').val(), $('#StartDate').datebox('getValue'), $('#EndDate').datebox('getValue'));
        $("#search_div").dialog('close');
    });

    $('#search_btn_cancel').click(function () {
        $('#search_div').dialog('close');
    });

    //GRID LOOKUP +++++++++++++++
    $("#lookup_table_coa").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        colNames: ['Id', 'Account Code', 'Account Code', 'Account Name'],
        colModel: [
                  { name: 'Id', index: 'Id', width: 40, hidden: true },
				  { name: 'Code', index: 'Code', width: 80, hidden: true },
                  { name: 'parsecode', index: 'parsecode', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: "", defaultValue: '0' } },
				  { name: 'Name', index: 'Name', width: 150 },
        ],
        page: '1',
        pager: $('#lookup_pager_coa'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        sortorder: "ASC",
        width: $("#lookup_div_coa").width() - 10,
        height: $("#lookup_div_coa").height() - 115
    });
    $("#lookup_table_coa").jqGrid('navGrid', '#lookup_toolbar_coa', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });


    $('#btncoa').click(function () {
       $("#lookup_table_coa").setGridParam({ url: base_url + 'ChartOfAccount/GetLeaves' }).trigger("reloadGrid");
       $('#lookup_div_coa').dialog('open');
    });

    $("#lookup_btn_add_coa").click(function () {
        var id = jQuery("#lookup_table_coa").jqGrid('getGridParam', 'selrow');

        if (id) {
            var ret = jQuery("#lookup_table_coa").jqGrid('getRowData', id);
            $('#AccountId').val(ret.Id);
            $('#AccountCode').val(ret.Code).data('kode', id);
            $('#AccountName').val(ret.Name);

            $("#lookup_div_coa").dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $("#lookup_btn_cancel_coa").click(function () {
        $("#lookup_div_coa").dialog('close');
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
}); //END DOCUMENT READY