$(document).ready(function () {
    // Initialize DataTable
    new DataTable('#skillTbl', {
        ajax: {
            url: '/Home/GetSkills',
            type: 'GET',
            dataSrc: 'data', // Specify the array location in the response
        },
        columns: [
            { data: 'id' },
            { data: 'skill' },
            { data: 'created_by' },
            { data: 'created_dt' },
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

    // Add new input field
    $(document).on('click', '.add-field', function () {
        const newField = `
            <div class="input-group mb-3">
                <input type="text" class="form-control" placeholder="Enter Skill" name="skills[]" />
                <button class="btn btn-success add-field" type="button"><i class="fa-solid fa-plus"></i></button>
                <button class="btn btn-danger remove-field" type="button"><i class="fa-solid fa-trash"></i></button>
            </div>
        `;
        $('#inputFieldsContainer').append(newField);
    });

    // Remove an input field
    $(document).on('click', '.remove-field', function () {
        $(this).closest('.input-group').remove();
    });

    $(document).on('click', '.delete-btn', function () {
        constid = $(this).data('id');
        if (confirm('Are you sure you wantto delete?')) {
            $.ajax({
                url: `/Home/DeleteSkill/${id}`,
                type: 'DELETE',
                success: function () {
                        $('#skillTbl').DataTable().ajax.reload();
                },
                error: function () {
                    console.error('Error occurred while deleting the skill.');
                },
            });
        }
    });
    $(document).on('click', '.edit-btn', function () {
        const id = $(this).data('id'); 
        $.ajax({
            url: `/Home/GetSkillById/${id}`, 
            type: 'GET',
            success: function (response) {
                if (response) {
                    $('#editSkillId').val(response.id); 
                    $('#editSkillName').val(response.skill); // Skill name field

                    $('#skillModal').modal('show');
                } else {
                    console.error('No data found for the provided ID.');
                }
            },
            error: function () {
                console.error('An error occurred while fetching the skill details.');
            },
        });
    });
    $(document).on('click', '.edit-btn', function () {
        const id = $(this).data('id'); 
        $.ajax({
            url: `/Home/GetSkillById/${id}`, 
            type: 'GET',
            success: function (response) {
                if (response) {
                    $('#inputFieldsContainer').hide();
                    $('#skillId').val(response.id); 
                    $('#skillName').val(response.skill); 
                    $('#skillModal').modal('show');
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
