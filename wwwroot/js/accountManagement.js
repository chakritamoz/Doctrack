$(document).on('click', '.action-approval', function() {
  const id = $(this).attr("data-id");
  modalAcceptBtn.id = 'modal-approval-button';
  $('#modal-approval-button').attr('data-id', id);
  modal.classList.toggle("display");
  const modalBody = $('<form id="modal-form-approve" autocomplete="off"></form>');
  $('#modal-title').html('<div>Confirm approve account<div>');
  $('#modal-body').html(modalBody);
  $('#modal-form-approve').html('<p>Are you sure you want to approve this Account?</p>');
  $('#modal-form-approve').append('<p>User ID: <span style="color:red;display:inline">' + id + '</span></p>');
}); // end click approve-account

$(document).on('click', '#modal-approval-button', function() {
  const id = $(this).attr('data-id');
  $.ajax({
    url: 'Approval/' + id,
    type: 'POST',
    success: function(response) {
      window.location.href = '/Accounts/Management';
    },
    error: function(xhr, status, error) {
      console.log(error);
    }
  });
});

$(document).on('click', '.action-delete', function() {
  const id = $(this).attr("data-id");
  modalAcceptBtn.id = 'modal-delete-button';
  $('#modal-delete-button').attr('data-id', id);
  modal.classList.toggle("display");
  const modalBody = $('<form id="modal-form-del" autocomplete="off"></form>');
  $('#modal-title').html('<div>Confirm delete account<div>');
  $('#modal-body').html(modalBody);
  $('#modal-form-del').html('<p>Are you sure you want to delete this Account?</p>');
  $('#modal-form-del').append('<p>User ID: <span style="color:red;display:inline">' + id + '</span></p>');
}); // end click del-account

$(document).on('click', '#modal-delete-button', function() {
  const id = $(this).attr('data-id');
  $.ajax({
    url: 'Delete/' + id,
    type: 'POST',
    success: function(response) {
      window.location.href = '/Accounts/Management';
    },
    error: function(xhr, status, error) {
      console.log(error);
    }
  });
});