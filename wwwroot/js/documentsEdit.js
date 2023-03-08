// when click submit icon
// toggle modal
$(document).on('click', '#newId', () => {
  if ($('#newId').attr('readonly')) {
    modal.classList.toggle('display');
    $('#modal-title').html('<div>Confirm edit Document No.?</div>');
    $('#modal-body').html('<p>Are you sure, you want to edit Document No.</p>');
  }
}); // end click sub-doc-icon

$(document).on('click', '#modal-accept-button', () => {
  modal.classList.toggle('display');
  $('#newId').attr('readonly', false);
})