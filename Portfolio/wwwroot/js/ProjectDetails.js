$(document).ready(function () {
    // Initialize DataTable
    new DataTable('#ProjectDtl', {
        ajax: {
            url: '/Home/GetProjectDtl',
            type: 'GET',
            dataSrc: 'data',
        },
        scrollY: '280px',
        deferRender: true,
        scroller: true,
        columns: [
            { data: 'id' },
            { data: 'name' },
            { data: 'description' },
            { data: 'gitLink' },
            { data: 'deployLink' },
            {
                data: 'imageUrl',
                render: function (data, type, row) {
                    if (!data) {
                        return '';
                    }
                    return `
                        <a href="${data}" target="_blank">
                            <img src="${data}" alt="Project Image" style="width: 80px; height: 28px; object-fit: cover;">
                        </a>
                    `;
                },
            },
            {
                data: 'id',
                title: 'Action',
                render: function (data, type, row) {
                    return `
                    <i class="fas fa-edit fa-lg edit-btn" data-id="${data}" style="color: orange; cursor: pointer;"></i>
                    <i class="fas fa-trash fa-lg delete-btn" data-id="${data}" style="color: red; cursor: pointer; margin-left: 10px;"></i>
                    `;
                },
            },
        ],
    });
    $(document).on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        if (confirm('Are you sure you want to delete?')) {
            $.ajax({
                url: `/Home/DeleteProjectData/${id}`,
                type: 'POST',
                success: function (response) {
                    if (response) {
                        $('#ProjectDtl').DataTable().ajax.reload();

                    } else {
                        console.error('Failed to fetch skill data.');
                    }
                }
            });
        }
    });
    $(document).on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        window.location.href = `/Home/ProjectDetail?id=${id}`;
    });
});
