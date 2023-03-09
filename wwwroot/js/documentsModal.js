const modal = document.getElementById("writableModal");
const modalBody = document.getElementById("modal-body");
const modalBodyDet = document.getElementById("modal-body-detail");
const modalCloseBtn = document.getElementById("modal-close-button");
const modalAcceptBtn = document.getElementById("modal-accept-button");
const spanClose = document.getElementsByClassName("close")[0];

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