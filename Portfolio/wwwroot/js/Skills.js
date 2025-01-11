$(document).ready(function () {
    // Initialize DataTable
    $('#skillTbl').DataTable();

    // Add new input field
    $(document).on('click', '.add-field', function () {
        const newField = `
            <div class="input-group mb-3">
                <input type="text" class="form-control" placeholder="Enter Skill" name="skills[]" />
                <button class="btn btn-success add-field" type="button"><i class="fa-solid fa-plus"></i></button>
                <button class="btn btn-danger remove-field" type="button"><i class="fa-solid fa-trash"></i></button>
            </div>
        `;
        $('#inputFieldsContainer').append(newField); // Append the new input field
    });

    // Remove an input field
    $(document).on('click', '.remove-field', function () {
        $(this).closest('.input-group').remove(); // Remove the closest input group
    });
});
