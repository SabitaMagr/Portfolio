$(document).ready(function () {
    // Initialize DataTable
    new DataTable('#PersonalDtl', {
        ajax: {
            url: '/Home/GetPersonalDtl',
            type: 'GET',
            dataSrc: 'data', // Specify the array location in the response
        },
        columns: [
            { data: 'UserId' },
            { data: 'FullName' },
            { data: 'MobileNo' },
            { data: 'Email' },
            { data: 'Profile' },
            { data: 'About' },
            { data: 'Summary' },
            {
                data: 'UserId',
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
                url: `/Home/DeletePersonalData/${id}`,
                type: 'DELETE',
                success: function (response) {
                    if (response) {
                        $('#PersonalDtl').DataTable().ajax.reload();

                    } else {
                        console.error('Failed to fetch skill data.');
                    }
                }
            });
        }
    });
    $(document).on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.ajax({
            url: `/Home/GetPersonalDtById/${id}`,
            type: 'GET',
            success: function (response) {
                if (response) {
                    window.location.href = `/Home/PersonalDetails?id=${response.id}`;
                } else {
                    console.error('Failed to fetch skill data.');
                }
            },
            error: function () {
                console.error('An error occurred while fetching the skill data.');
            },
        });
    });
});
