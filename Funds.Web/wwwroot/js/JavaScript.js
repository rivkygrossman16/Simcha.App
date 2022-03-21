$(() => {
    $('#new-contributor').on('click', function () {
        $('#add-name-modal').modal();
    });
    $('.deposit-button').on('click', function () {
        $('#deposit-modal').modal();
    });
    
    $('.deposit-button').on('click', function () {
        //element = document.getElementById('#deposit-name');
        
        const button = $(this);
        const firstName = button.data('first');
      /*  $('#contributor_edit_first_name').val(firstName);*/
        const lastName = button.data('last');
       /* $('#contributor_edit_last_name').val(lastName);*/
        //const id = button.data('contribid');
        //const firstName = button.data('first');
        //const lastName = button.data('last');
        $('#deposit-name').append(`${firstName} ${lastName}`)
        const id = button.data('contribid');
        $('#deposit_id').val(id);
    
    });
    $('#my-table').on('click',' #edit-btn', function () {
        $('#edit-name-modal').modal();
        console.log('hey');
        const button = $(this);
        const firstName = button.data('first-name');
        $('#contributor_edit_first_name').val(firstName);
        const lastName = button.data('last-name');
        $('#contributor_edit_last_name').val(lastName);
        const cell = button.data('cell');
        $('#contributor_edit_cell_number').val(cell);
        const alwaysInclude = button.data('always-include');
        $('#contributor_edit_always_include').val(alwaysInclude);
        const date = button.data('date');
        $('#contributor_edit_created_at').val(date);
        const id = button.data('id');
        $('#id').val(id);
        
    });
    //getter
    //$('.deposit-button').on('click', function () {
    //    console.log(tr.data('contribid'));
    //});
    //tr.data('person-id', 100); //setter

});