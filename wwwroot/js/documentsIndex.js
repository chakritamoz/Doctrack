const floatingBtn = document.getElementById("fabCheckbox");
var activeElement;
var expandElement;
var footerElement;
var eleOverflow;

var currentDocId;
var isMove = false;
var triggerReload = false;
var triggerDelEmp = false;
var triggerEditEmp = false;

//Store Search Criteria
var queryDocNo;
var queryDocType;
var queryDocTitle;
var queryEmployee;
var tabType;
var isEndOfData = false
var page = 1;

initialBuddhist();

var docActive = localStorage.getItem('docId');
if (docActive) {
  currentDocId = docActive;
  activeElement = $('#' + docActive.replace('/','\\/').replace('.','\\.')).parent().addClass('active');
  expandElement = $('#sub-' + docActive.replace('/','\\/').replace('.','\\.')).addClass('expand');
  footerElement = $('#sub-' + docActive.replace('/','\\/').replace('.','\\.')).next().addClass('row-footer');
  $('#fabCheckbox').attr('disabled', false);
  $('#fabCheckbox').prop('checked', true);
  $('#' + docActive.replace('/','\\/').replace('.','\\.'))
    .animate({"opacity": ".9"}, 300)
    .animate({"opacity": "1"}, 300)
    .animate({"opacity": ".9"}, 300)
    .animate({"opacity": "1"}, 300);
  localStorage.clear();
}

function disableActive() {
  if (activeElement || expandElement || footerElement){
    activeElement.removeClass('active');
    expandElement.removeClass('expand');
    footerElement.removeClass('row-footer');
  }
}

// when click delete icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '.del-doc-icon', () => {
  cancelTrigger();
  modalAcceptBtn.id = 'modal-delete-button';
  modal.classList.toggle("display");
  const modalBody = $('<form id="modal-form-del" autocomplete="off"></form>');
  $('#modal-title').html('<div>Confirm delete document<div>');
  $('#modal-body').html(modalBody);
  $('#modal-form-del').html('<p>Are you sure you want to delete document?</p>');
  $('#modal-form-del').append('<p>Document ID: <span style="color:red;display:inline">' + currentDocId + '</span></p>');
}); // end click del-doc-icon

// when click update operation icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '.upop-doc-icon', () => {
  cancelTrigger();
  modalAcceptBtn.id = 'modal-upop-button';
  modal.classList.toggle('display');
  const modalBody = $('<form id="modal-form" autocomplete="off"></form>');
  $.ajax({
    url: 'Documents/UpdateOP/',
    type: 'GET',
    data: { 'id': currentDocId},
    success: function(result) {
      $('#modal-title').html('<div>Are you sure, you want to update operation?<div>');
      $('#modal-body').html(modalBody);
      $('#modal-form').append('<label for="oplocate">Operation Location</label><br />');
      $('#modal-form').append('<input id="oplocate"/><br />')
      $('#modal-form').append('<span class="text-danger"></span>')
      $('#modal-form').append('<label for="opdate">Operation Date</label><br />');
      $('#modal-form').append('<input id="opdate" /><br />')
      $('#modal-form').append('<span class="text-danger dateError"></span>')
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
$(document).on('click', '.edit-doc-icon', function(){
  // window.location.href = '/Documents/Edit/' + encodeURIComponent(currentDocId);
  window.location.href = '/Documents/Edit/' + currentDocId.replace("/","_");
}); // end click edit-doc-icon

// when click submit icon
// toggle modal
// set modal title and modal body
// set modal accept btn id
$(document).on('click', '.sub-doc-icon', () => {
  cancelTrigger();  
  modalAcceptBtn.id = 'modal-sub-button';
  modal.classList.toggle('display');
  const modalBody = $('<form id="modal-form" autocomplete="off"></form>');
  $.ajax({
    url: 'Documents/UpdateOP/',
    type: 'GET',
    data: { 'id': currentDocId },
    success: function(result) {
      $('#modal-title').html('<div>Are you sure, you want to update end date?</div>');
      $('#modal-body').html(modalBody);
      $('#modal-form').append('<label for="endDoc">End date</label><br />');
      $('#modal-form').append('<input id="endDoc"/><br />');
      $('#modal-form').append('<span class="text-danger dateError"></span>');
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
  cancelTrigger();
  modalAcceptBtn.id = 'modal-addEmp-button';
  $('#modal-close-button').addClass('modal-closeAddEmp-button');
  $('.close').addClass('modal-closeAddEmp-button');
  modal.classList.toggle('display');
  const modalBody = $('<form id="modal-form" autocomplete="off"></form>');
  $('#modal-title').html('<div>Add employee to document</div>');
  $('#modal-body').html(modalBody);
  setSelectJob();
  setSelectTitle();
  $('#modal-form').append('<label for="firstName">First Name</label><br />');
  $('#modal-form').append('<input id="firstName"/><br />');
  $('#modal-form').append('<span class="text-danger"></span>');
  $('#modal-form').append('<label for="lastName">Last Name</label><br />');
  $('#modal-form').append('<input id="lastName"/><br />');
  $('#modal-form').append('<span class="text-danger"></span>');
  $('#modal-form').append('<label for="remark">Remark</label><br />');
  $('#modal-form').append('<input id="remark"/><br />');
  $('#modal-form').append('<span class="text-danger"></span>');
  $('#modal-form').append('<span id="disp-alert" class="text-success"></span>');
}); // end click sub-doc-icon

$(document).on('keyup', function(e) {
  if (e.keyCode === 27) {
    cancelTrigger();
  }
});

$(document).on('click', '#del-emp-icon', function() {
  if (!triggerDelEmp) {
    cancelTrigger();
    triggerDelEmp = true;
    $('#del-emp-icon').addClass('select');
    $('.sub-row').css('cursor', 'pointer');
  }else {
    cancelTrigger();
  }
});

$(document).on("click", '#edit-emp-icon', function() {
  if (!triggerEditEmp) {
    cancelTrigger();
    triggerEditEmp = true;
    $('#edit-emp-icon').addClass('select');
    $('.sub-row').css('cursor', 'pointer');
  }else {
    cancelTrigger();
  }
});

$('.main-row').click(() => {
  localStorage.clear();
});

$(document).on("click", '.sub-row',function() {
  if (triggerDelEmp) {
    var docdId = $(this).attr('id');
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: 'Documents/DeleteEmployee/',
      type: 'POST',
      headers: { 'RequestVerificationToken': token },
      data: {
        'id': docdId,
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          localStorage.setItem('docId',currentDocId);
          location.reload();
        } else {
          alert('An error occurred while deleting the employee from document.');
        }
      }
    });
  }
  if (triggerEditEmp) {
    var docdId = $(this).attr('id');
    modalAcceptBtn.id = 'modal-editEmp-button';
    modal.classList.toggle('display');
    const modalBody = $('<form id="modal-form" autocomplete="off"></form>');
    $('#modal-title').html('<div>Edit employee in document</div>');
    $('#modal-body').html(modalBody);
    $('#modal-body').append(`<input type="hidden" id=docdId value="${docdId}"/>`)
    setSelectJob();
    setSelectTitle();
    $('#modal-form').append('<label for="firstName">First Name</label><br />');
    $('#modal-form').append('<input id="firstName" disabled/><br />');
    $('#modal-form').append('<label for="lastName">Last Name</label><br />');
    $('#modal-form').append('<input id="lastName" disabled/><br />');
    $('#modal-form').append('<label for="remark">Remark</label><br />');
    $('#modal-form').append('<input id="remark"/><br />');
    $('#modal-form').append('<span class="text-danger"></span>');
    $('#modal-form').append('<span id="disp-alert" class="text-success"></span>');
    $.ajax({
      url: 'Documents/UpdateEmployee/',
      type: 'GET',
      data: { 'id': docdId },
      success: function(result) {
        $('#selectJob').val(result.documentDetail.Job_Id);
        $('#selectRank').val(result.documentDetail.Rank_Id);
        $('#firstName').val(result.documentDetail.Employee.FirstName);
        $('#lastName').val(result.documentDetail.Employee.LastName);
        $('#remark').val(result.documentDetail.Remark);
      }
    });
  }
});

/*----------- Click confirm button on Modal -----------*/
// when click confirm add employee button on modal
// send method post to update data
$(document).on('click', '#modal-addEmp-button', () => {
  if (validateForm()) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: 'Documents/AddEmployee/',
      type: 'POST',
      headers: { 'RequestVerificationToken': token },
      data: {
        'id': currentDocId,
        'jobId': $('#selectJob').val(),
        'rankId': $('#selectRank').val(),
        'firstName': $('#firstName').val(),
        'lastName': $('#lastName').val(),
        'remark': $('#remark').val(),
        '__RequestVerificationToken': token
      },
      success: function(result) {
        $('#disp-alert').html('Successfully added employees');
        $('#modal-form input').val("");
        triggerReload = true;
      }
    });
  }
});

$(document).on('click', '.modal-closeAddEmp-button', () => {
  localStorage.setItem('docId',currentDocId);
  triggerReload ? location.reload() : null;
});

// when click confirm update op button on modal
// send method post to update data
$(document).on('click', '#modal-upop-button', () => {
  if (validateForm()){
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dateSplit = $('#opdate').val().split('/');
    var jsDay = dateSplit[0];
    var jsMonth = dateSplit[1];
    var jsYear = dateSplit[2];
    var conJSDate = jsMonth + "/" + jsDay + "/" + jsYear;
    $.ajax({
      url: 'Documents/UpdateOP/',
      type: 'POST',
      headers: { 'RequestVerificationToken': token },
      data: {
        'id': currentDocId,
        'operation': $('#oplocate').val(),
        'operationDate': conJSDate,
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          localStorage.setItem('docId',currentDocId);
          location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
}); // end click modal-upop-button

$(document).on('click', '#modal-editEmp-button', () => {
  if (validateForm()) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: 'Documents/UpdateEmployee/',
      type: 'POST',
      headers: { 'RequestVerificationToken': token },
      data: {
        'id': $('#docdId').val(),
        'jobId': $('#selectJob').val(),
        'rankId': $('#selectRank').val(),
        'remark': $('#remark').val(),
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          localStorage.setItem('docId',currentDocId);
          location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
});
// when click confirm submit button on modal
// send method post to update data
$(document).on('click', '#modal-sub-button', () => {
  if(validateForm()){
    var token = $('input[name="__RequestVerificationToken"]').val();
    var dateSplit = $('#endDoc').val().split('/');
    var jsDay = dateSplit[0];
    var jsMonth = dateSplit[1];
    var jsYear = dateSplit[2];
    var conJSDate = jsMonth + "/" + jsDay + "/" + jsYear;
    $.ajax({
      url: 'Documents/UpdateEndDate/',
      type: 'POST',
      headers: { 'RequsetVerificationToken': token },
      data: {
        'id': currentDocId,
        'endDate': conJSDate,
        '__RequestVerificationToken': token
      },
      success: function(result) {
        if (result.success) {
          localStorage.setItem('docId',currentDocId);
          location.reload();
        } else {
          alert('An error occurred while deleting the document.');
        }
      }
    });
  }
}); // end click modal-upop-button

// when click confirm delete button on modal
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

//When select new Job on modal
$(document).on('change', '#selectJob', function() {
  $('#selectRank').empty();
  $.ajax({
    url: 'Documents/GetAllRanks/',
    type: 'GET',
    async:  false,
    dataType: 'json',
    data: { 'id': $('#selectJob').val() },
    success: function (result) {
      result.forEach(rank => {
        const optionRank =  '<option value="' + rank.id + '">' + rank.title + '</option>';
        $('#selectRank').append(optionRank);
      })
    } // End success
  }); // End ajax
});

// validate Operation edit form
function validateForm() {
  var datePattern = /^(0?[1-9]|[12][0-9]|3[01])[\/](0?[1-9]|[1][012])[\/]\d{4}$/;
  var isValid = true;

  $('#modal-form input').each(function() {
    var spanElement = $(this).next().next();

    if ($(this).val() == '' && !$(this).has("#remark")) {
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

function setSelectJob() {
  const labelJob = $('<label for="selectJob">Job</label><br />');
  const selectJob = $('<select id="selectJob"></select>');
  $.ajax({
    url: 'Documents/GetAllJobs',
    type: 'GET',
    async:  false,
    dataType: 'json',
    success: function(result) {
      result.forEach(job => {
        const optionJob = '<option value="' + job.id + '">' + job.title + '</option>';
        selectJob.append(optionJob);
      });
    } // End success
  }); // End ajax
  $('#modal-form').append(labelJob);
  $('#modal-form').append(selectJob);
  $('#modal-form').append('<br />');
}

function setSelectTitle() {
  const labelRank = $('<label for="selectRank">Title</label><br />');
  const selectRank = $('<select id="selectRank"></select>');
  $.ajax({
    url: 'Documents/GetAllRanks/',
    type: 'GET',
    async:  false,
    dataType: 'json',
    data: { 'id': $('#selectJob').val() },
    success: function (result) {
      result.forEach(rank => {
        const optionRank =  '<option value="' + rank.id + '">' + rank.title + '</option>';
        selectRank.append(optionRank);
      })
    } // End success
  }); // End ajax
  $('#modal-form').append(labelRank);
  $('#modal-form').append(selectRank);
  $('#modal-form').append('<br />');
}

function cancelTrigger() {
  triggerDelEmp = false;
  triggerEditEmp = false;
  $('#del-emp-icon').removeClass('select');
  $('#edit-emp-icon').removeClass('select');
  $('.sub-row').css('cursor', 'default');
}

function disableSwipe(docId) {
  if (docId){
    $(`#btnBehide-${docId.replace('/','\\/').replace('.','\\.')}`).removeClass('swipe');
    $(`#btnFront-${docId.replace('/','\\/').replace('.','\\.')}`).removeClass('swipe');
    $(`#${docId.replace('/','\\/').replace('.','\\.')}`).removeAttr('style');
  }
}

//Search function
$(document).on('keypress', '#search-docNo, #search-docType, #search-docTitle, #search-employee', function(event) {
  if (event.key === "Enter") {
    event.preventDefault();
    document.getElementById('search-document-btn').click();
  }
});

$(document).on('click', '#search-document-btn', function() {
  queryDocNo = $('#search-docNo').val();
  queryDocType = $('#search-docType').val();
  queryDocTitle = $('#search-docTitle').val();
  queryEmployee = $('#search-employee').val();
  tabType;
  if ($('#tab-all').hasClass('active')){
    tabType = "all";
  }else{
    tabType = "user";
  }
  isEndOfData = false;
  page = 1;
  $.ajax({
    url: 'Documents/Index',
    type: 'GET',
    data: { 
      'queryDocNo': queryDocNo,
      'queryDocType': queryDocType,
      'queryDocTitle': queryDocTitle,
      'queryEmployee': queryEmployee,
      'tabType': tabType,
      'isSearch': true
    },
    success: function(data)
    {
      $('.search-contrainer').toggleClass('expand');
      $('#sort-row').html(data);
      initialBuddhist();
      floatingBtn.setAttribute('disabled', true);
      floatingBtn.checked = false;
      currentDocId = null;
    }
  });
})

$(document).on('click', '.search-expand-btn', function() {
  $('.search-contrainer').toggleClass('expand');
});

$(document).on('click', '#tab-user', function() {
  if (!$('#tab-user').hasClass('active')){
    $('#tab-user').addClass('active');
    $('#tab-all').removeClass('active');

    document.getElementById('search-document-btn').click();
  }
});

$(document).on('click', '#tab-all', function() {
  if (!$('#tab-all').hasClass('active')){
    $('#tab-all').addClass('active');
    $('#tab-user').removeClass('active');

    document.getElementById('search-document-btn').click();
  }
});

// Export Excel
$(document).on('click', '#ex-excel', function() {
  showLoadingScreen();
  $.ajax({
    url: 'Documents/ExportExcel',
    type: 'GET',
    xhrFields: {
        responseType: 'blob' // Set the response type to 'blob'
    },
    success: function (data) {
      hideLoadingScreen();
      var a = document.createElement('a');
      var url = window.URL.createObjectURL(data);
      a.href = url;
      a.download = 'document_tracking.xlsx';
      a.style.display = 'none';
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
    },
    error: function (xhr, status, error) {
      // Handle the error
      console.log(error);
    }
  });
});

// Import Excel
$(document).on('click', '#im-excel', function() {
  selectFile();
})

function selectFile() {
  var fileInput = document.createElement('input');
  fileInput.type = 'file';
  fileInput.accept = '.xlsx, .xls';
  fileInput.style.display = 'none';
  fileInput.addEventListener('change', function(event) {
    var selectedFile = event.target.files[0];
    importFile(selectedFile);
  });

  document.body.appendChild(fileInput);
  fileInput.click();
}

function importFile(file) {
  if (file) {
    showLoadingScreen();
    var formData = new FormData();
    formData.append('file', file);
    $.ajax({
      url: '/Documents/ImportExcel',
      type: 'POST',
      data: formData,
      processData: false,
      contentType: false,
      success: function(response) {
        hideLoadingScreen();
        window.location.reload();
        // File imported successfully
        // console.log(response);
      },
      error: function(xhr, status, error) {
        // Error occurred during file import
        console.error('Error', error);
      }
    });
  }
}

$(document).ready(function() {
  eleOverflow = $('.container-wrapper').css("overflow");
})

$(window).on('resize', function() {
  eleOverflow = $('.container-wrapper').css("overflow");
})

function isContScrollAtBottom() {
  var scrollTop = $('.container-wrapper').scrollTop();
  var pageYOffset = $('.container-wrapper').offset().top;
  var scrollHeight = $('.container-wrapper').prop('scrollHeight');

  return (scrollTop + pageYOffset) >= scrollHeight;
}

$('.container-wrapper').scroll(function() {
  var loadSkip = page * 20;
  if (eleOverflow === 'scroll' && isContScrollAtBottom() && isLoadData && !isEndOfData) {
    isLoadData = false;
    $.ajax({
      url: 'Documents/Index',
      type: 'GET',
      data: { 
        'queryDocNo': queryDocNo,
        'queryDocType': queryDocType,
        'queryDocTitle': queryDocTitle,
        'queryEmployee': queryEmployee,
        'tabType': tabType,
        'loadSkip': loadSkip,
        'isSearch': true
      },
      success: function(data) {
        if (!data.includes("Search Not Found")) {
          $('.search-contrainer').toggleClass('expand');
          var sortRow =document.getElementById('sort-row');
          sortRow.insertAdjacentHTML('beforeEnd', data);
          initialBuddhist();
          floatingBtn.setAttribute('disabled', true);
          floatingBtn.checked = false;
          currentDocId = null;
          page++;

          setTimeout(function() {
            isLoadData = true;
          }, 1000);
        }else {
          isEndOfData = true;
          isLoadData = true;
        }
      },
    });
  }
})

window.addEventListener('scroll', function() {
  var header = $('.stick-header');
  var scrollTop = $(this.window).scrollTop();
  var headerTop = header.offset().top;
  if (headerTop === scrollTop) {
    header.css('border-radius', '0 0 0 0');
    header.css('transform', 'scaleX(1.04)');
  } else {
    header.css('transition','border-radius, .1s ease-out');
    header.css('border-radius', '15px 15px 0 0');
    header.css('transform', 'scaleX(1.00)')
  }
  var loadSkip = page * 20;
  if (eleOverflow === 'visible' && isWinScrollAtBottom() && isLoadData && !isEndOfData) {
    $.ajax({
      url: 'Documents/Index',
      type: 'GET',
      data: { 
        'queryDocNo': queryDocNo,
        'queryDocType': queryDocType,
        'queryDocTitle': queryDocTitle,
        'queryEmployee': queryEmployee,
        'tabType': tabType,
        'loadSkip': loadSkip,
        'isSearch': true
      },
      success: function(data) {
        if (!data.includes("Search Not Found")) {
          $('.search-contrainer').toggleClass('expand');
          var sortRow =document.getElementById('sort-row');
          sortRow.insertAdjacentHTML('beforeEnd', data);
          initialBuddhist();
          floatingBtn.setAttribute('disabled', true);
          floatingBtn.checked = false;
          currentDocId = null;
          page++;
        }else {
          isEndOfData = true;
        }
      },
    });
  } // End if scroll window
});