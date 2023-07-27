const annoModal = document.getElementById("annoModal");
const annoModalBody = document.getElementById("anno-modal-body");

$(document).on('click', '#clear-emp-btn', () => {
  modal.classList.add('display');
});

$(document).on('click', '#modal-accept-button', function() {
  showLoadingScreen();
  modal.classList.remove('display');
  $.ajax({
    url: 'Employees/ClearEmployees',
    type: 'POST',
    success: function(data) {
      hideLoadingScreen();
      $('#document-table').html(data.html);
      if (data.hasDelete) {
        annoModalBody.innerHTML = "Completely clear employee is non document.";
        annoModal.classList.add('display');
      }else {
        annoModalBody.innerHTML = "Don't have any employees are non document was clear.";
        annoModal.classList.add('display');
      }
    }
  })
});

window.onclick = function(event) {
  if (event.target == annoModal)
  {
    annoModal.classList.remove('display');
  }
}

$(document).on('click', '#anno-span-close', function() {
  annoModal.classList.remove('display');
});

$(document).on('click', '#anno-modal-button', function() {
  annoModal.classList.remove('display');
});