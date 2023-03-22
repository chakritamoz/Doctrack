$('#search-btn').click(function() {
  const queryStr = $('#search-box').val();
  console.log(queryStr);
  $.ajax({
    url: '/DocumentTypes/Index/',
    type: 'GET',
    data: {
      'queryStr': queryStr
    },
    success: function (data) { 
      Model = data;
      console.log(data);
    }
  })
})