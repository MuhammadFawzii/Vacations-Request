var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/product/getall', // Keep original endpoint
            dataSrc: 'data'
        },
        "columns": [
            {
                data: null,
                "render": function (data, type, row, meta) {
                    return meta.row + 1; // Auto-increment row number
                },
                "width": "5%"
            },
            {
                data: 'submissionDate',
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                },
                "width": "12%"
            },
            { data: 'employeeName', "width": "15%" },
            { data: 'departmentName', "width": "12%" },
            {
                data: 'vacationDateFrom',
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                },
                "width": "10%"
            },
            {
                data: 'vacationDateTo',
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                },
                "width": "10%"
            },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                 <a href="/admin/product/details?id=${data}" class="btn btn-info btn-sm mx-1"> 
                                    <i class="bi bi-eye"></i> View
                                 </a>
                                 <a href="/admin/product/edit?id=${data}" class="btn btn-primary btn-sm mx-1"> 
                                    <i class="bi bi-pencil-square"></i> Edit
                                 </a>  
                                 <a onClick="Delete('/admin/product/delete?id=${data}')" class="btn btn-danger btn-sm mx-1"> 
                                    <i class="bi bi-trash-fill"></i> Delete
                                 </a>
                             </div>`;
                },
                "width": "26%",
                "orderable": false
            }
        ],
        "order": [[1, 'desc']], // Order by submission date descending
        "pageLength": 10,
        "responsive": true,
        "language": {
            "emptyTable": "No vacation requests found"
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error('Error occurred while deleting the request');
                }
            });
        }
    });
}