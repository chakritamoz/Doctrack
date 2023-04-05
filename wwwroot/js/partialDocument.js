$('.main-row').swipe({
  swipeLeft: function(){
    disableSwipe(currentDocId);
    $(this).parent().hasClass('active')? null: disableActive();
    currentDocId = $(this).attr('id');
    $(this).css('transform', 'translate(-200px,0px)');
    $(`#btnBehide-${currentDocId}`).addClass('swipe');
    isMove = true;
  },
  swipeRight: function(){
    disableSwipe(currentDocId);
    $(this).parent().hasClass('active')? null: disableActive();
    currentDocId = $(this).attr('id');
    $(this).css('transform', 'translate(200px,0px)');
    $(`#btnFront-${currentDocId}`).addClass('swipe');
    isMove = true;
  },
  swipeStatus: function(event,phase, direction, distance) {
    if (phase == 'cancel')
    {
      disableSwipe(currentDocId);
      if (distance < 5 && !isMove){
        currentDocId = $(this).attr('id');
        const mainRowElement = $(`#${currentDocId}`).parent();
        const subRowElement = $(`#sub-${currentDocId}`);
        const rowFooterElement = subRowElement.next();
        if (mainRowElement.hasClass('active')) {
          disableActive();
          floatingBtn.setAttribute('disabled', true);
          floatingBtn.checked = false;
        }else {
          disableActive();
          activeElement = mainRowElement.addClass('active');
          expandElement = subRowElement.addClass('expand');
          footerElement = rowFooterElement.addClass('row-footer');
          floatingBtn.removeAttribute('disabled');
        }
      }
      $(`#btn-${currentDocId}`).removeClass('swipe');
      $(this).removeAttr('style');
      isMove = false;
    }
  },
  threshold: 30,
  allowPageScroll: "vertical"
  });