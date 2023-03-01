const addEmpIcon = document.getElementById("add-emp-icon");
const editDocIcon = document.getElementById("edit-doc-icon");
const delDocIcon = document.getElementById("del-doc-icon");
const upOpDocIcon = document.getElementById("upop-doc-icon");
const subDocIcon = document.getElementById("sub-doc-icon");
const floatingBtn = document.getElementById("fabCheckbox");

const modal = document.getElementById("writableModal");
const modalTitle = document.getElementById("modal-title");
const modalBody = document.getElementById("modal-body");
const modalBodyDet = document.getElementById("modal-body-detail");
const modalCloseBtn = document.getElementById("modal-close-button");
const modalAcceptBtn = document.getElementById("modal-accept-button");
const spanClose = document.getElementsByClassName("close")[0];
var currentDocId = "";

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
  addEmpIcon.setAttribute('href','Documents/AddEmployee/' + docId);
  editDocIcon.setAttribute('href','Documents/Edit/' + docId);
  // delDocIcon.setAttribute('href','Documents/Delete/' + docId);
  upOpDocIcon.setAttribute('href','Documents/UpdateOperation/' + docId);
  subDocIcon.setAttribute('href','Documents/SubmitDocument/' + docId);
}

delDocIcon.onclick = () => {
  modal.classList.toggle("display")
  modalTitle.innerHTML = "Alert delete document";
  modalBody.innerHTML = "Are you sure you want to delete document?";
  modalBodyDet.innerHTML = 'Document ID: <span style="color:red">' + currentDocId + '</span>';
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