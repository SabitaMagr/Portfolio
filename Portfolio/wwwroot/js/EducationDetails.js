$(document).ready(function () {
    // Initialize DataTable
    new DataTable('#EducationDtl', {
        ajax: {
            url: '/Home/GetEducationDtl',
            type: 'GET',
            dataSrc: 'data',
        },
        scrollY: '280px',
        deferRender: true,
        scroller: true,
        columns: [
            { data: 'id' },
            { data: 'institution' },
            { data: 'location' },
            { data: 'degree' },
            { data: 'grade' },
            { data: 'fieldStudy' },
            { data: 'specialization' },
            { data: 'startDt' },
            { data: 'endDt' },
            { data: 'summary' },
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
                url: `/Home/DeleteEducationData/${id}`,
                type: 'POST',
                success: function (response) {
                    if (response) {
                        $('#EducationDtl').DataTable().ajax.reload();

                    } else {
                        console.error('Failed to fetch skill data.');
                    }
                }
            });
        }
    });
    $(document).on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        window.location.href = `/Home/EducationDetail?id=${id}`;
    });
});
