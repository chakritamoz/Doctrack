$(document).on('click', '#clear-emp-btn', () => {
  modal.classList.toggle('display');
});

$(document).on('click', '#modal-accept-button', function() {
  modal.classList.toggle('display');
  $.ajax({
    url: 'Employees/ClearEmployees',
    type: 'POST',
    success: function(data) {
      $('#document-table').html(data);
    }
  })
});
