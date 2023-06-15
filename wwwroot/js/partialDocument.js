const threshold = 30;
var touchStartX = 0;
var touchEndX = 0;
var distance = 0;
var subHeight = 0;
var isLoadData = true;

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
      const mainRowElement = $(`#${currentDocId.replace('/','\\/').replace('.','\\.')}`).parent();
      const subRowElement = $(`#sub-${currentDocId.replace('/','\\/').replace('.','\\.')}`);
      const rowFooterElement = subRowElement.next();
      if (mainRowElement.hasClass('active')) {
        disableActive();
        floatingBtn.setAttribute('disabled', true);
        floatingBtn.checked = false;
        if(isScrollAtBottom()) {
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
    $(`#btn-${currentDocId.replace('/','\\/').replace('.','\\.')}`).removeClass('swipe');
    $(this).removeAttr('style');
    isMove = false;
  }
  else if (distance < 0) {
      $(this).parent().hasClass('active')? null: disableActive();
      currentDocId = $(this).attr('id');
      $(this).css('transform', 'translate(200px,0px)');
      $(this).css('border-radius', 'unset');
      $(`#btnFront-${currentDocId.replace('/','\\/').replace('.','\\.')}`).addClass('swipe');
      isMove = true;
  }
  else {
      $(this).parent().hasClass('active')? null: disableActive();
      currentDocId = $(this).attr('id');
      $(this).css('transform', 'translate(-200px,0px)');
      $(this).css('border-radius', 'unset');
      $(`#btnBehide-${currentDocId.replace('/','\\/').replace('.','\\.')}`).addClass('swipe');
      isMove = true;
  }
});

function isScrollAtBottom() {
  var scrollPosition = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
  var windowHeight = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
  var documentHeight = Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);

  return (scrollPosition + windowHeight) >= documentHeight;
}
  
function initialBuddhist() {
  // Set date buddhist format
  var receiptDates = document.querySelectorAll('.receipt-date');
  var operationDates = document.querySelectorAll('.operation-date');
  var endDates = document.querySelectorAll('.end-date');
  var buddhistOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    calendar: 'buddhist',
    // numberingSystem: 'thai'
  };
  
  var thaiBuddhistFormat = new Intl.DateTimeFormat('th-TH-u-ca-buddhist', buddhistOptions);
  receiptDates.forEach(div => {
    receiptDate = div.getAttribute('data-receiptDate');
    var [day, month, year] = receiptDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
  
  operationDates.forEach(div => {
    operationDate = div.getAttribute('data-operationDate');
    if (operationDate == "") return;
    var [day, month, year] = operationDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
  
  endDates.forEach(div => {
    endDate = div.getAttribute('data-endDate');
    if (endDate == "") return;
    var [day, month, year] = endDate.split('/').map(Number);
    date = new Date(year, month-1, day);
    div.textContent = thaiBuddhistFormat.format(date);
  });
}

// $('.main-row').swipe({
//     swipeLeft: function(){
//       disableSwipe(currentDocId);
//       $(this).parent().hasClass('active')? null: disableActive();
//       currentDocId = $(this).attr('id');
//       $(this).css('transform', 'translate(-200px,0px)');
//       $(this).css('border-radius', 'unset');
//       $(`#btnBehide-${currentDocId.replace('/','\\/').replace('.','\\.')}`).addClass('swipe');
//       isMove = true;
//     },
//     swipeRight: function(){
//       disableSwipe(currentDocId);
//       $(this).parent().hasClass('active')? null: disableActive();
//       currentDocId = $(this).attr('id');
//       $(this).css('transform', 'translate(200px,0px)');
//       $(this).css('border-radius', 'unset');
//       $(`#btnFront-${currentDocId.replace('/','\\/').replace('.','\\.')}`).addClass('swipe');
//       isMove = true;
//     },
//     swipeStatus: function(event,phase, direction, distance) {
//       if (phase == 'cancel')
//       {
//         disableSwipe(currentDocId);
//         if (distance < 5 && !isMove){
//           currentDocId = $(this).attr('id');
//           const mainRowElement = $(`#${currentDocId.replace('/','\\/').replace('.','\\.')}`).parent();
//           const subRowElement = $(`#sub-${currentDocId.replace('/','\\/').replace('.','\\.')}`);
//           const rowFooterElement = subRowElement.next();
//           if (mainRowElement.hasClass('active')) {
//             disableActive();
//             floatingBtn.setAttribute('disabled', true);
//             floatingBtn.checked = false;
//           }else {
//             disableActive();
//             activeElement = mainRowElement.addClass('active');
//             expandElement = subRowElement.addClass('expand');
//             footerElement = rowFooterElement.addClass('row-footer');
//             floatingBtn.removeAttribute('disabled');
//           }
//         }
//         $(`#btn-${currentDocId.replace('/','\\/').replace('.','\\.')}`).removeClass('swipe');
//         $(this).removeAttr('style');
//         isMove = false;
//       }
//     },
//     threshold: 30,
//     allowPageScroll: "vertical"
// });