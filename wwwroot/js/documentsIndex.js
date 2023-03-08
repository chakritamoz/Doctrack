const addEmpIcon = document.getElementById("add-emp-icon");
const editDocIcon = document.getElementById("edit-doc-icon");
const delDocIcon = document.getElementById("del-doc-icon");
const upOpDocIcon = document.getElementById("upop-doc-icon");
const subDocIcon = document.getElementById("sub-doc-icon");
const floatingBtn = document.getElementById("fabCheckbox");

const modal = document.getElementById("writableModal");
const modalBody = document.getElementById("modal-body");
const modalBodyDet = document.getElementById("modal-body-detail");
const modalCloseBtn = document.getElementById("modal-close-button");
const modalAcceptBtn = document.getElementById("modal-accept-button");
const spanClose = document.getElementsByClassName("close")[0];
var currentDocId;

function displayTable(docId) {
  currentDocId = docId;
  const target = document.getElementById(currentDocId);
  const subTarget = document.getElementById("sub-"+currentDocId);

  if (target.classList.contains('active')) {
    disableActive();
    floatingBtn.setAttribute('disabled', true);
    floatingBtn.checked = false;
  }else if (subTarget == null) {
    disableActive();
    floatingBtn.removeAttribute('disabled');
    setAttrId(currentDocId);
  }else {
    disableActive();
    const nextTarget = subTarget.nextElementSibling;
    target.classList.toggle('active');
    subTarget.classList.toggle('expand');
    if (nextTarget != null) nextTarget.classList.toggle('row-footer');
    floatingBtn.removeAttribute('disabled');
    setAttrId(currentDocId);
  }
}

function disableActive() {
  const disableElements = document.querySelectorAll('.active, .expand, .row-footer');
  disableElements.forEach(element => {
    element.classList.remove('active', 'expand', 'row-footer');
  });
}

function setAttrId(docId) {
  addEmpIcon.setAttribute('data-id',docId);
  editDocIcon.setAttribute('data-id',docId);
  delDocIcon.setAttribute('data-id',docId);
  upOpDocIcon.setAttribute('data-id',docId);
  subDocIcon.setAttribute('data-id',docId);
}

spanClose.onclick = function() {
  modal.classList.toggle("display");
}

modalCloseBtn.onclick = function() {
  modal.classList.toggle("display");
}

window.onclick = function(event) {
  if (event.target == modal)
  {
    modal.classList.toggle("display");
  }
}

// when click delete icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '#del-doc-icon', () => {
  modalAcceptBtn.id = 'modal-delete-button';
  modal.classList.toggle("display");
  $('#modal-title').html('<div>Confirm delete document<div>');
  $('#modal-body').html('<p>Are you sure you want to delete document?</p>');
  $('#modal-body').append('<p>Document ID: <span style="color:red">' + currentDocId + '</span></p>');
}); // end click del-doc-icon

// when click update operation icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '#upop-doc-icon', () => {
  modalAcceptBtn.id = 'modal-upop-button';
  modal.classList.toggle('display');
  $.ajax({
    url: 'Documents/UpdateOP/',
    type: 'GET',
    data: { 'id': currentDocId},
    success: function(result) {
      $('#modal-title').html('<div>Are you sure, you want to update operation?<div>');
      $('#modal-body').html('<label for="oplocate">Operation Location</label><br />');
      $('#modal-body').append('<input id="oplocate"/><br />')
      $('#modal-body').append('<span class="text-danger"></span>')
      $('#modal-body').append('<label for="opdate">Operation Date</label><br />');
      $('#modal-body').append('<input id="opdate" /><br />')
      $('#modal-body').append('<span class="text-danger dateError"></span>')
      
      $('#oplocate').val(result.operation);
      var date = result.operationDate != null
        ?new Date(result.operationDate)
        :new Date(Date.now());
      var day = date.getDate().toString().padStart(2, '0');
      var month = (date.getMonth()+1).toString().padStart(2, '0');
      var year = date.getFullYear();
      var myDate = day + "/" + month + "/" + year
      $('#opdate').val(myDate);
    }
  })
}); // end click upop-doc-icon

// when click edit icon
// redirect to documents/edit/?id=5
$('#edit-doc-icon').on('click', function(){
  var id = $(this).attr('data-id');
  window.location.href = '/Documents/Edit/' + id;
}); // end click edit-doc-icon

// when click submit icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '#sub-doc-icon', () => {
  modalAcceptBtn.id = 'modal-sub-button';
  modal.classList.toggle('display');
  $.ajax({
    url: 'Documents/UpdateOP/',
    type: 'GET',
    data: { 'id': currentDocId },
    success: function(result) {
      $('#modal-title').html('<div>Are you sure, you want to update end date?</div>');
      $('#modal-body').html('<label for="endDoc">End date</label><br />');
      $('#modal-body').append('<input id="endDoc"/><br />');
      $('#modal-body').append('<span class="text-danger dateError"></span>');
    
      var date = result.endDate != null
        ?new Date(result.endDate)
        :new Date(Date.now());
      var day = date.getDate().toString().padStart(2, '0');
      var month = (date.getMonth()+1).toString().padStart(2, '0');
      var year = date.getFullYear();
      var myDate = day + "/" + month + "/" + year
      $('#endDoc').val(myDate);
    }
  })
}); // end click sub-doc-icon

// when click add employee icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '#add-emp-icon', () => {
  modalAcceptBtn.id = 'modal-addEmp-button';
  modal.classList.toggle('display');
  $('#modal-title').html('<div>Add employee to document</div>');
  $('#modal-body').html('<label for="jobTitle">Job</label><br />');
  $('#modal-body').append('<input id="jobTitle"/><br />');
  $('#modal-body').append('<span class="text-danger"></span>');
  $('#modal-body').append('<label for="rankTitle">Title</label><br />');
  $('#modal-body').append('<input id="rankTitle"/><br />');
  $('#modal-body').append('<span class="text-danger"></span>');
  $('#modal-body').append('<label for="firstName">First Name</label><br />');
  $('#modal-body').append('<input id="firstName"/><br />');
  $('#modal-body').append('<span class="text-danger"></span>');
  $('#modal-body').append('<label for="lastName">Last Name</label><br />');
  $('#modal-body').append('<input id="lastName"/><br />');
  $('#modal-body').append('<span class="text-danger"></span>');
  $('#modal-body').append('<label for="remark">Remark</label><br />');
  $('#modal-body').append('<input id="remark"/><br />');
  $('#modal-body').append('<span class="text-danger"></span>');
}); // end click sub-doc-icon


/*----------- Click confirm button on Modal -----------*/
// when click confirm update op button on modal
// send method post to update data
$(document).on('click', '#modal-upop-button', () => {
  if(validateForm()){
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: 'Documents/UpdateOP/',
      type: 'POST',
      headers: { 'RequsetVerificationToken': token },
      data: {
        'id': currentDocId,
        'operation': $('#oplocate').val(),
        'operationDate': $('#opdate').val(),
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          // location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
}); // end click modal-upop-button

// when click confirm submit button on modal
// send method post to update data
$(document).on('click', '#modal-sub-button', () => {
  if(validateForm()){
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: 'Documents/UpdateEndDate/',
      type: 'POST',
      headers: { 'RequsetVerificationToken': token },
      data: {
        'id': currentDocId,
        'endDate': $('#endDoc').val(),
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
}); // end click modal-upop-button


// when click confirm button on modal
// send method post to update data
$(document).on('click', '#modal-delete-button', () => {
  var token = $('input[name="__RequestVerificationToken"]').val();
  $.ajax({
    url: '/Documents/Delete/',
    type: 'POST',
    headers: { 'RequestVerificationToken': token },
    data: { 
      'id': currentDocId, 
      '__RequestVerificationToken': token 
    },
    success: function(result) {
      if (result.success) {
        location.reload();
      } else {
        alert('An error occurred while deleting the document.');
      }
    }
  });
}); // end click #modal-delete-button


// validate Operation edit form
function validateForm() {
  var datePattern = /^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|[1][012])[\/]\d{4}$/;
  var isValid = true;

  $('#modal-body > input').each(function() {
    var spanElement = $(this).next().next();

    if ($(this).val() == '') {
      if (spanElement.length && spanElement.prop('tagName').toLowerCase() === 'span') {
        spanElement.html('Please enter data.<br />');
      }
      isValid = false;
    }else {
      spanElement.html('');
    }

    if ($(this).attr('id') == 'opdate' || $(this).attr('id') == 'endDoc') {
      var dateValue = $(this).val();
      if (!datePattern.test(dateValue)) {
        $('.dateError').html('Format much be dd/mm/yyyy.<br />');
        isValid = false;
      }else {
        $('.dateError').html('');
      }
    }
  });

  return isValid;
} // end validate op function