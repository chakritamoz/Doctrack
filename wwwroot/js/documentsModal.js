const modal = document.getElementById("writableModal");
const modalBody = document.getElementById("modal-body");
const modalBodyDet = document.getElementById("modal-body-detail");
const modalAcceptBtn = document.getElementById("modal-accept-button");

$(document).on('click', '#modal-span-close', function() {
  modal.classList.remove('display');
});

$(document).on('click', '#modal-close-button', function() {
  modal.classList.remove('display');
})

window.onclick = function(event) {
  console.log(event)
  if (event.target == modal)
  {
    modal.classList.remove("display");
  }
}