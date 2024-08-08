var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    debugger;
    dataTable = $('#tblData').DataTable({
        "ajax": { url: "/order/getall" },
        "columns": [
            { data: 'orderHeaderid', "width": "5%" },
            { data: 'email', "width": "25%" }
        ]
    })
}