const threshold = 30;
let touchStartX = 0;
let touchEndX = 0;
let distance = 0;
let subHeight = 0;
let isLoadData = true;

$(document).on('mousedown', '.main-row', function(event) {
  touchStartX = event.clientX;
});

$(document).on('mouseup', '.main-row', function(event) {
  disableSwipe(currentDocId);
  touchEndX = event.clientX;
  distance = touchStartX - touchEndX;

  if (Math.abs(distance) < threshold) {
    if (!isMove){
      currentDocId = $(this).attr('id');
      const mainRowElement = $(`#${currentDocId
        .replace('/','\\/')
        .replace('.','\\.')
        .replace('(','\\(')
        .replace(')','\\)')
      }`).parent();
      const subRowElement = $(`#sub-${currentDocId
        .replace('/','\\/')
        .replace('.','\\.')
        .replace('(','\\(')
        .replace(')','\\)')
      }`);
      const rowFooterElement = subRowElement.next();
      if (mainRowElement.hasClass('active')) {
        disableActive();
        floatingBtn.setAttribute('disabled', true);
        floatingBtn.checked = false;
        if(isWinScrollAtBottom()) {
          window.scrollBy({
            top: -100,
            left: 0,
            behavior: "smooth"}
          );
        }
        // Status true for load dynamic data 
        // when scroll is on bottom of page
        setTimeout(function() {
          isLoadData = true;
        }, 1000);
      }else {
        // Status false for block load dynamic data 
        // when scroll is on bottom of page
        isLoadData = false;
        disableActive();
        activeElement = mainRowElement.addClass('active');
        expandElement = subRowElement.addClass('expand');
        footerElement = rowFooterElement.addClass('row-footer');
        floatingBtn.removeAttribute('disabled');
      }
    }
    $(`#btn-${currentDocId
        .replace('/','\\/')
        .replace('.','\\.')
        .replace('(','\\(')
        .replace(')','\\)')
    }`).removeClass('swipe');
    $(this).removeAttr('style');
    isMove = false;
  }
  else if (distance < 0) {
      $(this).parent().hasClass('active')? null: disableActive();
      currentDocId = $(this).attr('id');
      $(this).css('transform', 'translate(200px,0px)');
      $(this).css('border-radius', 'unset');
      $(`#btnFront-${currentDocId
        .replace('/','\\/')
        .replace('.','\\.')
        .replace('(','\\(')
        .replace(')','\\)')
      }`).addClass('swipe');
      isMove = true;
  }
  else {
      $(this).parent().hasClass('active')? null: disableActive();
      currentDocId = $(this).attr('id');
      $(this).css('transform', 'translate(-200px,0px)');
      $(this).css('border-radius', 'unset');
      $(`#btnBehide-${currentDocId
        .replace('/','\\/')
        .replace('.','\\.')
        .replace('(','\\(')
        .replace(')','\\)')
      }`).addClass('swipe');
      isMove = true;
  }
});

function isWinScrollAtBottom() {
  const scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
  const windowHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
  const documentHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
  
  return (scrollPosition + windowHeight) >= documentHeight;
}
  
function initialBuddhist() {
  // Set date buddhist format
  const receiptDates = document.querySelectorAll('.receipt-date');
  const operationDates = document.querySelectorAll('.operation-date');
  const endDates = document.querySelectorAll('.end-date');
  const buddhistOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    calendar: 'buddhist',
    // numberingSystem: 'thai'
  };
  
  const thaiBuddhistFormat = new Intl.DateTimeFormat('th-TH-u-ca-buddhist', buddhistOptions);
  receiptDates.forEach(div => {
    receiptDate = div.getAttribute('data-receiptDate');
    const [day, month, year] = receiptDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
  
  operationDates.forEach(div => {
    operationDate = div.getAttribute('data-operationDate');
    if (operationDate == "") return;
    const [day, month, year] = operationDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
  
  endDates.forEach(div => {
    endDate = div.getAttribute('data-endDate');
    if (endDate == "") return;
    const [day, month, year] = endDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
}

function disableActive() {
  if (activeElement || expandElement || footerElement){
    activeElement.removeClass('active');
    expandElement.removeClass('expand');
    footerElement.removeClass('row-footer');
  }
}