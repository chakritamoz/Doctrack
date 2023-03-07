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
var activeDocId;

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

//when click delete icon toggle modal
//set modal title and modal body
//set modal accept btn id
$(document).on('click', '#del-doc-icon', () => {
  modalAcceptBtn.id = 'modal-delete-button';
  modal.classList.toggle("display");
  $('#modal-title').html('<div>Confirm delete document<div>');
  $('#modal-body').html('<p>Are you sure you want to delete document?</p>');
  $('#modal-body').append('<p>Document ID: <span style="color:red">' + currentDocId + '</span></p>');
});

//when click add employee icon toggle modal
//set modal title and modal body
//set modal accept btn id
$(document).on('click', '#upop-doc-icon', () => {
  modalAcceptBtn.id = 'modal-upop-button';
  modal.classList.toggle("display");
  $.ajax({
    url: 'Documents/UpdateOP/',
    type: 'GET',
    data: { 'id': currentDocId},
    success: function(response) {
      $('#modal-title').html('<div>Add employee to document<div>');
      $('#modal-body').html('<label for="oplocate">Operation Location</label><br />');
      $('#modal-body').append('<input id="oplocate"/><br />')
      $('#modal-body').append('<span id="oplocateError" class="text-danger"></span>')
      $('#modal-body').append('<label for="opdate">Operation Date</label><br />');
      $('#modal-body').append('<input id="opdate" /><br />')
      $('#modal-body').append('<span id="opdateError" class="text-danger"></span>')

      $('#oplocate').val(response.operation);
      var date = new Date(response.operationDate);
      var day = date.getDate().toString().padStart(2, '0');
      var month = (date.getMonth()+1).toString().padStart(2, '0');
      var year = date.getFullYear();
      var myDate = day + "/" + month + "/" + year
      $('#opdate').val(myDate);
    }
  })
});

$(document).on('click', '#modal-upop-button', () => {
  if(validateOPForm()){
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
          location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
});

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
});

function validateOPForm() {
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

    if ($(this).attr('id') == 'opdate') {
      var dateValue = $(this).val();
      console.log(dateValue);
      console.log(datePattern.test(dateValue));
      if (!datePattern.test(dateValue)) {
        $('#opdateError').html('Format much be dd/mm/yyyy.<br />');
        isValid = false;
      }else {
        $('#opdateError').html('');
      }
    }
  });

  return isValid;
}