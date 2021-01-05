var gridOptions = {
    rowData: null, 
    gridAutoHeight: true,
    enableSorting: true,
    enableFilter: true,
    pagination: true,
    enableColResize: true
};

$(function () {

    $("#btnExportar").click(function () {
        gridOptions.api.exportDataAsExcel();
    });

    $("#selectOrigen").change(function () {
        $("#myGrid").html("");

        let id = $(this).val();

        agGrid.LicenseManager.setLicenseKey("Evaluation_License_Valid_Until__25_August_2018__MTUzNTE1MTYwMDAwMA==53aedc27a9df43c4e4bd6c97d9e6f413");
        var gridDiv = document.querySelector('#myGrid');
        var grid = new agGrid.Grid(gridDiv, gridOptions);

        $.ajax({
            type: "GET",
            traditional: true,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            url: urlDatos,
            data: { id: id },
            success: function (result) {
                if (result) {

                    gridOptions.api.setColumnDefs(result.columnas);
                    gridOptions.api.setRowData(result.datos);
                    gridOptions.api.redrawRows();

                    $("#btnExportar").show();
                }
            },
            error: function () {
                $("#btnExportar").hide();
            }
        });

    });
});